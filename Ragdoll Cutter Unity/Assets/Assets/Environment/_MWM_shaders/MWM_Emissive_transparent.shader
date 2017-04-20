Shader "MWM/Emissive/Simple_Emissive_transparent" {
	Properties {
		_MainTex ("Base (RGB) Alpha(A)", 2D) = "white" {}
		_EmitTex ("Emission", 2D) = "black" {}
		_EmitPow ("Emission Power", Float) = 1
	}
	SubShader {
		Tags { "Queue"="Transparent" "RenderType"="Transparent" }
		LOD 200
		
		CGPROGRAM
		//#pragma surface surf Lambert alpha
		#pragma surface surf Lambert decal:blend

		struct Input {
			float2 uv_MainTex;
		};
		
		sampler2D _MainTex;
		sampler2D _EmitTex;
		fixed _EmitPow;

		void surf (Input IN, inout SurfaceOutput o) {
			half4 tex = tex2D (_MainTex, IN.uv_MainTex);
			fixed4 eTex = tex2D (_EmitTex, IN.uv_MainTex);
			o.Albedo = tex.rgb;
			o.Alpha = tex.a;
			o.Emission = _EmitPow * eTex;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
