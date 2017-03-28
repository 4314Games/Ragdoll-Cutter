Shader "MWM/Reflective/Glass Reflective" {
	Properties {
		_GlossTex ("Gloss", 2D) = "white" {}
		_SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
		_Shininess ("Shininess", Range (0.01, 1)) = 0.078125
		_ReflectColor ("Reflection Color", Color) = (1,1,1,0.5)
		_Cube ("Reflection Cubemap", Cube) = "black" { /* used to beTexGen CubeReflect */ }
	}
	SubShader {
		Tags {
			"Queue"="Transparent"
			"IgnoreProjector"="True"
			"RenderType"="Transparent"
		}
		LOD 300
		
		CGPROGRAM
			#pragma surface surf BlinnPhong decal:add nolightmap
			
			samplerCUBE _Cube;
			sampler2D _GlossTex;
			fixed4 _ReflectColor;
			half _Shininess;
			
			struct Input {
				fixed3 worldRefl;
				fixed2 uv_GlossTex;
			};
			
			void surf (Input IN, inout SurfaceOutput o) {
				fixed4 gTex = tex2D (_GlossTex, IN.uv_GlossTex);
				o.Albedo = 0;
				o.Gloss = 1 - gTex.rgb;
				o.Specular = (1 - gTex.rgb) * _Shininess;		
				fixed4 reflcol = texCUBE (_Cube, IN.worldRefl);
				o.Emission = reflcol.rgb * _ReflectColor.rgb * (1 - gTex.rgb);
			}
		ENDCG
	}
	FallBack "Transparent/VertexLit"
}