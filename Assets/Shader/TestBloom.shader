Shader "Hidden/TestBlood"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Radius("Radius",Range(0,20)) = 5
		_TextureSizeX("TextureSizeX",Float) = 256
		_TextureSizeY("TextureSizeY",Float) = 256
		_BloomFactor("BloomFactor",Range(0,1)) = 0.5
		_ColorThreshold("ColorThreshold",Color) = (0.5,0.5,0.5,1)
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

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
			float4 _MainTex_ST;
			int _Radius;
			float _TextureSizeX;
			float _TextureSizeY;
			float _BloomFactor;
			fixed4 _ColorThreshold;

			fixed4 BlurTexture(float2 uv,int radius,float textureSizeX,float textureSizeY){
				float pixelSizeX = 1.0/textureSizeX;
				float pixelSizeY = 1.0/textureSizeY;
				int count = radius * 2 +1;
				count *= count;
				float4 tmpColor = float4(0,0,0,0);
				for(int x = -radius;x<=radius;x++){
					for(int y = -radius;y<=radius;y++){
						float4 color = tex2D(_MainTex,uv+float2(x*pixelSizeX,y*pixelSizeY));
						color = saturate(color - _ColorThreshold);
						tmpColor += color;
					}
				}
				return tmpColor/count;
			}
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 orgColor = tex2D(_MainTex, i.uv);
				fixed4 col = BlurTexture(i.uv,_Radius,_TextureSizeX,_TextureSizeY);
				fixed4 final = orgColor + col * _BloomFactor;

				return final;
			}
			ENDCG
		}
	}
}
