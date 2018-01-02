// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/Outline_Base"
{
	Properties
	{
		_Color("Color",Color)=(0.0,1.0,0.0,1.0)
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct v2f
			{
				float4 pos : SV_POSITION;
			};

			
			v2f vert (appdata_base v)
			{
				v2f o;
				//o.pos=UnityObjectToClipPos(v.vertex);
				float4 vec = mul(unity_ObjectToWorld, v.vertex);
                o.pos = mul(UNITY_MATRIX_VP, vec);
				return o;
			}

			fixed4 _Color;
			
			fixed4 frag (v2f i) : SV_Target
			{
				return _Color;
				//return fixed4(1.0,0.0,0.0,1.0);
			}
			ENDCG
		}
	}
}
