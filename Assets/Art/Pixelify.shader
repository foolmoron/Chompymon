Shader "FX/Pixelify"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_PixelDensityX ("PixelDensityX", Range(0.1, 100)) = 100
		_PixelDensityY ("PixelDensityY", Range(0.1, 100)) = 100
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
            fixed _PixelDensityX;
            fixed _PixelDensityY;

			fixed4 frag (v2f i) : SV_Target
			{
                fixed2 pixels = (_ScreenParams.xy * fixed2(_PixelDensityX, _PixelDensityY)) / 100.;
                fixed2 uv = floor(i.uv * pixels) / pixels;
				fixed4 finalColor = tex2D(_MainTex, uv);
				return finalColor;
			}
			ENDCG
		}
	}
}
