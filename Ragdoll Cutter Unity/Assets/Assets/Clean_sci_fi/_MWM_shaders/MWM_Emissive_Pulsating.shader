Shader "MWM/Emissive/Emissive Pulse" {
	Properties {
		_MainTex ("Diffuse", 2D) = "white" {}
		_EmitTex ("Emission", 2D) = "black" {}
		_EmitPow ("Emission Power", Float) = 1
		_ThrobFreq ("Throb Frequency", Range(0.0,200.0)) = 0.0
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
		fixed _ThrobFreq;

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 tex = tex2D (_MainTex, IN.uv_MainTex);
			fixed4 eTex = tex2D (_EmitTex, IN.uv_MainTex);
			float throb = max(0.0, sin(_Time.x * _ThrobFreq));
			o.Albedo = tex.rgb;
			o.Emission = throb * (tex.a * (_EmitPow * eTex ));
		}
		
		ENDCG
	}
	Fallback "Diffuse"
}