Shader "Custom/bars" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_SubColor("SubColor", Color) = (1,1,1,1)
		_Percent("Percent", Range(0.0,1.0)) = 1.0
		_SubPercent("SubPercent", Range(0.0,1.0)) = 1.0
		_Alpha("Alpha", Range(0.0,1.0)) = 1.0
		_FrameAlpha("FrameAlpha", Range(0.0,1.0)) = 1.0
		_MainTex("FrameTexture", 2D) = "white" {}
		[Toggle] _IsVertical("IsVertical", Float) = 1.0
	}
	SubShader{

		Pass{
			LOD 200

			ZTest Always
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			fixed4 _Color;
			fixed4 _SubColor;
			float _Percent;
			float _SubPercent;
			float _Alpha;
			float _IsVertical;
			float _FrameAlpha;
			sampler2D _MainTex;

			struct vin {
				float4 v:POSITION;
				float2 uv:TEXCOORD0;
			};
			struct vout {
				float4 v:SV_POSITION;
				float2 uv:TEXCOORD0;
			};

			vout vert(vin v)
			{
				vout o;
				o.v = UnityObjectToClipPos(v.v);
				o.uv = v.uv;
				return o;
			}

			fixed4 frag(vout v) : SV_Target
			{
				fixed4 _col;
				float _p;
				if (_IsVertical > 0.0)
					_p = v.uv.y;
				else
					_p = v.uv.x;

				if (_p < _Percent)
					_col = _Color;
				else if (_p < _SubPercent)
					_col = _SubColor;
				else
					_col = fixed4(0, 0, 0, 0);

				fixed4 _tex = tex2D(_MainTex, v.uv);
				float _fa = _tex.a*_FrameAlpha;
				float a = _fa + _col.a;
				if (a > 1.0)
					a = 1.0;

				fixed4 _ret = fixed4(_tex.rgb*_fa + _col.rgb*(a - _fa), a*_Alpha);
				return _ret;
			}

			ENDCG
		}
	}
	FallBack "Diffuse"
}
