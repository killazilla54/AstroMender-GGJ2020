
Shader "Custom/MatCapDithered" {
	Properties{
		_Color("Main Color", Color) = (0.5,0.5,0.5,1)
		_MainTex("MatCap (RGB)", 2D) = "white" {}
		_DitherTex("Dither (BW)", 2D) = "white" {}
		_DitherScale("Dither Scale", Range(0.1, 10)) = 1
	}

		Subshader{
		Tags{ "RenderType" = "Opaque" }

		Pass{
		Tags{ "LightMode" = "Always" }

		CGPROGRAM
	#pragma vertex vert
	#pragma fragment frag
	#include "UnityCG.cginc"

		struct appdata{
			float4 vertex : POSITION;
			float2 uv : TEXCOORD0;
			float3 normal : TEXCOORD3;
		};

		struct v2f
		{
			float4 pos : SV_POSITION;
			float2	uv : TEXCOORD0;
			float3	TtoV0 : TEXCOORD1;
			float3	TtoV1 : TEXCOORD2;
			float4 screenPosition : TEXCOORD4;
			float2	uvBase : TEXCOORD5;
		};

	uniform float4 _Color;
	uniform sampler2D _MainTex;
	float4 _MainTex_ST;
	uniform sampler2D _DitherTex;
	float4 _DitherTex_TexelSize;
	uniform float _DitherScale;

	v2f vert(appdata_base v)
	{
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);

		float3 normalX = unity_WorldToObject[0].xyz * v.normal.x;
		float3 normalY = unity_WorldToObject[1].xyz * v.normal.y;
		float3 normalZ = unity_WorldToObject[2].xyz * v.normal.z;

		float3 totalNormal = mul((float3x3)UNITY_MATRIX_V, normalize(normalX + normalY + normalZ));
		o.uv.xy = totalNormal.xy * 0.5 + 0.5;
		o.uvBase = TRANSFORM_TEX(v.texcoord, _MainTex);
		o.screenPosition = ComputeScreenPos(o.pos);

		return o;
	}



	float4 frag(v2f i) : COLOR
	{
		float4 matcapTex = tex2D(_MainTex, i.uv);

		float texColor = tex2D(_MainTex, i.uvBase).r;
		float2 screenPos = i.screenPosition.xy / i.screenPosition.w;
		float2 ditherCoordinate = screenPos * _ScreenParams.xy * _DitherTex_TexelSize.xy;
		float ditherValue = tex2D(_DitherTex, ditherCoordinate).r;
		//matcapTex = _Color * matcapTex * 2.0;

		float ditheredValue = step(ditherValue, matcapTex);
		float4 col = lerp(float4(0,0,0,1), float4(1,1,1,1), ditheredValue);
		return col;
		//return matcapTex;
	}
		ENDCG
	}
	}
}