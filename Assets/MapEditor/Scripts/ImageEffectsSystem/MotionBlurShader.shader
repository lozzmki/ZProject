// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/MotionBlurShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_BlurAmount("Blur Amount",Float) = 1.0
	}
		SubShader
		{
			Pass
			{
				Blend SrcAlpha OneMinusSrcAlpha
				ColorMask RGB

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment fragRGB

				#include "UnityCG.cginc"

				sampler2D _MainTex;
				fixed _BlurAmount;

				struct v2f {
					float4 pos : SV_POSITION;
					half2 uv : TEXCOORD0;
				};
				v2f vert(appdata_img v) {
					v2f o;
					o.uv = v.texcoord;
					o.pos = UnityObjectToClipPos(v.vertex);
					return o;
				}
				fixed4 fragRGB(v2f i) :SV_TARGET{
					return fixed4(tex2D(_MainTex,i.uv).rgb,_BlurAmount);
				}
				ENDCG
			}
			Pass
			{
					Blend One Zero
					ColorMask A

					CGPROGRAM
					#pragma vertex vert
					#pragma fragment fragA

					#include "UnityCG.cginc"

					sampler2D _MainTex;
					fixed _BlurAmount;

					struct v2f {
						float4 pos : SV_POSITION;
						half2 uv : TEXCOORD0;
					};
					v2f vert(appdata_img v) {
						v2f o;
						o.uv = v.texcoord;
						o.pos = UnityObjectToClipPos(v.vertex);
						return o;
					}
					fixed4 fragA(v2f i) :SV_TARGET{
						return tex2D(_MainTex,i.uv);
					}
					ENDCG
			}
	}
		Fallback off
}
