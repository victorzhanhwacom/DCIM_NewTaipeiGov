using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;
using Unity.Mathematics;

namespace Heatmap
{
    [RequireComponent(typeof(MeshRenderer))]
    public class HeatmapDataManager : MonoBehaviour
    {
        [Header("Volume 解析度")]
        public int resX = 64, resY = 64, resZ = 64;

        [Header("Volume 世界空間邊界（對應 Shader _BoxMin/_BoxMax）")]
        public Vector3 volumeMin = new Vector3(-0.5f, -0.5f, -0.5f);
        public Vector3 volumeMax = new Vector3(0.5f, 0.5f, 0.5f);

        [Header("更新設定")]
        public float updateInterval = 0.05f;  // 秒，20fps 更新
        public bool autoFindSources = true;  // 自動搜尋場景內所有 HeatSource

        [Header("手動指定熱源（autoFind 關閉時使用）")]
        public List<HeatSource> heatSources = new List<HeatSource>();

        [Header("疊加模式")]
        [Tooltip("多個熱源密度相加後的上限")]
        public float maxAccumulatedDensity = 1.5f;

        private Texture3D _densityTex;
        private Material _mat;
        private float[] _densityBuffer;   // 用 float[] 比 Color[] 快

        void Start()
        {
            _mat = GetComponent<MeshRenderer>().material;

            // 產生並套用 HeatRamp
            Texture2D ramp = HeatRampGenerator.Create(18, 32);
            _mat.SetTexture("_HeatRamp", ramp);

            _densityBuffer = new float[resX * resY * resZ];

            _densityTex = new Texture3D(resX, resY, resZ,
                                        TextureFormat.RFloat, mipChain: false);
            _densityTex.filterMode = FilterMode.Bilinear;
            _densityTex.wrapMode = TextureWrapMode.Clamp;

            _mat.SetTexture("_DensityTex", _densityTex);
            _mat.SetVector("_BoxMin", new Vector4(volumeMin.x, volumeMin.y, volumeMin.z, 0));
            _mat.SetVector("_BoxMax", new Vector4(volumeMax.x, volumeMax.y, volumeMax.z, 0));

            StartCoroutine(UpdateLoop());
        }

        // ── 主更新迴圈 ───────────────────────────────────────────────
        IEnumerator UpdateLoop()
        {
            while (true)
            {
                // BakeHeatSourcesJobs();
                BakeHeatSources();
                yield return new WaitForSeconds(updateInterval);
            }
        }

        void BakeHeatSources()
        {
            // 1. 收集熱源列表
            if (autoFindSources)
                heatSources = new List<HeatSource>(
                    FindObjectsByType<HeatSource>(FindObjectsSortMode.None));

            if (heatSources.Count == 0)
            {
                System.Array.Clear(_densityBuffer, 0, _densityBuffer.Length);
                UploadBuffer();
                return;
            }

            // 2. 預先將熱源世界座標轉為 Volume UV 空間
            //    這樣內層迴圈不用重複做空間轉換
            int srcCount = heatSources.Count;
            var srcUV = new Vector3[srcCount];
            var srcTemp = new float[srcCount];
            var srcSig2 = new float[srcCount];   // 2 * sigma^2（預算，避免迴圈重複）
            var srcFall = new float[srcCount];

            Vector3 volSize = volumeMax - volumeMin;

            for (int s = 0; s < srcCount; s++)
            {
                HeatSource src = heatSources[s];

                // 世界座標 → Volume UV（需考慮 Volume 物件本身的 Transform）
                Vector3 localPos = transform.InverseTransformPoint(src.transform.position);
                // localPos 在 volumeMin ~ volumeMax 空間
                srcUV[s] = new Vector3(
                    (localPos.x - volumeMin.x) / volSize.x,
                    (localPos.y - volumeMin.y) / volSize.y,
                    (localPos.z - volumeMin.z) / volSize.z);

                // 原本
                srcTemp[s] = src.Temperature;

                // 改後：溫度 → 正規化密度
                /* srcTemp[s] = Mathf.Clamp01(
                    (src.Temperature - 18) / (32 - 18)); */

                // sigma 也轉為 UV 空間（取 X 軸為基準）
                float sigmaUV = src.Radius / volSize.x;
                srcSig2[s] = 2f * sigmaUV * sigmaUV;
                srcFall[s] = src.Falloff;
            }

            // 3. 烘焙密度場
            float invResX = 1f / (resX - 1);
            float invResY = 1f / (resY - 1);
            float invResZ = 1f / (resZ - 1);

            for (int z = 0; z < resZ; z++)
            {
                float uz = z * invResZ;
                for (int y = 0; y < resY; y++)
                {
                    float uy = y * invResY;
                    for (int x = 0; x < resX; x++)
                    {
                        float ux = x * invResX;
                        float density = 0f;

                        for (int s = 0; s < srcCount; s++)
                        {
                            float dx = ux - srcUV[s].x;
                            float dy = uy - srcUV[s].y;
                            float dz = uz - srcUV[s].z;
                            float d2 = dx * dx + dy * dy + dz * dz;

                            // Gaussian 衰減
                            float g = Mathf.Exp(-Mathf.Pow(d2, srcFall[s] * 0.5f)
                                                / srcSig2[s]);
                            density += g * srcTemp[s];
                        }

                        _densityBuffer[x + y * resX + z * resX * resY] =
                            Mathf.Clamp01(density / maxAccumulatedDensity);
                    }
                }
            }

            UploadBuffer();
        }

        void UploadBuffer()
        {
            _densityTex.SetPixelData(_densityBuffer, mipLevel: 0);
            _densityTex.Apply(updateMipmaps: false, makeNoLongerReadable: false);
        }

        // ── 公開 API：外部動態新增/移除熱源 ─────────────────────────
        public void RegisterSource(HeatSource src)
        {
            if (!heatSources.Contains(src))
                heatSources.Add(src);
        }

        public void UnregisterSource(HeatSource src)
        {
            heatSources.Remove(src);
        }

#if UNITY_EDITOR
        // Gizmo：在 Scene View 顯示 Volume 邊界
        void OnDrawGizmosSelected()
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = new Color(1f, 0.4f, 0f, 0.3f);
            Vector3 center = (volumeMin + volumeMax) * 0.5f;
            Vector3 size = volumeMax - volumeMin;
            Gizmos.DrawWireCube(center, size);
        }
#endif
        // ── 在 HeatmapDataManager 中替換 BakeHeatSources() ──────────
        void BakeHeatSourcesJobs()
        {
            if (autoFindSources)
                heatSources = new List<HeatSource>(
                    FindObjectsByType<HeatSource>(FindObjectsSortMode.None));

            int srcCount = heatSources.Count;
            int total = resX * resY * resZ;

            // 準備 NativeArray
            var output = new NativeArray<float>(total, Allocator.TempJob);
            var sources = new NativeArray<HeatSourceData>(srcCount, Allocator.TempJob);

            Vector3 volSize = volumeMax - volumeMin;

            for (int s = 0; s < srcCount; s++)
            {
                HeatSource src = heatSources[s];
                Vector3 localPos = transform.InverseTransformPoint(src.transform.position);
                float3 uv = new float3(
                    (localPos.x - volumeMin.x) / volSize.x,
                    (localPos.y - volumeMin.y) / volSize.y,
                    (localPos.z - volumeMin.z) / volSize.z);

                float sigmaUV = src.Radius / volSize.x;

                sources[s] = new HeatSourceData
                {
                    uvPos = uv,
                    temp = src.Temperature,
                    sig2 = 2f * sigmaUV * sigmaUV,
                    falloff = src.Falloff,
                };
            }

            var job = new BakeDensityJob
            {
                resX = resX,
                resY = resY,
                resZ = resZ,
                sources = sources,
                maxDensity = maxAccumulatedDensity,
                output = output,
            };

            JobHandle handle = job.Schedule(total, 64);  // 每批 64 個 voxel
            handle.Complete();

            _densityTex.SetPixelData(output, mipLevel: 0);
            _densityTex.Apply(false, false);

            output.Dispose();
            sources.Dispose();
        }


    }



    // ── Job 結構 ─────────────────────────────────────────────────
    struct HeatSourceData
    {
        public float3 uvPos;
        public float temp;
        public float sig2;
        public float falloff;
    }

    [BurstCompile]
    struct BakeDensityJob : IJobParallelFor
    {
        [ReadOnly] public int resX, resY, resZ;
        [ReadOnly] public NativeArray<HeatSourceData> sources;
        [ReadOnly] public float maxDensity;

        [WriteOnly] public NativeArray<float> output;

        public void Execute(int idx)
        {
            int x = idx % resX;
            int y = (idx / resX) % resY;
            int z = idx / (resX * resY);

            float ux = x / (float)(resX - 1);
            float uy = y / (float)(resY - 1);
            float uz = z / (float)(resZ - 1);

            float density = 0f;

            for (int s = 0; s < sources.Length; s++)
            {
                float3 d = new float3(ux, uy, uz) - sources[s].uvPos;
                float d2 = math.dot(d, d);
                float g = math.exp(-math.pow(d2, sources[s].falloff * 0.5f)
                                     / sources[s].sig2);
                density += g * sources[s].temp;
            }

            output[idx] = math.clamp(density / maxDensity, 0f, 1f);
        }
    }
}