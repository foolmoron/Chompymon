Shader "FX/Demon"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
        _Color1 ("Color1", Color) = (1, 1, 1, 1)
        _Color2 ("Color2", Color) = (0, 0, 0, 1)
		_FreqX ("FreqX", Range(0, 200)) = 100
		_FreqY ("FreqY", Range(0, 200)) = 100
		_ScrollX ("ScrollX", Range(-50, 50)) = 10
		_ScrollY ("ScrollY", Range(-50, 50)) = 0
		_Warp ("Warp", Range(-50, 50)) = 20
		_Lerp ("Lerp", Range(0, 1)) = 0.7

        _Rounding ("Rounding", Range(0, 1)) = 0.25
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
				float2 screen : TEXCOORD1;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
                o.screen = ComputeScreenPos(o.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
            fixed4 _Color1;
            fixed4 _Color2;
			fixed _FreqX;
			fixed _FreqY;
			fixed _ScrollX;
			fixed _ScrollY;
			fixed _Warp;
			fixed _Lerp;
			float _Rounding;

            float sin01(float f) {
                return (sin(f) + 1.)/2.;
            }

            float roundTo(float v, float r) {
                return floor(v / r) * r;
            }

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 finalColor = fixed4(0, 0, 0, 0);
                float l1 = sin01((i.screen.x + i.screen.y) * _FreqX + _Time.y*_ScrollX);
                float l2 = sin01((i.screen.x + -i.screen.y) * _FreqY + _Time.y*_ScrollY);
                l1 = sin01((i.screen.x + i.screen.y) * _FreqX + _Time.y*_ScrollX*sign(sin(l2*_Warp)));
				float l = lerp(l1, l2, _Lerp);
                finalColor = lerp(_Color1, _Color2, roundTo(l, _Rounding));
				return finalColor;
			}
			ENDCG
		}
	}
}
