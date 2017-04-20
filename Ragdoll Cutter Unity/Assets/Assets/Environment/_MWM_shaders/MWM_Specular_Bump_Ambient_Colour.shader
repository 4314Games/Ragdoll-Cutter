Shader "MWM/Specular/Specular Bump Ambient Colour" {
	Properties {
		_MainTex ("Diffuse(RGB), Gloss(A)", 2D) = "white" {}
		_BumpMap ("Normal", 2D) = "bump" {}
		_AmbColor ("Ambient Value", Color) = (0,0,0,0)
		_SpecColor ("Specular Colour", Color) = (1,1,1,1)
		_Shininess ("Shininess",Range (0.05, 2.0) ) = 0.25
	}
	SubShader {
		Tags {"Rendertype" = "Opaque"}
		CGPROGRAM
		
		#pragma surface surf BlinnPhong noambient
		#pragma exclude_renderers flash
		
		struct Input {
			fixed2 uv_MainTex;
			fixed3 worldRefl;
		};
		
		sampler2D _MainTex;
		sampler2D _BumpMap;
		fixed _Shininess;
		fixed4 _AmbColor;

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 tex = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = tex.rgb;
			o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_MainTex));
			o.Specular = _Shininess;
			o.Gloss = tex.a;
			o.Emission = (_AmbColor *tex.rgb);
		}
		
		ENDCG
	}
	Fallback "Specular"
}