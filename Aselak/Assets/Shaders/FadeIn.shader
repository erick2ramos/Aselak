// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/FadeIn"
{
	Properties
	{
		_Color ("Color", COLOR) = (0,0,0,1)
		_Cutoff ("Cutoff value", Range(0,1)) = 0.0
		_MainTex ("Texture", 2D) = "white" {}
		_FadePattern("Grayscale Pattern Texture", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _FadePattern;
			half4 _Color;
			fixed _Cutoff;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 alpha = tex2D(_FadePattern, i.uv);

				if (alpha.b < 1 - _Cutoff) {
					return col = lerp(col, _Color, 1);
				}
				return col;
			}
			ENDCG
		}
	}
}
