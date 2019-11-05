Shader "Image Effects/ Glitch effect 1"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_MinDif("Min diff", range(0,1)) = 0.1
		_MaxDif("Max diff", range(0,1)) = 0.5
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always
		  
		 GrabPass
        {
            "_GrabTex"
        }

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
				float4 grabPos : TEXCOORD1;
				float4 scrPos : TEXCOORD2;
				
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.grabPos = ComputeGrabScreenPos(o.vertex);
				o.scrPos = ComputeScreenPos(o.vertex);
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _GrabTex;
			//sampler2D _CameraDepthTexture;
			sampler2D _CameraMotionVectorsTexture;
			//sampler2D _CameraDepthNormalsTexture;

			float _MinDif;
			float _MaxDif;

			 float dif(float3 col1, float3 col2){
			 	 float r,g,b;
				 r = abs(col1.r-col2.r);
				 g = abs(col1.g-col2.g);
				 b = abs(col1.b-col2.b);
				 if(r > g && r>b){
				 	 return r;
				 }else{
				 	 if(g >b){
					 	 return g;
					 }else{
					 	 return b;
					 }
				 }
			 }

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv );
				float4 motionVectors = tex2D(_CameraMotionVectorsTexture,i.uv);
				fixed4 result = fixed4(1,1,1,1);
				float2 disp = float2(motionVectors.x,motionVectors.y);
				result.rgb = tex2D(_GrabTex,i.grabPos + disp);
				if(dif(result,col)>_MinDif && dif(result,col)<_MaxDif) {
					result = col;
				}

				if(length(result.rgb)<0.1){
					result = col;
				}

				return result;
			}
			ENDCG
		}
	}
}
