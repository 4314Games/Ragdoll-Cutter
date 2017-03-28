Shader "MWM/Reflective/Reflective Bump Ambient Colour" {
	Properties {
		_MainTex ("Diffuse(RGB), Gloss(A)", 2D) = "white" {}
		_BumpMap ("Normal", 2D) = "bump" {}
		_Cube ("Cube Map", CUBE) = "" {}
		_RflPower ("Reflectivity", Float) = 1
		_AmbColor ("Ambient Value", Color) = (0,0,0,0)
	}
	SubShader {
		Tags {"Rendertype" = "Opaque"}
		CGPROGRAM
		
		#pragma surface surf Lambert noambient
		#pragma exclude_renderers flash
		
		struct Input {
			fixed2 uv_MainTex;
			fixed3 worldRefl;
			INTERNAL_DATA
		};
		
		sampler2D _MainTex;
		sampler2D _BumpMap;
		samplerCUBE _Cube;
		fixed4 _AmbColor;
		fixed _RflPower;

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 tex = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = tex.rgb;
			o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_MainTex));
			o.Gloss = tex.a;
			o.Emission = (_AmbColor *tex.rgb)+(_RflPower * tex.a * texCUBE (_Cube, WorldReflectionVector(IN, o.Normal)).rgb);
		}
		
		ENDCG
	}
	Fallback "Specular"
}