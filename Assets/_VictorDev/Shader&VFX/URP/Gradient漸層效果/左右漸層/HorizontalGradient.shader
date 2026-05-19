Shader "UI/HorizontalGradient"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _ColorLeft ("Left Color", Color) = (1, 1, 1, 1)    // 左側顏色
        _ScaleLeft ("Scale Left", Range(0.01, 1)) = 0.5    // 左側縮放
        _ColorRight ("Right Color", Color) = (0, 0.5, 1, 1) // 右側顏色
        _ScaleRight ("Scale Right", Range(0.01, 1)) = 0.5  // 右側縮放

        // UI Masking parameters
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
            float4 _ColorLeft, _ColorRight;
            float _ScaleLeft, _ScaleRight;

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
                // Gradient based on X axis
                float t = input.uv.x;
                
                // Remap using Scale logic
                float edge0 = _ScaleLeft * 0.5;
                float edge1 = 1.0 - (_ScaleRight * 0.5);
                float gradient = smoothstep(edge0, edge1, t);

                half4 finalColor = lerp(_ColorLeft, _ColorRight, gradient);
                half4 texColor = tex2D(_MainTex, input.uv);

                // Multiply by vertex color to support UI Image component's Alpha/Color
                return finalColor * texColor * input.color;
            }
            ENDHLSL
        }
    }
}