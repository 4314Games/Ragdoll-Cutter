Shader "MWM/Reflective/Reflective Bump Ambient Cube" {
	Properties {
		_MainTex ("Diffuse(RGB), Gloss(A)", 2D) = "white" {}
		_BumpMap ("Normal", 2D) = "bump" {}
		_AmbCube ("Ambient Cube Map", CUBE) = "" {}
		_RefCube ("Reflection Cube Map", CUBE) = "" {}
		_AmbPower ("Ambient Power", Float) = 0
		_RflPower ("Reflectivity", Float) = 1
	}
	SubShader {
		Tags {"Rendertype" = "Opaque"}
		CGPROGRAM
		
		#pragma surface surf Lambert
		
		struct Input {
			fixed2 uv_MainTex;
			fixed3 worldRefl;
			fixed3 worldNormal;
			INTERNAL_DATA
		};
		
		sampler2D _MainTex;
		sampler2D _BumpMap;
		samplerCUBE _AmbCube;
		samplerCUBE _RefCube;
		fixed _SpecPower;
		fixed _RflPower;
		fixed _AmbPower;

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 tex = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = tex.rgb;
			o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_MainTex));
			o.Gloss = tex.a;
			o.Emission = tex.rgb * (_AmbPower * texCUBE (_AmbCube, WorldNormalVector (IN, o.Normal)).rgb) + _RflPower * tex.a * texCUBE (_RefCube, WorldReflectionVector(IN, o.Normal)).rgb;
		}
		
		ENDCG
	}
	Fallback "Diffuse"
}