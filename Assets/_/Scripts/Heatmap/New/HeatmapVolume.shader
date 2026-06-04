Shader "Custom/HeatmapVolume"
{
    Properties
    {
        _StepCount     ("Ray Steps",       Range(16, 128)) = 64
        _StepSize      ("Step Size",       Range(0.001, 0.1)) = 0.02
        _Density       ("Density",         Range(0.0, 5.0)) = 1.0
        _AlphaThreshold("Alpha Threshold", Range(0.0, 1.0)) = 0.01

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

        [Header(Edge Flow)]
        _FlowTime      ("Flow Time (set by C#)", Float) = 0.0
        _FlowSpeed     ("Flow Speed",      Range(0.0, 5.0)) = 0.6
        _FlowStrength  ("Flow Strength",   Range(0.0, 2.0)) = 0.35
        _FlowScale     ("Flow Scale",      Range(0.1, 8.0)) = 2.0
        _FlowOctaves   ("Flow Octaves",    Range(1,   4  )) = 3
        _EdgeBand      ("Edge Band Width", Range(0.0, 1.0)) = 0.4
        _FlowDirection ("Flow Direction",  Vector) = (0.2, 1.0, 0.1, 0.0)
    }

    SubShader
    {
        Tags
        {
            "RenderType"     = "Transparent"
            "Queue"          = "Transparent+100"
            "RenderPipeline" = "UniversalPipeline"
            "IgnoreProjector"= "True"
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

            // ----------------------------------------------------------------
            // Constant Buffers
            // ----------------------------------------------------------------
            // IMPORTANT: std140 alignment rules for WebGL —
            //   every float4 must start on a 16-byte boundary.
            //   We group lone floats into float4 packs to guarantee this.
            CBUFFER_START(UnityPerMaterial)
                // pack 1 — raymarching scalars
                float4 _RaymarchPack;   // x=StepCount  y=Density  z=AlphaThreshold  w=StepSize

                // pack 2 — temperature range
                float4 _TempRangePack;  // x=TempMin  y=TempMax  z=ActiveStops  w=unused

                // pack 3 — flow scalars
                float4 _FlowPack;       // x=FlowTime  y=FlowSpeed  z=FlowStrength  w=FlowScale

                // pack 4 — flow octaves / edge
                float4 _FlowPack2;      // x=FlowOctaves  y=EdgeBand  zw=unused

                // pack 5 — flow direction (already float4, naturally aligned)
                float4 _FlowDirection;

                // colour stops (float4 → always aligned)
                float4 _TempColor0, _TempColor1, _TempColor2, _TempColor3;
                float4 _TempColor4, _TempColor5, _TempColor6, _TempColor7;

                // stop positions packed 2-per-float4 to keep alignment
                float4 _TempStopPack0;  // x=Stop0  y=Stop1  z=Stop2  w=Stop3
                float4 _TempStopPack1;  // x=Stop4  y=Stop5  z=Stop6  w=Stop7

                // base temperature pack
                // x=BaseTemp  y=BaseDensityScale  zw=unused
                float4 _BaseTempPack;
            CBUFFER_END

            // Accessors — keeps rest of code readable
            #define _StepCount       _RaymarchPack.x
            #define _Density         _RaymarchPack.y
            #define _AlphaThreshold  _RaymarchPack.z
            #define _StepSizeUnused  _RaymarchPack.w

            #define _TempMin         _TempRangePack.x
            #define _TempMax         _TempRangePack.y
            #define _ActiveStops     _TempRangePack.z

            #define _FlowTime        _FlowPack.x
            #define _FlowSpeed       _FlowPack.y
            #define _FlowStrength    _FlowPack.z
            #define _FlowScale       _FlowPack.w

            #define _FlowOctaves     _FlowPack2.x
            #define _EdgeBand        _FlowPack2.y

            #define _BaseTemp        _BaseTempPack.x
            #define _BaseDensityScale _BaseTempPack.y

            // ----------------------------------------------------------------
            // Heat Sources
            // ----------------------------------------------------------------
            #define MAX_HEAT_SOURCES 32
            CBUFFER_START(HeatSourceBuffer)
                float4 _HeatSourcePositions[MAX_HEAT_SOURCES]; // xyz=pos, w=temp
                float4 _HeatSourceParams[MAX_HEAT_SOURCES];    // x=radius, y=falloff
                float4 _HeatSourceCountPack;                   // x=count (float for alignment)
            CBUFFER_END
            #define _HeatSourceCount (int)_HeatSourceCountPack.x

            // ----------------------------------------------------------------
            // Structs
            // ----------------------------------------------------------------
            struct Attributes { float4 positionOS : POSITION; };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;
            };

            // ----------------------------------------------------------------
            // Noise (pure math, no texture, WebGL safe)
            // ----------------------------------------------------------------
            float3 hash33(float3 p)
            {
                p = float3(dot(p, float3(127.1, 311.7,  74.7)),
                           dot(p, float3(269.5, 183.3, 246.1)),
                           dot(p, float3(113.5, 271.9, 124.6)));
                return frac(sin(p) * 43758.5453123);
            }

            float vnoise(float3 p)
            {
                float3 i = floor(p);
                float3 f = frac(p);
                float3 u = f * f * (3.0 - 2.0 * f);
                float a = dot(hash33(i               ), float3(1,0,0));
                float b = dot(hash33(i + float3(1,0,0)), float3(1,0,0));
                float c = dot(hash33(i + float3(0,1,0)), float3(1,0,0));
                float d = dot(hash33(i + float3(1,1,0)), float3(1,0,0));
                float e = dot(hash33(i + float3(0,0,1)), float3(1,0,0));
                float ff= dot(hash33(i + float3(1,0,1)), float3(1,0,0));
                float g = dot(hash33(i + float3(0,1,1)), float3(1,0,0));
                float h = dot(hash33(i + float3(1,1,1)), float3(1,0,0));
                return lerp(lerp(lerp(a,b,u.x),lerp(c,d,u.x),u.y),
                            lerp(lerp(e,ff,u.x),lerp(g,h,u.x),u.y),u.z);
            }

            float fbm(float3 p, int oct)
            {
                float v = 0.0, amp = 0.5, freq = 1.0;
                // Fully unrolled — dynamic loops over int uniforms are
                // unreliable on some WebGL drivers
                if (oct >= 1) { v += amp * vnoise(p * freq); amp *= 0.5; freq *= 2.1; }
                if (oct >= 2) { v += amp * vnoise(p * freq); amp *= 0.5; freq *= 2.1; }
                if (oct >= 3) { v += amp * vnoise(p * freq); amp *= 0.5; freq *= 2.1; }
                if (oct >= 4) { v += amp * vnoise(p * freq); }
                return v;
            }

            float3 FlowDisplacement(float3 wsPos, float edgeMask)
            {
                // _FlowTime comes from C# (Time.time), guaranteed to advance
                float animT = _FlowTime * _FlowSpeed;
                float sc    = _FlowScale;
                float3 dir  = normalize(_FlowDirection.xyz + float3(0.001,0.001,0.001));
                float3 drift = dir * animT * 0.15;
                float3 ep    = wsPos * sc;
                int oct      = max(1, min((int)_FlowOctaves, 4));

                float nx = fbm(ep + drift + float3(0.0, 1.7, 9.2), oct) * 2.0 - 1.0;
                float ny = fbm(ep + drift + float3(8.3, 2.8, 1.0), oct) * 2.0 - 1.0;
                float nz = fbm(ep + drift + float3(3.1, 6.7, 4.4), oct) * 2.0 - 1.0;

                // upward buoyancy
                ny += 0.4;

                return float3(nx, ny, nz) * _FlowStrength * edgeMask;
            }

            // ----------------------------------------------------------------
            // Heat helpers
            // ----------------------------------------------------------------
            float3 WorldToLocal(float3 ws)
            {
                return mul(unity_WorldToObject, float4(ws, 1.0)).xyz;
            }

            bool InsideBox(float3 p) { return all(abs(p) <= 0.5); }

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

            float SourceInfluence(int i, float3 wsPos)
            {
                float3 srcPos = _HeatSourcePositions[i].xyz;
                float  radius = _HeatSourceParams[i].x;
                float  fallof = max(_HeatSourceParams[i].y, 0.001);
                float  dist   = length(wsPos - srcPos);
                return pow(saturate(1.0 - dist / radius), fallof);
            }

            void SampleField(float3 wsPos, out float totalTemp, out float maxInf)
            {
                totalTemp = 0.0; maxInf = 0.0;
                for (int i = 0; i < _HeatSourceCount && i < MAX_HEAT_SOURCES; i++)
                {
                    float inf = SourceInfluence(i, wsPos);
                    totalTemp   += _HeatSourcePositions[i].w * inf;
                    maxInf       = max(maxInf, inf);
                }
                // Blend: areas with no heat source influence fall back to BaseTemp.
                // maxInf==0 → pure base; maxInf==1 → pure heat-source temperature.
                totalTemp = lerp(_BaseTemp, totalTemp, saturate(maxInf));
            }

            float NormalizeTemp(float t)
            {
                return saturate((t - _TempMin) / max(_TempMax - _TempMin, 0.001));
            }

            float4 TempToColor(float nt)
            {
                float  stops[8]  = { _TempStopPack0.x, _TempStopPack0.y,
                                     _TempStopPack0.z, _TempStopPack0.w,
                                     _TempStopPack1.x, _TempStopPack1.y,
                                     _TempStopPack1.z, _TempStopPack1.w };
                float4 colors[8] = { _TempColor0, _TempColor1, _TempColor2, _TempColor3,
                                     _TempColor4, _TempColor5, _TempColor6, _TempColor7 };
                int n = max(2, min((int)_ActiveStops, 8));
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

            // ----------------------------------------------------------------
            // Vertex
            // ----------------------------------------------------------------
            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.positionWS = TransformObjectToWorld(IN.positionOS.xyz);
                return OUT;
            }

            // ----------------------------------------------------------------
            // Fragment
            // ----------------------------------------------------------------
            float4 frag(Varyings IN) : SV_Target
            {
                float3 camWS  = _WorldSpaceCameraPos;
                float3 fragWS = IN.positionWS;

                float3 roOS = WorldToLocal(camWS);
                float3 rdOS = normalize(WorldToLocal(fragWS) - roOS);

                float tmin, tmax;
                if (!RayAABB(roOS, rdOS, tmin, tmax)) discard;
                tmin = max(tmin, 0.0);

                float stepSz = (tmax - tmin) / _StepCount;
                float3 stepV = rdOS * stepSz;
                float3 curOS = roOS + rdOS * (tmin + stepSz * 0.5);

                // Convert one object-space step to world-space metres.
                // unity_ObjectToWorld columns encode scale, so the world-space
                // length of a unit OS vector is length(col0/col1/col2).
                // We use the ray direction (already a unit OS vector scaled by stepSz)
                // and measure its world-space length to get the true step distance.
                float3 stepWS     = mul((float3x3)unity_ObjectToWorld, rdOS * stepSz);
                float  stepSzWS   = length(stepWS);   // world-space metres per step

                float4 accum = float4(0,0,0,0);

                for (int s = 0; s < (int)_StepCount; s++)
                {
                    if (!InsideBox(curOS)) { curOS += stepV; continue; }

                    float3 wsPos = mul(unity_ObjectToWorld, float4(curOS, 1.0)).xyz;

                    float rawTemp, maxInf;
                    SampleField(wsPos, rawTemp, maxInf);

                    float normTemp = NormalizeTemp(rawTemp);

                    // Edge mask: 1 at boundary, 0 at core
                    float eb       = max(_EdgeBand, 0.01);
                    float edgeMask = (1.0 - smoothstep(0.0, eb, maxInf))
                                   * smoothstep(0.0, 0.05, normTemp);

                    // Flow displacement + re-sample
                    float3 displaced = wsPos + FlowDisplacement(wsPos, edgeMask);
                    float dispTemp, dispInf;
                    SampleField(displaced, dispTemp, dispInf);
                    float dispNorm = NormalizeTemp(dispTemp);

                    float blended = lerp(normTemp, dispNorm, edgeMask);

                    float4 col   = TempToColor(blended);
                    // Beer-Lambert volumetric integral:
                    //   alpha = 1 - exp(-sigma * stepSzWS)
                    // sigma (extinction) is scaled by maxInf so the medium is
                    // denser near heat sources.  _Density is the artist knob.
                    // Using world-space step length keeps opacity consistent
                    // regardless of how large the Volume object is scaled.
                    float sigmaBase = _BaseDensityScale * _Density;
                    float sigmaSrc  = maxInf * _Density;
                    float sigma     = lerp(sigmaBase, sigmaSrc, saturate(maxInf));
                    float alpha     = 1.0 - exp(-sigma * stepSzWS);

                    accum.rgb += (1.0 - accum.a) * col.rgb * alpha;
                    accum.a   += (1.0 - accum.a) * alpha;

                    if (accum.a >= 0.99) break;
                    curOS += stepV;
                }

                if (accum.a < _AlphaThreshold) discard;
                return accum;
            }
            ENDHLSL
        }
    }
    FallBack Off
}
