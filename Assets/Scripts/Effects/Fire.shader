Shader "VFX/Fire"
{
    Properties
    {
        [HDR] _OuterFire ("Outer Fire Color", Color) = (1,1,1,1)
        [HDR] _InnerFire ("Inner Fire Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseTex ("Noise", 2D) = "white" {}
        _NoiseSpeed1 ("Noise Speed R", float) = 2.0
        _NoiseSpeed2 ("Noise Speed G", float) = 0.5

    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha
        Cull OFF
        LOD 100

        Pass
        {
            CGPROGRAM
            // Upgrade NOTE: excluded shader from DX11; has structs without semantics (struct v2f members objectRot)
            #pragma exclude_renderers d3d11
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 worldNormal : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _NoiseTex;
            float4 _NoiseTex_ST;

            fixed4 _InnerFire;
            fixed4 _OuterFire;

            float _NoiseSpeed1;
            float _NoiseSpeed2;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldNormal = normalize( mul(float4(v.normal, 0.0), unity_ObjectToWorld).xyz);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                // Find our UVs for each axis based on world position of the fragment.
                half2 yUV = i.worldNormal.xz;
                half2 xUV = i.worldNormal.zy;
                half2 zUV = i.worldNormal.xy;
                // Now do texture samples from our diffuse map with each of the 3 UV set's we've just made.
                half3 yDiff = tex2D (_NoiseTex, yUV);
                half3 xDiff = tex2D (_NoiseTex, float2(xUV.x, xUV.y - _Time.x * _NoiseSpeed1));
                half3 zDiff = tex2D (_NoiseTex, float2(zUV.x, zUV.y - _Time.x * _NoiseSpeed1));
                // Get the absolute value of the world normal.
                // Put the blend weights to the power of BlendSharpness, the higher the value,
                // the sharper the transition between the planar maps will be.
                half3 blendWeights = pow (abs(i.worldNormal), 1.0);
                // Divide our blend mask by the sum of it's components, this will make x+y+z=1
                blendWeights = blendWeights / (blendWeights.x + blendWeights.y + blendWeights.z);
                // Finally, blend together all three samples based on the blend mask.
                fixed3 tricolor = xDiff * blendWeights.x /*+ yDiff * blendWeights.y */ +zDiff * blendWeights.z;

                // Noise 2
                half3 yDiff2 = tex2D (_NoiseTex, yUV);
                half3 xDiff2 = tex2D (_NoiseTex, float2(xUV.x, xUV.y - _Time.x * _NoiseSpeed2));
                half3 zDiff2 = tex2D (_NoiseTex, float2(zUV.x, zUV.y - _Time.x * _NoiseSpeed2));
                // Get the absolute value of the world normal.
                // Put the blend weights to the power of BlendSharpness, the higher the value,
                // the sharper the transition between the planar maps will be.
                half3 blendWeights2 = pow (abs(i.worldNormal), 1.0);
                // Divide our blend mask by the sum of it's components, this will make x+y+z=1
                blendWeights2 = blendWeights2 / (blendWeights2.x + blendWeights2.y + blendWeights2.z);
                // Finally, blend together all three samples based on the blend mask.
                fixed3 tricolor2 = xDiff2 * blendWeights2.x /*+ yDiff2 * blendWeights2.y */ +zDiff2 * blendWeights2.z;

                // // return (_InnerFire * tricolor3.r + _OuterFire * tricolor3.g);
                fixed noise1 = tex2D(_NoiseTex, float2(uv.x, uv.y + _Time.x * _NoiseSpeed1)).r;
                fixed noise2 = tex2D(_NoiseTex, float2(uv.x, uv.y + _Time.x * _NoiseSpeed2)).g;

                // Noise still scrolls
                float noise = tricolor + tricolor2;
                float distort = noise * uv.y;

                fixed4 col = tex2D(_MainTex, float2(uv.x, uv.y + distort));
                col = (_InnerFire * col.r + _OuterFire * col.g);
                clip(col.b);
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
