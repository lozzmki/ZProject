Shader "Custom/nums" {
	Properties {
		_Alpha("Alpha", Range(0,1)) = 1.0
		_MainTex ("Texture", 2D) = "white" {}
		_Position("Position", Vector) = (0.0,0.0,0.0,0.0)
	}
	SubShader{
		LOD 250
		Pass{
			ZTest Always
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			sampler2D _MainTex;
			float4 _Position;
			fixed _Alpha;

			struct vf {
				float4 pos:POSITION;
				float4 uv:TEXCOORD0;
			};
			struct vfo {
				float4 pos:SV_POSITION;
				float4 uv:TEXCOORD0;
			};

			vfo vert(vf v)
			{
				vf o=v;
				o.pos.xy = _Position.xy / _ScreenParams.xy + v.pos.xy;
				o.pos.xy = o.pos.xy*2.0 - float2(1.0, 1.0);
				o.pos.z = 0.0;
				o.pos.w = 1.0;
				return o;
			}

			fixed4 frag(vfo i) :SV_TARGET{
				fixed4 _col = tex2D(_MainTex, i.uv);
				if (_col.a < 0.6)
					_col.a = 0;
				_col.a = _col.a*_Alpha;
				return _col;
			}

			ENDCG
				
		}

	}
	FallBack "Diffuse"
}
