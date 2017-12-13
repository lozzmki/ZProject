Shader "Unlit/OutlineShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_OutlineColor ("Outline Color",Color) = (1,0,1,1)
		_OutlineFactor("Outline Factor",Range(0,3)) = 0.01
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
				ENDCG
			}
			Pass{
				Cull Front

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				fixed4 _OutlineColor;
				float _OutlineFactor;
				#include "UnityCG.cginc"

			float4 vert(appdata_base v):SV_POSITION
			{
				
				float4 pos = UnityObjectToClipPos(v.vertex);
				float3 normal = mul((float3x3)UNITY_MATRIX_MVP, v.normal);
				
				pos.xy += normal.xy * _OutlineFactor;
				return pos;

			}

			fixed4 frag() : SV_Target
			{
				
				return _OutlineColor;
			}

				ENDCG
		}
	}
}
