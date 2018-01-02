Shader "Unlit/Outline_Main"
{
	Properties
	{
		_MainTex ("RT2", 2D) = "white" {}
		_SrcTexture("Src",2D)="white" {}
		_Strength("Strength",Float)=3.0
		_IterCount("IterCount",Int)=1
		_Color("Color",Color)=(0.0,1.0,0.0,1.0)
	}
	SubShader
	{
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
				float4 pos : SV_POSITION;
			};

			sampler2D _MainTex;
			float2 _MainTex_Size;
			float2 _MainTex_TexelSize;
			sampler2D _SrcTexture;
			Float _Strength;
			int _IterCount;
			float4 _MainTex_ST;
			fixed4 _Color;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv=v.uv;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				int iter=_IterCount*2+1;
				fixed4 ColorRadius=fixed4(0.0,0.0,0.0,0.0);
				float TX_x=_MainTex_TexelSize.x*_Strength;
				float TX_y=_MainTex_TexelSize.y*_Strength;
				float2 _uv = i.uv;
				_uv.y = 1 - _uv.y;

				for(int k=0;k<iter;++k)
					for(int j=0;j<iter;++j)
						ColorRadius += tex2D(_MainTex,i.uv+ float2( (k-iter/2.0)*TX_x , (j-iter/2.0)*TX_y ) );

				//如果模糊半径小于等于0,或者有颜色,画出原场景
				fixed4 emmm=tex2D(_MainTex,i.uv);

				//if (ColorRadius || (emmm.r) > 0 || (emmm.g) > 0 || (emmm.b) > 0)
				//	return tex2D(_SrcTexture, _uv);

				if((emmm.r+emmm.g+emmm.b)>0 || ColorRadius.r+ ColorRadius.g+ ColorRadius.b <= 0.0)
						return tex2D(_SrcTexture, _uv);

				return _Color.a*_Color + (1-_Color.a)*tex2D(_SrcTexture, _uv);
			}
			ENDCG
		}
	}
}
