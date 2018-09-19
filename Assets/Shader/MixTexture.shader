Shader "Hidden/MixTexture"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_BlurTex("Blur Texture",2D) = "while"{}
		_BlurFactor("Blur Factor",Range(0,1)) = 0.5
	}
	SubShader
	{
		Tags{ "RenderType" = "Opaque"}
		LOD 100

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

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _BlurTex;
			float _BlurFactor;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 blur = tex2D(_BlurTex,i.uv);
				fixed4 final = col + blur * _BlurFactor;

				return final;
			}
			ENDCG
		}
	}
}
