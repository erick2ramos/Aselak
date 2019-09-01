// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/VectorBorder" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Emission ("Emission", Color) = (0,0,0)
		_EmissionPower ("Emission Power", Range(0.0, 5.0)) = 0.1
		_EmissionLength ("Wireframe Length", Range(0.01, 0.1)) = 0.1
	}
	SubShader {
		Tags{
			//"Glowable" = "True"
			//"RenderType" = "Opaque"
			//"Queue" = "Transparent"
		}
		Pass{
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#include "UnityCG.cginc"
			#pragma vertex vert
			#pragma fragment frag
			// Activates the emission shader feature
			#pragma shader_feature _EMISSION_MAP

			sampler2D _MainTex;
			sampler2D _EmissionMap;
			float4 _Color;
			float4 _Emission;
			float _EmissionPower;
			float _EmissionLength;

			struct appdata {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			v2f vert(appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			// Returns the emission value given the texture or not
			float3 GetEmission(v2f i) {
				#if defined(FORWARD_BASE_PASS)
					#if defined(_EMISSION_MAP)
						return tex2D(_EmissionMap, i.uv.xy) * _Emission;
					#else
						return _Emission;
					#endif
				#else
					return  _Emission;
				#endif
			}

			float4 frag(v2f i) : SV_Target {
				float4 colorOut = float4(1,1,1,1);
				
				// Using default values from texture UV and the representation per face
				// with bk, r, g, y representation, replace the color from uv with _Color
				// given the amount of Red or Green used on that pixel everyother is colored
				// black
				if (i.uv.r < _EmissionLength) {
					colorOut *= _Color;
				} else
				if (i.uv.g < _EmissionLength) {
					colorOut *= _Color;
				} else
				if (i.uv.r > 1 - _EmissionLength) {
					colorOut *= _Color;
				} else
				if (i.uv.g > 1 - _EmissionLength) {
					colorOut *= _Color;
				}
				else {
					return fixed4(0, 0, 0, 1);
				}
					
				
				// As emission is additive, add emission value to the previous color
				// Only colored pixels emits color
				// Note: HDR camera needed as this may return colors out of 0,1 range
				return colorOut + float4(GetEmission(i), 1)	 * _EmissionPower;
			}

		ENDCG
		}
		
	}
	FallBack "Diffuse"
}
