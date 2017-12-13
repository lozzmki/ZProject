// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'



Shader "ZShader/Edge"
{
	Properties{
		_Diffuse("Diffuse", Color) = (1,1,1,1)
		_OutlineCol("OutlineCol", Color) = (1,0,0,1)
		_OutlineFactor("OutlineFactor", Range(0,1)) = 0.1
		_MainTex("Base 2D", 2D) = "white"{}
		_RimPow("Rim pow",Range(0.1,8)) = 1
	}


		SubShader
		{


			Pass
		{

			Tags{ "RenderType" = "Transparent" }
			CGPROGRAM
			#include "UnityCG.cginc"  
#include "Lighting.cginc"
			#pragma vertex vert  
			#pragma fragment frag  
			fixed4 _OutlineCol;
			float _OutlineFactor;

			struct v2f
			{
				float4 pos : SV_POSITION;
				float3 worldNormal:TEXCOORD0;
				float3 worldViewDir:TEXCOORD1;
				float2 uv : TEXCOORD2;
			};

		v2f vert(appdata_full v)
		{
			v2f o;
			float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
			o.pos = UnityObjectToClipPos(v.vertex);
			o.worldViewDir = _WorldSpaceCameraPos.xyz - worldPos;
			o.worldNormal = mul(v.normal, (float3x3)unity_WorldToObject);
			o.uv = v.texcoord.xy;
			return o;
		}
		float _RimPow;
		float4 _Diffuse;
		sampler2D _MainTex;
		fixed4 frag(v2f i) : SV_Target
		{
			float3 wnormal = normalize(i.worldNormal);
			float3 viewDir = normalize(i.worldViewDir);
			float3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz);
			float rim = 1 - max(0, dot(viewDir, wnormal));
			float4 rimColor = _OutlineCol * pow(rim, _RimPow);
			//半兰伯特
			fixed4 halfLambert = 0.5*dot(wnormal, worldLightDir) + 0.5;
			fixed4 ambient = UNITY_LIGHTMODEL_AMBIENT * _Diffuse;
			fixed4 diffuse = _Diffuse * halfLambert * _LightColor0 + ambient;
			fixed4 color = tex2D(_MainTex, i.uv) * diffuse;

			return rimColor + color;
		}
			ENDCG
		}

		}
}
