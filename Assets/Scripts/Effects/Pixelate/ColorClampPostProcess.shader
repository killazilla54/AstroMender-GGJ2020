Shader "Hidden/ColorClamp"
{
    HLSLINCLUDE

    #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

    TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
    TEXTURE2D_SAMPLER2D(_DitherTex, sampler_DitherTex);

    uniform float _Columns;
    uniform float _Rows;
    uniform float _ColorDepth;

    struct vertInput
    {
        float4 vertex : POSITION;
    };

    struct Varyings {
        float4 vertex : SV_POSITION;
        float2 uv : TEXCOORD0;
        float4 screenPosition : TEXCOORD1;
    };

    float4 frag (Varyings i) : SV_Target
    {
        float2 uv = i.uv;
        uv.x *= _Columns;
        uv.y *= _Rows;
        uv.x = round(uv.x);
        uv.y = round(uv.y);
        uv.x /= _Columns;
        uv.y /= _Rows;

        float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);

        col = floor(col * _ColorDepth) / _ColorDepth;

        return col;
    }

    ENDHLSL


    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            HLSLPROGRAM
            #pragma vertex VertDefault
            #pragma fragment frag
            ENDHLSL
        }
    }
}
