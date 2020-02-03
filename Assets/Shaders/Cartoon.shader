Shader "Custom/Cartoon"
{
    Properties
    {
        //[Enum(UnityEngine.Rendering.CullMode)] _Cull ("Cull", Float) = 2
    
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _ParTex ("Parameter Texture", 2D) = "white" {}
        
        _BumpMap ("Normal Map", 2D) = "bump" {}
        
        [HDR] _Emission("Emission", Color) = (0,0,0,1)
        _EmissionMap("Emission Map", 2D) = "white" {}
        _EmissionIntensity("Emission Intensity", Range(0, 10)) = 1.0
        
        _AO("Occlusion Map", 2D) = "white" {}
        _AmbientOcclusion("Ambient Occlusion", Range(0, 1)) = 0
        
        _ShadowAttWeight ("Shadow Attenuation", Range(0,1)) = 0.0
        _DividLineH("High Division", Range(0, 1)) = 0.0
        _DividLineM("Mid Division", Range(0, 1)) = 0.0
        _DividLineD("Dark Division", Range(0, 1)) = 0.0
        
        _SpecMap("Specular Map", 2D) = "white" {}
        _DividLineSpec("Specular Division", Range(0, 1)) = 0.0
        _SpecIntensity("Specular Intensity", Range(0, 1)) = 1.0
        
        _FresnelEff("Fresnel Effect", Range(0, 1)) = 0.0
        
        _SSSColor ("Subsurface Scattering Color", Color) = (1,0,0,1)
		_SSSColorSub("Subsurface Scattering 2nd Color", Color) = (0.8,0,0.2,1)
		_SSSWeight ("Subsurface Scattering Weight", Range(0,1)) = 0.5
		_SSSSize("Subsurface Scattering Size", Range(0,1)) = 0.5
		_SSForwardAtt("Forward Scattering Attenuation", Range(0,1)) = 0.5
        
		_OutlineWidth("Outline Width", Range(0, 0.5)) = 0.024
		_OutlineColor("Outline Color", Color) = (0.5,0.5,0.5,1)
		
        // Settings
		[HideInInspector] _Fold("__fld", Float) = 1.0
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        //Cull [_Cull]
        LOD 200

        CGPROGRAM

        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Toon fullforwardshadows
        
        #define _NORMAL_MAP 1
        
        #include "AnimeInc.cginc"

        ENDCG
        Pass
        {
            Name "OUTLINE"
            Tags { "LightMode" = "Always" }
            Cull Front
            ZWrite On
            ColorMask RGB
            Blend SrcAlpha OneMinusSrcAlpha
    
            CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
        
            #pragma shader_feature _OUTLINE_ENABLED

			#include "UnityCG.cginc"
            #include "OutlineInc.cginc"

			ENDCG

		}//Pass
    }
    FallBack "Diffuse"
    
    CustomEditor "CartoonShaderGUI"
}
