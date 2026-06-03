Shader "Custom/HeatmapVolume"
{
    Properties
    {
        _StepCount ("Ray Steps", Range(16, 128)) = 64
        _StepSize ("Step Size", Range(0.001, 0.1)) = 0.02
        _Density ("Density", Range(0.0, 5.0)) = 1.0
        _AlphaThreshold ("Alpha Threshold", Range(0.0, 1.0)) = 0.01

        // Temperature ramp: up to 8 color stops (temp 0..1 normalized)
        _TempColor0 ("Temp Color 0", Color) = (0,0,1,1)
        _TempColor1 ("Temp Color 1", Color) = (0,1,1,1)
        _TempColor2 ("Temp Color 2", Color) = (0,1,0,1)
        _TempColor3 ("Temp Color 3", Color) = (1,1,0,1)
        _TempColor4 ("Temp Color 4", Color) = (1,0.5,0,1)
        _TempColor5 ("Temp Color 5", Color) = (1,0,0,1)
        _TempColor6 ("Temp Color 6", Color) = (1,0,1,1)
        _TempColor7 ("Temp Color 7", Color) = (1,1,1,1)

        _TempStop0 ("Temp Stop 0", Range(0,1)) = 0.0
        _TempStop1 ("Temp Stop 1", Range(0,1)) = 0.15
        _TempStop2 ("Temp Stop 2", Range(0,1)) = 0.3
        _TempStop3 ("Temp Stop 3", Range(0,1)) = 0.45
        _TempStop4 ("Temp Stop 4", Range(0,1)) = 0.6
        _TempStop5 ("Temp Stop 5", Range(0,1)) = 0.75
        _TempStop6 ("Temp Stop 6", Range(0,1)) = 0.88
        _TempStop7 ("Temp Stop 7", Range(0,1)) = 1.0

        _ActiveStops ("Active Stop Count", Range(2, 8)) = 6

        _TempMin ("Min Temperature", Float) = 0.0
        _TempMax ("Max Temperature", Float) = 100.0
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Transparent"
            "Queue" = "Transparent+100"
            "RenderPipeline" = "UniversalPipeline"
            "IgnoreProjector" = "True"
        }

        Pass
        {
            Name "HeatmapVolumePass"
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Front

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            // ---- Properties ----
            CBUFFER_START(UnityPerMaterial)
                float  _StepCount;
                float  _StepSize;
                float  _Density;
                float  _AlphaThreshold;

                float4 _TempColor0, _TempColor1, _TempColor2, _TempColor3;
                float4 _TempColor4, _TempColor5, _TempColor6, _TempColor7;

                float  _TempStop0, _TempStop1, _TempStop2, _TempStop3;
                float  _TempStop4, _TempStop5, _TempStop6, _TempStop7;
                float  _ActiveStops;

                float  _TempMin;
                float  _TempMax;
            CBUFFER_END

            // ---- Heat Sources (up to 32 via constant buffer) ----
            #define MAX_HEAT_SOURCES 32
            CBUFFER_START(HeatSourceBuffer)
                float4 _HeatSourcePositions[MAX_HEAT_SOURCES]; // xyz=world pos, w=temperature
                float4 _HeatSourceParams[MAX_HEAT_SOURCES];    // x=radius, y=falloff, zw=unused
                int    _HeatSourceCount;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;
                float3 objectPos  : TEXCOORD1;
            };

            // ---- Helpers ----
            float3 WorldToLocal(float3 ws)
            {
                return mul(unity_WorldToObject, float4(ws, 1.0)).xyz;
            }

            // Returns true when point is inside unit cube [-0.5, 0.5]
            bool InsideBox(float3 p)
            {
                return all(abs(p) <= 0.5);
            }

            // Intersect ray with unit AABB [-0.5, 0.5]
            bool RayAABB(float3 ro, float3 rd, out float tmin, out float tmax)
            {
                float3 inv = 1.0 / rd;
                float3 t0  = (-0.5 - ro) * inv;
                float3 t1  = ( 0.5 - ro) * inv;
                float3 lo  = min(t0, t1);
                float3 hi  = max(t0, t1);
                tmin = max(max(lo.x, lo.y), lo.z);
                tmax = min(min(hi.x, hi.y), hi.z);
                return tmin < tmax && tmax > 0.0;
            }

            // Sample temperature at world-space point
            float SampleTemperature(float3 wsPos)
            {
                float totalTemp = 0.0;
                for (int i = 0; i < _HeatSourceCount && i < MAX_HEAT_SOURCES; i++)
                {
                    float3 srcPos = _HeatSourcePositions[i].xyz;
                    float  srcTemp = _HeatSourcePositions[i].w;
                    float  radius  = _HeatSourceParams[i].x;
                    float  falloff = max(_HeatSourceParams[i].y, 0.001);

                    float dist = length(wsPos - srcPos);
                    // Smooth inverse-square with clamp at radius
                    float influence = saturate(1.0 - dist / radius);
                    influence = pow(influence, falloff);
                    totalTemp += srcTemp * influence;
                }
                return totalTemp;
            }

            // Map raw temperature to [0,1]
            float NormalizeTemp(float t)
            {
                return saturate((t - _TempMin) / max(_TempMax - _TempMin, 0.001));
            }

            // Evaluate gradient with up to 8 stops
            float4 TempToColor(float nt)
            {
                float stops[8]  = { _TempStop0, _TempStop1, _TempStop2, _TempStop3,
                                    _TempStop4, _TempStop5, _TempStop6, _TempStop7 };
                float4 colors[8] = { _TempColor0, _TempColor1, _TempColor2, _TempColor3,
                                     _TempColor4, _TempColor5, _TempColor6, _TempColor7 };
                int n = (int)clamp(_ActiveStops, 2, 8);

                if (nt <= stops[0]) return colors[0];
                for (int i = 1; i < n; i++)
                {
                    if (nt <= stops[i])
                    {
                        float t = (nt - stops[i-1]) / max(stops[i] - stops[i-1], 0.0001);
                        return lerp(colors[i-1], colors[i], t);
                    }
                }
                return colors[n-1];
            }

            // ---- Vertex ----
            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.positionWS = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.objectPos  = IN.positionOS.xyz;
                return OUT;
            }

            // ---- Fragment ----
            float4 frag(Varyings IN) : SV_Target
            {
                float3 camWS  = _WorldSpaceCameraPos;
                float3 fragWS = IN.positionWS;

                // Ray in object/local space
                float3 roOS = WorldToLocal(camWS);
                float3 rdOS = normalize(WorldToLocal(fragWS) - roOS);

                float tmin, tmax;
                if (!RayAABB(roOS, rdOS, tmin, tmax)) discard;
                tmin = max(tmin, 0.0);

                float stepSize = (tmax - tmin) / _StepCount;
                float3 stepVec = rdOS * stepSize;
                float3 curOS   = roOS + rdOS * (tmin + stepSize * 0.5);

                float4 accum = float4(0, 0, 0, 0);

                for (int s = 0; s < (int)_StepCount; s++)
                {
                    if (!InsideBox(curOS)) { curOS += stepVec; continue; }

                    // World space position of this sample
                    float3 wsPos = mul(unity_ObjectToWorld, float4(curOS, 1.0)).xyz;

                    float rawTemp  = SampleTemperature(wsPos);
                    float normTemp = NormalizeTemp(rawTemp);

                    // Only render non-trivial temperatures
                    if (normTemp < 0.001) { curOS += stepVec; continue; }

                    float4 col = TempToColor(normTemp);
                    float  alpha = col.a * normTemp * _Density * stepSize;
                    alpha = saturate(alpha);

                    // Front-to-back compositing
                    accum.rgb += (1.0 - accum.a) * col.rgb * alpha;
                    accum.a   += (1.0 - accum.a) * alpha;

                    if (accum.a >= 0.99) break;
                    curOS += stepVec;
                }

                if (accum.a < _AlphaThreshold) discard;
                return accum;
            }
            ENDHLSL
        }
    }
    FallBack Off
}
