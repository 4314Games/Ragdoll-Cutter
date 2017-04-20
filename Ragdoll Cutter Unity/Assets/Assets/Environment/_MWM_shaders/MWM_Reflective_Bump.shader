Shader "MWM/Reflective/Reflective Bump" {
	Properties {
		_MainTex ("Diffuse(RGB), Gloss(A)", 2D) = "white" {}
		_BumpMap ("Normal", 2D) = "bump" {}
		_Cube ("Cube Map", CUBE) = "" {}
		_RflPower ("Reflectivity", Float) = 1
	}
	SubShader {
		Tags {"Rendertype" = "Opaque"}
		CGPROGRAM
		
		#pragma surface surf Lambert
		
		struct Input {
			fixed2 uv_MainTex;
			fixed3 worldRefl;
			INTERNAL_DATA
		};
		
		sampler2D _MainTex;
		sampler2D _BumpMap;
		samplerCUBE _Cube;
		fixed _SpecPower;
		fixed _RflPower;

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 tex = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = tex.rgb;
			o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_MainTex));
			o.Emission = _RflPower * tex.a * texCUBE (_Cube, WorldReflectionVector(IN, o.Normal)).rgb;
		}
		
		ENDCG
	}
	Fallback "Diffuse"
}