Shader "Custom/VolumetricHeatmap"
{
    Properties
    {
        _HeatRamp         ("Heat Color Ramp",   2D)    = "white" {}
        _DensityTex       ("Density Volume",    3D)    = "white" {}
        _StepCount        ("Step Count",        Range(16, 256))  = 64
        _Density          ("Density Scale",     Range(0.1, 10))  = 2.0
        _AlphaThreshold   ("Alpha Cutoff",      Range(0.0, 0.99)) = 0.95
        _BoxMin           ("Box Min",           Vector) = (-0.5,-0.5,-0.5,0)
        _BoxMax           ("Box Max",           Vector) = ( 0.5, 0.5, 0.5,0)

        [Header(Heat Rise Animation)]
        _RiseSpeed        ("Rise Speed",        Range(0.0, 2.0))  = 0.3
        _TurbulenceScale  ("Turbulence Scale",  Range(0.5, 8.0))  = 3.0
        _TurbulenceStrength ("Turbulence Str",  Range(0.0, 0.3))  = 0.08
        _SwayAmount       ("Sway Amount",       Range(0.0, 0.2))  = 0.04
        _HeightFalloff    ("Height Falloff",    Range(0.0, 5.0))  = 2.0

        _RampBias    ("Ramp Bias（往紅端推）",  Range(1.0, 4.0)) = 1.5
        _RampContrast("Ramp Contrast（拉開層次）", Range(0.5, 3.0)) = 1.0
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Front
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex   vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _HeatRamp;
            sampler3D _DensityTex;
            int       _StepCount;
            float     _Density;
            float     _AlphaThreshold;
            float4    _BoxMin, _BoxMax;

            float _RiseSpeed;
            float _TurbulenceScale;
            float _TurbulenceStrength;
            float _SwayAmount;
            float _HeightFalloff;

            float _RampBias;
            float _RampContrast;
            
            struct appdata { float4 vertex : POSITION; };
            struct v2f
            {
                float4 clipPos  : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.clipPos  = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            // ── AABB ────────────────────────────────────────────
            bool RayAABB(float3 ro, float3 rd,
            float3 bmin, float3 bmax,
            out float tNear, out float tFar)
            {
                float3 invDir = 1.0 / rd;
                float3 t0 = (bmin - ro) * invDir;
                float3 t1 = (bmax - ro) * invDir;
                float3 tMin = min(t0, t1);
                float3 tMax = max(t0, t1);
                tNear = max(max(tMin.x, tMin.y), tMin.z);
                tFar  = min(min(tMax.x, tMax.y), tMax.z);
                return tNear <= tFar && tFar > 0.0;
            }

            // ── Hash：產生假隨機數，Noise 的基礎 ────────────────
            float3 hash33(float3 p)
            {
                p = frac(p * float3(443.8975, 397.2973, 491.1871));
                p += dot(p, p.yxz + 19.19);
                return frac((p.xxy + p.yxx) * p.zyx);
            }

            // ── Value Noise（比 Perlin 更輕量）──────────────────
            float valueNoise(float3 p)
            {
                float3 i = floor(p);
                float3 f = frac(p);
                // Smoothstep
                float3 u = f * f * (3.0 - 2.0 * f);

                return lerp(
                lerp(
                lerp(hash33(i).x,               hash33(i + float3(1,0,0)).x, u.x),
                lerp(hash33(i + float3(0,1,0)).x, hash33(i + float3(1,1,0)).x, u.x),
                u.y),
                lerp(
                lerp(hash33(i + float3(0,0,1)).x, hash33(i + float3(1,0,1)).x, u.x),
                lerp(hash33(i + float3(0,1,1)).x, hash33(i + float3(1,1,1)).x, u.x),
                u.y),
                u.z);
            }

            // ── FBM（分形疊加，2 層即夠，保持效能）─────────────
            float fbm(float3 p)
            {
                float v = 0.0;
                v += 0.500 * valueNoise(p);
                v += 0.250 * valueNoise(p * 2.1 + float3(1.7, 9.2, 3.4));
                // 若效能允許可加第三層：
                // v += 0.125 * valueNoise(p * 4.3 + float3(8.3, 2.8, 5.1));
                return v;
            }

            // ── Curl Noise（無散度，模擬流體渦旋）───────────────
            // 透過對 FBM 取數值偏微分，得到 curl(F) = ∇×F
            float3 curlNoise(float3 p)
            {
                const float eps = 0.01;

                // 三個方向的 FBM 偏微分
                float dFx_dy = (fbm(p + float3(0, eps, 0)) - fbm(p - float3(0, eps, 0)));
                float dFx_dz = (fbm(p + float3(0, 0, eps)) - fbm(p - float3(0, 0, eps)));
                float dFy_dz = (fbm(p + float3(0, 0, eps) + float3(31.4, 0, 0))
                - fbm(p - float3(0, 0, eps) + float3(31.4, 0, 0)));
                float dFy_dx = (fbm(p + float3(eps, 0, 0) + float3(31.4, 0, 0))
                - fbm(p - float3(eps, 0, 0) + float3(31.4, 0, 0)));
                float dFz_dx = (fbm(p + float3(eps, 0, 0) + float3(0, 17.8, 0))
                - fbm(p - float3(eps, 0, 0) + float3(0, 17.8, 0)));
                float dFz_dy = (fbm(p + float3(0, eps, 0) + float3(0, 17.8, 0))
                - fbm(p - float3(0, eps, 0) + float3(0, 17.8, 0)));

                return float3(
                dFx_dy - dFx_dz,
                dFy_dz - dFy_dx,
                dFz_dx - dFz_dy
                ) / (2.0 * eps);
            }

            // ── Transfer Function ────────────────────────────────
            float4 TransferFunction(float density)
            {
                // 1. Contrast：拉開中低密度的層次
                float d = saturate(pow(density, 1.0 / _RampContrast));

                // 2. Bias：整體往高端偏移，讓中心更容易到紅色
                d = saturate(pow(d, 1.0 / _RampBias));

                float4 color = tex2D(_HeatRamp, float2(d, 0.5));

                // 3. Alpha 也用調整後的 d，避免高溫區域透明
                color.a = saturate(d * d);
                return color;
            }

            // ── Fragment ─────────────────────────────────────────
            float4 frag(v2f i) : SV_Target
            {
                float3 rayOrigin = _WorldSpaceCameraPos;
                float3 rayDir    = normalize(i.worldPos - _WorldSpaceCameraPos);

                float3 wMin = mul(unity_ObjectToWorld, float4(_BoxMin.xyz, 1)).xyz;
                float3 wMax = mul(unity_ObjectToWorld, float4(_BoxMax.xyz, 1)).xyz;

                float tNear, tFar;
                if (!RayAABB(rayOrigin, rayDir, wMin, wMax, tNear, tFar))
                discard;

                tNear = max(tNear, 0.0);

                float3 accColor  = 0;
                float  accAlpha  = 0;
                float  stepLen   = (tFar - tNear) / (float)_StepCount;

                // Jitter：消除步進環狀 artifact
                float jitter = frac(sin(dot(i.clipPos.xy,
                float2(12.9898, 78.233))) * 43758.5453);

                float t = tNear + jitter * stepLen;

                [loop]
                for (int s = 0; s < _StepCount; s++)
                {
                    float3 wp  = rayOrigin + rayDir * t;
                    // 正規化至 [0,1] UV
                    float3 uv  = (wp - wMin) / (wMax - wMin);
                    uv = saturate(uv);

                    // ── 熱氣動態偏移 ──────────────────────────
                    float  time    = _Time.y;

                    // 高度越高（uv.y 越大）飄動越明顯
                    float  heightFactor = pow(saturate(uv.y), _HeightFalloff);

                    // 1. 垂直上升：沿 Y 負向平移 UV（密度場往上跑）
                    float3 animUV = uv;
                    animUV.y -= time * _RiseSpeed;

                    // 2. Curl Noise 擾動：水平渦旋
                    float3 noisePos = animUV * _TurbulenceScale;
                    float3 curl     = curlNoise(noisePos);

                    // 水平渦旋 + 側向搖擺，Y 方向不加（避免破壞上升感）
                    float3 offset;
                    offset.x = curl.x * _TurbulenceStrength
                    + sin(time * 1.3 + uv.z * 4.0) * _SwayAmount;
                    offset.y = 0.0;   // Y 不加 curl，保持向上乾淨
                    offset.z = curl.z * _TurbulenceStrength
                    + cos(time * 0.9 + uv.x * 4.0) * _SwayAmount;

                    // 越高飄動越強
                    float3 sampleUV = saturate(uv + offset * heightFactor);

                    // ── 採樣密度場 ────────────────────────────
                    float density = tex3D(_DensityTex, sampleUV).r * _Density;

                    if (density > 0.001)
                    {
                        float4 sc = TransferFunction(density);
                        float  alpha = 1.0 - exp(-sc.a * stepLen * 10.0);
                        float  T     = 1.0 - accAlpha;

                        accColor += T * alpha * sc.rgb;
                        accAlpha += T * alpha;
                    }

                    t += stepLen;
                    if (accAlpha >= _AlphaThreshold) break;
                }

                return float4(accColor, saturate(accAlpha));
            }
            ENDCG
        }
    }
}