Shader "UI/FourCornerGradient"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _ColorTopLeft ("Top Left", Color) = (1,1,1,1)
        _ScaleLeft ("Scale Left", Range(0, 1)) = 0.5
        _ColorTopRight ("Top Right", Color) = (0,1,1,1)
        _ScaleRight ("Scale Right", Range(0, 1)) = 0.5
        _ColorBottomLeft ("Bottom Left", Color) = (1,0.5,0,1)
        _ScaleBottomLeft ("Scale Bottom Left", Range(0, 1)) = 0.5
        _ColorBottomRight ("Bottom Right", Color) = (1,1,1,1)
        _ScaleBottomRight ("Scale Bottom Right", Range(0, 1)) = 0.5

        // Required for UI Masking
        [HideInInspector] _StencilComp ("Stencil Comparison", Float) = 8
        [HideInInspector] _Stencil ("Stencil ID", Float) = 0
        [HideInInspector] _StencilOp ("Stencil Operation", Float) = 0
        [HideInInspector] _StencilWriteMask ("Stencil Write Mask", Float) = 255
        [HideInInspector] _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15
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
            float4 _ColorTopLeft, _ColorTopRight, _ColorBottomLeft, _ColorBottomRight;
            float _ScaleLeft, _ScaleRight, _ScaleBottomLeft, _ScaleBottomRight;

            // Remap function to handle Scale/Spread
            float remap(float t, float scale) {
                return smoothstep(0.0, 1.0, t / max(scale, 0.001));
            }

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
                float2 uv = input.uv;

                // Bilinear interpolation logic
                // Bottom row interpolation
                half4 colorBottom = lerp(_ColorBottomLeft, _ColorBottomRight, remap(uv.x, _ScaleBottomRight));
                // Top row interpolation
                half4 colorTop = lerp(_ColorTopLeft, _ColorTopRight, remap(uv.x, _ScaleRight));
                // Vertical interpolation between top and bottom
                half4 finalColor = lerp(colorBottom, colorTop, uv.y);

                half4 texColor = tex2D(_MainTex, uv);
                return finalColor * texColor * input.color;
            }
            ENDHLSL
        }
    }
}