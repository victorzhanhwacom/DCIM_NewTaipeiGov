Shader "UI/VerticalGradient"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _ColorTop ("Top Color", Color) = (0, 1, 1, 1)    // 頂部顏色
        _ScaleTop ("Scale Top", Range(0.01, 1)) = 0.5   // 頂部縮放
        _ColorBottom ("Bottom Color", Color) = (1, 0.5, 0, 1) // 底部顏色
        _ScaleBottom ("Scale Bottom", Range(0.01, 1)) = 0.5 // 底部縮放

        // UI Masking parameters
        [HideInInspector] _StencilComp ("Stencil Comparison", Float) = 8
        [HideInInspector] _Stencil ("Stencil ID", Float) = 0
        [HideInInspector] _StencilOp ("Stencil Operation", Float) = 0
        [HideInInspector] _StencilWriteMask ("Stencil Write Mask", Float) = 255
        [HideInInspector] _StencilReadMask ("Stencil Read Mask", Float) = 255
        [HideInInspector] _ColorMask ("Color Mask", Float) = 15
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off Lighting Off ZWrite Off ZTest [tl_ZTest] Blend SrcAlpha OneMinusSrcAlpha ColorMask [_ColorMask]

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _ColorTop, _ColorBottom;
            float _ScaleTop, _ScaleBottom;

            Varyings vert (Attributes input)
            {
                Varyings output;
                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                output.uv = input.uv;
                output.color = input.color;
                return output;
            }

            half4 frag (Varyings input) : SV_Target
            {
                // Calculate gradient factor based on UV.y
                // Using smoothstep for softer transition controlled by scale
                float t = input.uv.y;
                
                // Remap the gradient using Scale values
                // Scale smaller = transition happens faster near the edge
                float edge0 = _ScaleBottom * 0.5;
                float edge1 = 1.0 - (_ScaleTop * 0.5);
                float gradient = smoothstep(edge0, edge1, t);

                half4 finalColor = lerp(_ColorBottom, _ColorTop, gradient);
                half4 texColor = tex2D(_MainTex, input.uv);

                return finalColor * texColor * input.color;
            }
            ENDHLSL
        }
    }
}