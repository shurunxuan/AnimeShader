using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
public class CartoonShaderGUI : ShaderGUI
{
    Material _material = null;
    MaterialProperty[] _props = null;
    MaterialEditor _materialEditor = null;
    private string[] _keywords = null;

    // General Properties
    private MaterialProperty _Color = null;
    private MaterialProperty _MainTex = null;
    private MaterialProperty _GlossinessMap = null;
    private MaterialProperty _Glossiness = null;
    private MaterialProperty _ParTex = null;

    // Normal Map
    private MaterialProperty _BumpMap = null;

    // Emission
    private MaterialProperty _Emission = null;
    private MaterialProperty _EmissionMap = null;
    private MaterialProperty _EmissionIntensity = null;

    // Ambient Occlusion
    private MaterialProperty _AOMap = null;
    private MaterialProperty _AOWeight = null;

    // Diffuse
    private MaterialProperty _ShadowAttenuation = null;
    private MaterialProperty _HighDivision = null;
    private MaterialProperty _MidDivision = null;
    private MaterialProperty _LowDivision = null;

    // Specular
    private MaterialProperty _SpecMap = null;
    private MaterialProperty _SpecDivision = null;
    private MaterialProperty _SpecIntensity = null;

    // Fresnel
    private MaterialProperty _FresnelIntensity = null;

    // SubSurface Scattering
    private MaterialProperty _ScatteringColor = null;
    private MaterialProperty _ScatteringColorSub = null;
    private MaterialProperty _ScatteringWeight = null;
    private MaterialProperty _ScatteringSize = null;
    private MaterialProperty _ScatteringAttenuation = null;

    // Outline
    private MaterialProperty _OutlineWidth = null;
    private MaterialProperty _OutlineColor = null;

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        _materialEditor = materialEditor;
        _material = materialEditor.target as Material;
        _props = properties;
        _keywords = _material.shaderKeywords;

        AssignProperties();

        Layout.Initialize(_material);

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(-7);
        EditorGUILayout.BeginVertical();
        EditorGUI.BeginChangeCheck();
        //EditorGUI.showMixedValue = true;
        DrawGUI();
        EditorGUI.EndChangeCheck();
        EditorGUILayout.EndVertical();
        GUILayout.Space(1);
        EditorGUILayout.EndHorizontal();

        Undo.RecordObject(_material, "Material Edition");
    }

    void AssignProperties()
    {
        _Color = FindProperty("_Color", _props);
        _MainTex = FindProperty("_MainTex", _props);
        _Glossiness = FindProperty("_Glossiness", _props);
        _GlossinessMap = FindProperty("_GlossinessMap", _props);
        _ParTex = FindProperty("_ParTex", _props);

        _BumpMap = FindProperty("_BumpMap", _props, false);

        _Emission = FindProperty("_Emission", _props);
        _EmissionMap = FindProperty("_EmissionMap", _props);
        _EmissionIntensity = FindProperty("_EmissionIntensity", _props);

        _AOMap = FindProperty("_AOMap", _props);
        _AOWeight = FindProperty("_AOWeight", _props);

        _ShadowAttenuation = FindProperty("_ShadowAttenuation", _props);
        _HighDivision = FindProperty("_HighDivision", _props);
        _MidDivision = FindProperty("_MidDivision", _props);
        _LowDivision = FindProperty("_LowDivision", _props);

        _SpecMap = FindProperty("_SpecMap", _props);
        _SpecDivision = FindProperty("_SpecDivision", _props);
        _SpecIntensity = FindProperty("_SpecIntensity", _props);

        _FresnelIntensity = FindProperty("_FresnelIntensity", _props);

        _ScatteringColor = FindProperty("_ScatteringColor", _props);
        _ScatteringColorSub = FindProperty("_ScatteringColorSub", _props);
        _ScatteringWeight = FindProperty("_ScatteringWeight", _props);
        _ScatteringSize = FindProperty("_ScatteringSize", _props);
        _ScatteringAttenuation = FindProperty("_ScatteringAttenuation", _props);

        _OutlineWidth = FindProperty("_OutlineWidth", _props);
        _OutlineColor = FindProperty("_OutlineColor", _props);
    }

    enum Category
    {
        General,
        Effects
    }

    void DrawGUI()
    {
        if (Layout.BeginFold((int) Category.General, "- General Settings -"))
        {
            DrawGeneralSettings();
            DrawEmissionSettings();
            DrawNormalMap();
            DrawOcclusionSettings();
        }

        Layout.EndFold();

        if (Layout.BeginFold((int) Category.Effects, "- Effects -"))
        {
            DrawDiffuseSettings();
            DrawSpecularSettings();
            DrawScatteringSettings();
            DrawOutlineSettings();
        }

        Layout.EndFold();

        //base.OnGUI(_materialEditor, _props);
    }

    private float ofs = 0.0f;

    void StartDrawing(string label = null)
    {
        GUILayout.Space(-3);
        if (label != null)
            GUILayout.Label(label, EditorStyles.boldLabel);
        ofs = EditorGUIUtility.labelWidth;
        EditorGUI.indentLevel++;
        _materialEditor.SetDefaultGUIWidths();
    }

    void EndDrawing()
    {
        EditorGUI.indentLevel--;
    }

    void DrawGeneralSettings()
    {
        StartDrawing();
        GUILayout.Space(2);
        EditorGUIUtility.labelWidth = 0;
        _materialEditor.TexturePropertySingleLine(new GUIContent("Albedo"), _MainTex, _Color);
        EditorGUIUtility.labelWidth = ofs;

        if (!_material.IsKeywordEnabled("_PARAMETER_TEXTURE"))
        {
            GUILayout.Space(2);
            EditorGUIUtility.labelWidth = 0;
            _materialEditor.TexturePropertySingleLine(new GUIContent("Smoothness Map"), _GlossinessMap);
            EditorGUIUtility.labelWidth = ofs;
        }

        _materialEditor.ShaderProperty(_Glossiness, "Smoothness");
        bool toggle = Array.IndexOf(_keywords, "_PARAMETER_TEXTURE") != -1;
        EditorGUI.BeginChangeCheck();
        
        toggle = EditorGUILayout.Toggle("Enable Parameter Texture", toggle);
        if (EditorGUI.EndChangeCheck())
        {
            if (toggle)
            {
                _material.EnableKeyword("_PARAMETER_TEXTURE");
            }
            else
            {
                _material.DisableKeyword("_PARAMETER_TEXTURE");
            }
        }

        if (_material.IsKeywordEnabled("_PARAMETER_TEXTURE"))
        {
            GUILayout.Space(2);
            EditorGUIUtility.labelWidth = 0;
            _materialEditor.TexturePropertySingleLine(new GUIContent("Parameter Texture"), _ParTex);
            EditorGUIUtility.labelWidth = ofs;
        }

        EndDrawing();
    }

    void DrawNormalMap()
    {
        if (_BumpMap == null) return;
        StartDrawing();
        GUILayout.Space(2);
        EditorGUIUtility.labelWidth = 0;
        _materialEditor.TexturePropertySingleLine(new GUIContent("Normal Map"), _BumpMap);
        EditorGUIUtility.labelWidth = ofs;
        EndDrawing();
    }

    void DrawEmissionSettings()
    {
        StartDrawing();
        GUILayout.Space(2);
        EditorGUIUtility.labelWidth = 0;
        _materialEditor.TexturePropertySingleLine(new GUIContent("Emission"), _EmissionMap, _Emission);
        EditorGUIUtility.labelWidth = ofs;
        _materialEditor.ShaderProperty(_EmissionIntensity, "Intensity");
        EndDrawing();
    }

    void DrawOcclusionSettings()
    {
        StartDrawing("Ambient Occlusion");
        EditorGUIUtility.labelWidth = 0;
        _materialEditor.TexturePropertySingleLine(new GUIContent("Occlusion Map"), _AOMap);
        EditorGUIUtility.labelWidth = ofs;
        _materialEditor.ShaderProperty(_AOWeight, "Weight");
        EndDrawing();
    }

    void DrawDiffuseSettings()
    {
        StartDrawing("Diffuse");
        _materialEditor.ShaderProperty(_ShadowAttenuation, "Shadow Attenuation");
        _materialEditor.ShaderProperty(_HighDivision, "High Division");
        _materialEditor.ShaderProperty(_MidDivision, "Mid Division");
        _materialEditor.ShaderProperty(_LowDivision, "Dark Division");
        EndDrawing();
    }

    void DrawSpecularSettings()
    {
        StartDrawing("Specular");
        if (!_material.IsKeywordEnabled("_PARAMETER_TEXTURE"))
        {
            GUILayout.Space(2);
            EditorGUIUtility.labelWidth = 0;
            _materialEditor.TexturePropertySingleLine(new GUIContent("Specular Map"), _SpecMap);
            EditorGUIUtility.labelWidth = ofs;
        }

        _materialEditor.ShaderProperty(_SpecDivision, "Division");
        _materialEditor.ShaderProperty(_SpecIntensity, "Intensity");
        _materialEditor.ShaderProperty(_FresnelIntensity, "Fresnel");
        EndDrawing();
    }

    void DrawScatteringSettings()
    {
        StartDrawing("Subsurface Scattering");
        bool toggle = Array.IndexOf(_keywords, "_SUBSURFACE_SCATTERING") != -1;
        EditorGUI.BeginChangeCheck();
        
        toggle = EditorGUILayout.Toggle("Enable", toggle);
        if (EditorGUI.EndChangeCheck())
        {
            if (toggle)
            {
                _material.EnableKeyword("_SUBSURFACE_SCATTERING");
            }
            else
            {
                _material.DisableKeyword("_SUBSURFACE_SCATTERING");
            }
        }

        if (_material.IsKeywordEnabled("_SUBSURFACE_SCATTERING"))
        {
            _materialEditor.ShaderProperty(_ScatteringColor, "Color");
            _materialEditor.ShaderProperty(_ScatteringColorSub, "2nd Color");
            _materialEditor.ShaderProperty(_ScatteringWeight, "Weight");
            _materialEditor.ShaderProperty(_ScatteringSize, "Size");
            _materialEditor.ShaderProperty(_ScatteringAttenuation, "Forward Attenuation");
        }
        EndDrawing();
    }

    void DrawOutlineSettings()
    {
        StartDrawing("Outline");

        bool toggle = Array.IndexOf(_keywords, "_OUTLINE_ENABLED") != -1;
        EditorGUI.BeginChangeCheck();
        toggle = EditorGUILayout.Toggle("Enable", toggle);
        if (EditorGUI.EndChangeCheck())
        {
            if (toggle)
            {
                _material.EnableKeyword("_OUTLINE_ENABLED");
            }
            else
            {
                _material.DisableKeyword("_OUTLINE_ENABLED");
            }
        }

        if (_material.IsKeywordEnabled("_OUTLINE_ENABLED"))
        {
            _materialEditor.ShaderProperty(_OutlineColor, "Color");
            _materialEditor.ShaderProperty(_OutlineWidth, "Width");
        }

        EndDrawing();
    }
}