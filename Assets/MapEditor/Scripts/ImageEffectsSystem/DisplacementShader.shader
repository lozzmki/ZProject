// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/DisplacementShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_DisplacementTex("Displacement",2D) = "white"{}
		_magnitude("Magnitude",Range(0,0.1)) = 0
		_Speed("Speed",Range(0,1)) = 0
	}
	SubShader
	{

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

			sampler2D _MainTex;
			sampler2D _DisplacementTex;
			float _magnitude;
			float4 _MainTex_ST;
			float _Speed;
			v2f vert(appdata v) {
				v2f o;
				o.uv = v.uv;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			float4 frag(v2f i) :COLOR{
				float2 uv = i.uv + _Time * _Speed;
				float2 disp = tex2D(_DisplacementTex, uv).xy;
				disp = (disp * 2 - 1) * _magnitude;
				return tex2D(_MainTex, i.uv + disp);
			}
			ENDCG
		}
	}
}
