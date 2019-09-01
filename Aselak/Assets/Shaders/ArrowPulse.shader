// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/ArrowPulse"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Main Color", COLOR) = (1,1,1,1)
		_Cutoff ("Pulse Border", Range(0.0, 4.0)) = 0.0
	}
	SubShader
	{
		// No culling or depth
		//Cull Off ZWrite Off ZTest Always
		Blend One One
		Tags{
			"LightMode"="Always"
			"RenderType"="Transparent"
			"Queue"="Transparent"
		}
		Pass
		{
			CGPROGRAM
// Upgrade NOTE: excluded shader from DX11; has structs without semantics (struct v2f members objectPos)
#pragma exclude_renderers d3d11
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0

			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float3 objectPos;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			fixed4 _Color;
			float4 _MainTex_ST;
			float _Cutoff = 0;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				o.objectPos = v.vertex.xyz;
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				_Cutoff = _Cutoff % 1.1;
				if (abs(_Cutoff - i.uv.x) <= 0.05) {
					if (col.b > 0.0) {
						col.b = 0.5;
						col.g = 0.5;
						col.r = 1;
					}
					_Cutoff += _Time.x;
					
				}
				return col * float4(1,1,1, 0.5) * _Color;
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
}
