using UnityEngine;

[ExecuteAlways]
public class HeatmapRenderer : MonoBehaviour
{
    [Header("Volume Bounds")]
    public Vector3 boxMin = new Vector3(-0.5f, -0.5f, -0.5f);
    public Vector3 boxMax = new Vector3( 0.5f,  0.5f,  0.5f);

    private Material _mat;

    void OnEnable()
    {
        // 確保 MeshFilter 有 Cube
        if (!GetComponent<MeshFilter>())
        {
            var mf = gameObject.AddComponent<MeshFilter>();
            // 用 Unity 內建 Cube primitive 的 mesh
            mf.mesh = Resources.GetBuiltinResource<Mesh>("Cube.fbx");
        }
        _mat = GetComponent<MeshRenderer>()?.sharedMaterial;
    }

    void Update()
    {
        if (_mat == null) return;
        _mat.SetVector("_BoxMin", new Vector4(boxMin.x, boxMin.y, boxMin.z, 0));
        _mat.SetVector("_BoxMax", new Vector4(boxMax.x, boxMax.y, boxMax.z, 0));
    }
}