﻿Shader "GGJ2020/SteppedExplosionDissolve_NonPriority
"
{
    Properties
    {
        _MainTex ("Exlosion Texture", 2D) = "white" {}
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _AnimStepCount("Animation Step Count", Int) = 3
        _DissolveReplace ("Dissolve Amount", Range(0, 1)) = 0
        [Toggle] _UseDissolveAmount("Use Dissolve Slider", float) = 0
        [Toggle] _ColorOverride("Override Texture output with Color", float) = 0
    }
    SubShader
    {
        Tags {
            "RenderType"="Opaque"
            }
        ZTest Always
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            struct v2f
            {
                float3 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _NoiseTex;
            float4 _NoiseTex_ST;
            float _DissolveReplace;
            float _UseDissolveAmount;
            float _ColorOverride;
            int _AnimStepCount;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv.xy = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv.z = v.uv.z;
                o.color = v.color;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv.xy);
                fixed4 noise = tex2D(_NoiseTex, i.uv.xy);
                fixed4 particleColor = 1- i.color;
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);

                col -= .01f;
                noise -= .01f;
                if(_UseDissolveAmount > 0)
                {
                    col -= round(_DissolveReplace * _AnimStepCount)/_AnimStepCount;
                }
                else
                {
                    col -= round(particleColor.a * _AnimStepCount)/_AnimStepCount;
                }
                clip(col * noise);
                
                if(_ColorOverride > 0)
                {
                    return i.color;
                }
                else
                {
                    return col;    
                }
                
            }
            ENDCG
        }
    }
}
