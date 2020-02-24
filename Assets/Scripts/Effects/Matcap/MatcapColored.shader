
Shader "Custom/MatCapColored
" {
	Properties{
		_Color("Main Color", Color) = (0.5,0.5,0.5,1)
		_MainTex("MatCap (RGB)", 2D) = "white" {}
	}

		Subshader{
		Tags{ "RenderType" = "Opaque" }

		Pass{
		Tags{ "LightMode" = "Always" }

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma fragmentoption ARB_precision_hint_fastest
#pragma multi_compile_fog
#include "UnityCG.cginc"

		struct v2f {
		float4 pos : SV_POSITION;
		float2	uv : TEXCOORD0;
		float3	TtoV0 : TEXCOORD1;
		float3	TtoV1 : TEXCOORD2;
	};


	v2f vert(appdata_base v)
	{
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);

		float3 normalX = unity_WorldToObject[0].xyz * v.normal.x;
		float3 normalY = unity_WorldToObject[1].xyz * v.normal.y;
		float3 normalZ = unity_WorldToObject[2].xyz * v.normal.z;

		float3 totalNormal = mul((float3x3)UNITY_MATRIX_V, normalize(normalX + normalY + normalZ));
		o.uv.xy = totalNormal.xy * 0.5 + 0.5;

		return o;
	}

	uniform float4 _Color;
	uniform sampler2D _MainTex;

	float4 frag(v2f i) : COLOR
	{
		float4 matcapTex = tex2D(_MainTex, i.uv);
		//matcapTex = _Color * matcapTex * 2.0;

		return matcapTex * _Color + 0.3;
	}
		ENDCG
	}
	}
}