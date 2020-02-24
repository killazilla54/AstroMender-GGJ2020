Shader "Custom/BasicShield"
{
    Properties
    {
        _MainTex("Fresnel Dissolve Pattern", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _RimStregth("Rim Line Thickness", Range(0,1)) = 0.8
        _PatternStrength("Pattern Rim Strength", Range(0,1)) = 1
        [Toggle] _UsesTexture("Should Use Texture", Float) = 0

    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent"}
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows alpha

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex; 
        fixed4 _Color;
        float _RimStregth;
        float _PatternStrength;
        float _UsesTexture;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldNormal;
            float3 viewDir;
        };

        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float fresnel = dot(IN.worldNormal, IN.viewDir);
            fresnel = saturate(fresnel);

            fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
            // Albedo comes from a texture tinted by color
            fixed4 c = _Color;
            float fresnelOutput = 1 - fresnel;
            float patternFresnel = 1 - fresnel;

            if(fresnelOutput > _RimStregth)
            {
                fresnelOutput = 1;
            }
            else
            {
                fresnelOutput = 0;
            }

            tex -= _PatternStrength;
            clip(tex);
            float4 finalOutput = c.rgba * fresnelOutput;

            if(_UsesTexture > 0)
            {
                finalOutput *= tex;
            }

            o.Emission = finalOutput.rgb;
            o.Albedo =  finalOutput.rgb;
            // Metallic and smoothness come from slider variables
            o.Alpha = finalOutput.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
