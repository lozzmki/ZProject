Shader "MyShader/TransparentShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Diffuse("Color",Color) = (1,0,0,1)
	}
		SubShader
		{
			Tags { "RenderType" = "Transparent" "IgnoreProjector"="True" "Queue"="Transparent" }
			
			
		Pass
		{
			Tags{ "LightMode" = "ForwardBase" }

			// First pass renders only back faces 
			Cull Front

			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			
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
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			float4 _Diffuse;
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = _Diffuse;
				return col;
			}
			ENDCG
		}
			Pass
			{
				Tags{ "LightMode" = "ForwardBase" }

				// First pass renders only back faces 
				Cull Off

				ZWrite Off
				Blend SrcAlpha OneMinusSrcAlpha

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
				UNITY_FOG_COORDS(1)
					float4 vertex : SV_POSITION;
			};


			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			float4 _Diffuse;
			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = _Diffuse;
			return col;
			}
				ENDCG
			}
	}
}
