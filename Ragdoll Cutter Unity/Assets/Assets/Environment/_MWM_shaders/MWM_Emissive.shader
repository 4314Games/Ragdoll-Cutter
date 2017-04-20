Shader "MWM/Emissive/Emissive" {
	Properties {
		_MainTex ("Diffuse", 2D) = "white" {}
		_EmitTex ("Emission", 2D) = "black" {}
		_EmitPow ("Emission Power", Float) = 1
	}
	SubShader {
		Tags {"Rendertype" = "Opaque"}
		CGPROGRAM
		
		#pragma surface surf Lambert
		
		struct Input {
			fixed2 uv_MainTex;
		};
		
		sampler2D _MainTex;
		sampler2D _EmitTex;
		fixed _EmitPow;

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 tex = tex2D (_MainTex, IN.uv_MainTex);
			fixed4 eTex = tex2D (_EmitTex, IN.uv_MainTex);
			o.Albedo = tex.rgb;
			o.Emission = tex.a * (_EmitPow * eTex );
		}
		
		ENDCG
	}
	Fallback "Diffuse"
}