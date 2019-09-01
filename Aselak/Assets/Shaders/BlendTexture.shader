Shader "Hidden/BlendTexture"
{
	Properties
	{
		_MainTex("Texture", 2D) = "black" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always
		Tags { "Queue"="Transparent" }
		Pass
		{
			Blend OneMinusSrcAlpha One
			SetTexture[_MainTex] {combine texture}
		}
	}
}
