// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/AselakEffect"
{
	Properties
	{
		_Color ("Color", COLOR) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
		_Emission("Emission", Color) = (0,0,0)
		_EmissionPower("Emission Power", Range(0.0, 5.0)) = 0.1
		_EmissionLength("Wireframe Length", Range(0.0, 0.1)) = 0.1
		_Alpha("Transparency", Range(0.0, 1.0)) = 1
	}
	SubShader
	{
		// No culling or depth
		// Cull On ZWrite Off ZTest Always
		//Blend One SrcAlpha//SrcAlpha OneMinusSrcAlpha
		Blend SrcAlpha OneMinusSrcAlpha
		Tags{
			"RenderType"="Opaque"
			"Queue"="Transparent"
		}
		Pass
		{
			CGPROGRAM
			#include "UnityCG.cginc"
			#pragma vertex vert
			#pragma fragment frag
			#pragma shader_feature _EMISSION_MAP


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
			fixed4 _Color;
			float3 _Emission;
			float _EmissionLength;
			float _EmissionPower;
			fixed _Alpha;

			half4 frag (v2f i) : SV_Target
			{
				fixed4 col = fixed4(1, 1, 1, 1);
				
				if (i.uv.r < _EmissionLength) {
					col *= _Color;
				} else
				if (i.uv.g < _EmissionLength) {
					col *= _Color;
				} else
				if (i.uv.r > 1 - _EmissionLength) {
					col *= _Color;
				} else
				if (i.uv.g > 1 - _EmissionLength) {
					col *= _Color;
				} else
				if (i.uv.r - i.uv.g < _EmissionLength){
					col *= _Color;
				} else {
					col *= float4(0, 0, 0, 1);
					return col;
				}

				return (col + ( abs(cos(_Time.x * 30)) * float4(_Emission, 1) * _EmissionPower)) * (1,1,1, _Alpha);
			}
			ENDCG
		}
	}
}
