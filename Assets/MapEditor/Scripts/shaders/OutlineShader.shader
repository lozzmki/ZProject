// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "_Shader/Edge" 
{
    Properties
    {
        _MainTex("main tex", 2D) = ""{ }
        _OutLineColor("outline color",Color) = (0,0,0,1)

		 _mColor("normal color",Color) = (0,0,0,1) 
		 _mAlpha("normal alpha",range(0,1))=0.5
    }
    SubShader
    {
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        pass
        {
            Cull Front
            Offset -5,-1 
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            sampler2D _MainTex;
            half4 _OutLineColor;
            struct v2f
            {
                float4  pos : SV_POSITION;
            };
            v2f vert(appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }
 
            float4 frag(v2f i) : COLOR
            {
                return _OutLineColor;
            }
            ENDCG
        } 
        
        pass
        {
			Tags{"LightMode" = "ForwardBase"}
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
 
            sampler2D _MainTex;
            float4 _MainTex_ST;
			half4 _mColor;
			half4 _OutLineColor;
			float _mAlpha;
            struct v2f
            {
                float4  pos : SV_POSITION;
                float2  uv : TEXCOORD0;
            };
            v2f vert(appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord,_MainTex);
                return o;
            }
            float4 frag(v2f i) : COLOR
            {
				float4 rlsColor = float4(_mColor.xyz,_mAlpha);
                return rlsColor;
            }
            ENDCG
        }
    }
	Fallback "Transparent/VertexLit"
}