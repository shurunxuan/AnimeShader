using System;
using UnityEngine;
using UnityEditor.Experimental.AssetImporters;
using System.IO;
using UnityEditor;

[ScriptedImporter(1, "props")]
public class UE4MatPropsImporter : ScriptedImporter
{
    private struct Parameter
    {
        public string ParameterName;
        public string ParameterValue;
        public string ParameterInfo;
    }

    private struct MaterialPreset
    {
        public float Smoothness;
        public Color Emission;
        public float EmissionIntensity;
        public float DiffuseShadowAtt;
        public float DiffuseHighDiv;
        public float DiffuseMidDiv;
        public float DiffuseDarkDiv;
        public float SpecularDiv;
        public float SpecularIntensity;
        public float Fresnel;
        public bool SSEnabled;
        public float SSWeight;
        public float SSSize;
        public float SSForwardAtt;
        public bool OutlineEnabled;
        public Color OutlineColor;
        public float OutlineWidth;
    }
    
    private readonly MaterialPreset body = new MaterialPreset
    {
        Smoothness = 0.635f,
        Emission = Color.black,
        EmissionIntensity = 1.0f,
        DiffuseShadowAtt = 0.11f,
        DiffuseHighDiv = 0.521f,
        DiffuseMidDiv = 0.348f,
        DiffuseDarkDiv = 0.033f,
        SpecularDiv = 0.78f,
        SpecularIntensity = 0.54f,
        Fresnel = 1.0f,
        SSEnabled = false,
        SSWeight = 0.0f,
        SSSize = 0.0f,
        SSForwardAtt = 0.0f,
        OutlineEnabled = true,
        OutlineColor = Color.black,
        OutlineWidth = 0.025f
    };
    
    private readonly MaterialPreset hair = new MaterialPreset
    {
        Smoothness = 0.327f,
        Emission = Color.black,
        EmissionIntensity = 1.0f,
        DiffuseShadowAtt = 0.11f,
        DiffuseHighDiv = 0.521f,
        DiffuseMidDiv = 0.348f,
        DiffuseDarkDiv = 0.033f,
        SpecularDiv = 0.78f,
        SpecularIntensity = 0.368f,
        Fresnel = 1.0f,
        SSEnabled = false,
        SSWeight = 0.0f,
        SSSize = 0.0f,
        SSForwardAtt = 0.0f,
        OutlineEnabled = true,
        OutlineColor = Color.black,
        OutlineWidth = 0.025f
    };
    
    private readonly MaterialPreset hairParts = new MaterialPreset
    {
        Smoothness = 0.586f,
        Emission = Color.black,
        EmissionIntensity = 1.0f,
        DiffuseShadowAtt = 0.11f,
        DiffuseHighDiv = 0.521f,
        DiffuseMidDiv = 0.348f,
        DiffuseDarkDiv = 0.033f,
        SpecularDiv = 0.78f,
        SpecularIntensity = 0.54f,
        Fresnel = 1.0f,
        SSEnabled = false,
        SSWeight = 0.0f,
        SSSize = 0.0f,
        SSForwardAtt = 0.0f,
        OutlineEnabled = true,
        OutlineColor = Color.black,
        OutlineWidth = 0.025f
    };
    
    private readonly MaterialPreset hair_emit = new MaterialPreset
    {
        Smoothness = 0.327f,
        Emission = Color.white,
        EmissionIntensity = 1.0f,
        DiffuseShadowAtt = 0.11f,
        DiffuseHighDiv = 0.521f,
        DiffuseMidDiv = 0.348f,
        DiffuseDarkDiv = 0.033f,
        SpecularDiv = 0.78f,
        SpecularIntensity = 0.368f,
        Fresnel = 1.0f,
        SSEnabled = false,
        SSWeight = 0.0f,
        SSSize = 0.0f,
        SSForwardAtt = 0.0f,
        OutlineEnabled = true,
        OutlineColor = Color.black,
        OutlineWidth = 0.025f
    };
    
    private readonly MaterialPreset hairParts_emit = new MaterialPreset
    {
        Smoothness = 0.586f,
        Emission = Color.white,
        EmissionIntensity = 2.04f,
        DiffuseShadowAtt = 0.11f,
        DiffuseHighDiv = 0.521f,
        DiffuseMidDiv = 0.348f,
        DiffuseDarkDiv = 0.033f,
        SpecularDiv = 0.78f,
        SpecularIntensity = 0.54f,
        Fresnel = 1.0f,
        SSEnabled = false,
        SSWeight = 0.0f,
        SSSize = 0.0f,
        SSForwardAtt = 0.0f,
        OutlineEnabled = true,
        OutlineColor = Color.black,
        OutlineWidth = 0.025f
    };
    
    private readonly MaterialPreset face_mayu_r0 = new MaterialPreset
    {
        Smoothness = 0.327f,
        Emission = Color.black,
        EmissionIntensity = 1.0f,
        DiffuseShadowAtt = 0.11f,
        DiffuseHighDiv = 0.521f,
        DiffuseMidDiv = 0.348f,
        DiffuseDarkDiv = 0.033f,
        SpecularDiv = 0.78f,
        SpecularIntensity = 0.368f,
        Fresnel = 1.0f,
        SSEnabled = false,
        SSWeight = 0.0f,
        SSSize = 0.0f,
        SSForwardAtt = 0.0f,
        OutlineEnabled = false,
        OutlineColor = Color.black,
        OutlineWidth = 0.025f
    };
    
    private readonly MaterialPreset face_r0t0 = new MaterialPreset
    {
        Smoothness = 0.0f,
        Emission = new Color(0.119f, 0.119f, 0.119f, 1.0f),
        EmissionIntensity = 1.0f,
        DiffuseShadowAtt = 0.11f,
        DiffuseHighDiv = 0.521f,
        DiffuseMidDiv = 0.348f,
        DiffuseDarkDiv = 0.033f,
        SpecularDiv = 0.78f,
        SpecularIntensity = 0.54f,
        Fresnel = 1.0f,
        SSEnabled = false,
        SSWeight = 0.0f,
        SSSize = 0.0f,
        SSForwardAtt = 0.0f,
        OutlineEnabled = false,
        OutlineColor = Color.black,
        OutlineWidth = 0.025f
    };
    
    private readonly MaterialPreset acc = new MaterialPreset
    {
        Smoothness = 0.635f,
        Emission = Color.black,
        EmissionIntensity = 1.0f,
        DiffuseShadowAtt = 0.11f,
        DiffuseHighDiv = 0.521f,
        DiffuseMidDiv = 0.348f,
        DiffuseDarkDiv = 0.033f,
        SpecularDiv = 0.78f,
        SpecularIntensity = 0.54f,
        Fresnel = 1.0f,
        SSEnabled = false,
        SSWeight = 0.0f,
        SSSize = 0.0f,
        SSForwardAtt = 0.0f,
        OutlineEnabled = true,
        OutlineColor = Color.black,
        OutlineWidth = 0.025f
    };
    
    private readonly MaterialPreset acc_emit = new MaterialPreset
    {
        Smoothness = 0.738f,
        Emission = Color.white,
        EmissionIntensity = 1.92f,
        DiffuseShadowAtt = 0.11f,
        DiffuseHighDiv = 0.521f,
        DiffuseMidDiv = 0.348f,
        DiffuseDarkDiv = 0.033f,
        SpecularDiv = 0.78f,
        SpecularIntensity = 0.54f,
        Fresnel = 1.0f,
        SSEnabled = false,
        SSWeight = 0.0f,
        SSSize = 0.0f,
        SSForwardAtt = 0.0f,
        OutlineEnabled = false,
        OutlineColor = Color.black,
        OutlineWidth = 0.025f
    };
    
    private readonly MaterialPreset skin = new MaterialPreset
    {
        Smoothness = 0.143f,
        Emission = Color.black,
        EmissionIntensity = 1.0f,
        DiffuseShadowAtt = 0.11f,
        DiffuseHighDiv = 0.871f,
        DiffuseMidDiv = 0.463f,
        DiffuseDarkDiv = 0.0f,
        SpecularDiv = 0.901f,
        SpecularIntensity = 0.688f,
        Fresnel = 1.0f,
        SSEnabled = true,
        SSWeight = 0.124f,
        SSSize = 0.098f,
        SSForwardAtt = 0.392f,
        OutlineEnabled = true,
        OutlineColor = Color.black,
        OutlineWidth = 0.025f
    };
    
    private readonly MaterialPreset wing = new MaterialPreset
    {
        Smoothness = 0.603f,
        Emission = Color.black,
        EmissionIntensity = 1.0f,
        DiffuseShadowAtt = 0.11f,
        DiffuseHighDiv = 0.871f,
        DiffuseMidDiv = 0.463f,
        DiffuseDarkDiv = 0.0f,
        SpecularDiv = 0.901f,
        SpecularIntensity = 0.688f,
        Fresnel = 1.0f,
        SSEnabled = false,
        SSWeight = 0.0f,
        SSSize = 0.0f,
        SSForwardAtt = 0.0f,
        OutlineEnabled = true,
        OutlineColor = Color.black,
        OutlineWidth = 0.025f
    };

    void SetMaterialPreset(Material mat, MaterialPreset preset)
    {
        mat.SetFloat("_Glossiness", preset.Smoothness);
        mat.SetColor("_Emission", preset.Emission);
        mat.SetFloat("_EmissionIntensity", preset.EmissionIntensity);
        mat.SetFloat("_ShadowAttenuation", preset.DiffuseShadowAtt);
        mat.SetFloat("_HighDivision", preset.DiffuseHighDiv);
        mat.SetFloat("_MidDivision", preset.DiffuseMidDiv);
        mat.SetFloat("_LowDivision", preset.DiffuseDarkDiv);
        mat.SetFloat("_SpecDivision", preset.SpecularDiv);
        mat.SetFloat("_SpecIntensity", preset.SpecularIntensity);
        mat.SetFloat("_FresnelIntensity", preset.Fresnel);
        if (preset.SSEnabled)
        {
            mat.EnableKeyword("_SUBSURFACE_SCATTERING");
        }
        else
        {
            mat.DisableKeyword("_SUBSURFACE_SCATTERING");
        }
        mat.SetFloat("_ScatteringWeight", preset.SSWeight);
        mat.SetFloat("_ScatteringSize", preset.SSSize);
        mat.SetFloat("_ScatteringAttenuation", preset.SSForwardAtt);
        if (preset.OutlineEnabled)
        {
            mat.EnableKeyword("_OUTLINE_ENABLED");
        }
        else
        {
            mat.DisableKeyword("_OUTLINE_ENABLED");
        }
        mat.SetColor("_OutlineColor", preset.OutlineColor);
        mat.SetFloat("_OutlineWidth", preset.OutlineWidth);
    }

    private AssetImportContext context;

    private Parameter[] TextureParameterValues;

    private string CN4GO_BaseFolder = "Assets\\Resources\\Cyberdimension Neptunia 4 Goddess Online\\";

    public override void OnImportAsset(AssetImportContext ctx)
    {
        context = ctx;

        if (Path.GetFileNameWithoutExtension(context.assetPath).Contains("outline"))
            return;
        
        string[] lines = File.ReadAllLines(ctx.assetPath);

        bool textureDataStarted = false;
        int currentReading = -1;

        foreach (var line in lines)
        {
            string trimedLine = line.Replace("    ", "");

            if (trimedLine.StartsWith("TextureParameterValues"))
            {
                string indexStr = trimedLine.Split('[')[1].Split(']')[0];
                int.TryParse(indexStr, out var index);

                if (!textureDataStarted)
                {
                    TextureParameterValues = new Parameter[index];

                    textureDataStarted = true;
                }
                else
                {
                    currentReading = index;
                }
            }
            else if (trimedLine.StartsWith("ParameterName"))
            {
                if (!textureDataStarted || currentReading == -1) continue;
                var strs = trimedLine.Split(new[] {" = "}, StringSplitOptions.RemoveEmptyEntries);

                TextureParameterValues[currentReading].ParameterName = strs[1];
            }
            else if (trimedLine.StartsWith("ParameterValue"))
            {
                if (!textureDataStarted || currentReading == -1) continue;
                var strs = trimedLine.Split(new[] {" = "}, StringSplitOptions.RemoveEmptyEntries);

                TextureParameterValues[currentReading].ParameterValue = strs[1];
            }
            else if (trimedLine.StartsWith("ParameterInfo"))
            {
                if (!textureDataStarted || currentReading == -1) continue;
                var strs = trimedLine.Split(new[] {" = "}, StringSplitOptions.RemoveEmptyEntries);

                TextureParameterValues[currentReading].ParameterInfo = strs[1];
            }
            else if (trimedLine.StartsWith("}"))
            {
                if (!textureDataStarted) continue;
                if (currentReading != -1)
                {
                    currentReading = -1;
                }
                else
                {
                    textureDataStarted = false;
                }
            }
        }

        CreateMaterial();
    }

    private string GetFileNameFromParameterValue(string parameterValue)
    {
        string path = parameterValue.Split('\'')[1];
        path = path.Replace('/', '\\');
        path = path.Remove(0, 1);
        string texPath = Path.GetDirectoryName(path);
        string filename = Path.GetFileNameWithoutExtension(path);
        return Path.Combine(CN4GO_BaseFolder, texPath, filename + ".png");
        //return baseFolder + "\\" + Path.GetFileNameWithoutExtension(filename) + ".png";
    }

    private void CreateMaterial()
    {
        int clrTexIndex = -1;
        int nrmTexIndex = -1;
        int parTexIndex = -1;
        int emiTexIndex = -1;
        
        if (TextureParameterValues == null) return;
        
        for (int i = 0; i < TextureParameterValues.Length; i++)
        {
            switch (TextureParameterValues[i].ParameterName)
            {
                case "ColorTexture":
                    clrTexIndex = i;
                    break;
                case "NomalTexture":
                    nrmTexIndex = i;
                    break;
                case "ParameterTexture":
                    parTexIndex = i;
                    break;
                case "EmissionTexture":
                    emiTexIndex = i;
                    break;
            }
        }

        string shaderName = nrmTexIndex == -1 ? "Custom/AnimeNoNormal" : "Custom/Anime";

        var mat = new Material(Shader.Find(shaderName));

        context.AddObjectToAsset(Path.GetFileNameWithoutExtension(context.assetPath), mat);

        if (clrTexIndex != -1)
        {
            Texture2D clrTex =
                AssetDatabase.LoadAssetAtPath<Texture2D>(
                    GetFileNameFromParameterValue(TextureParameterValues[clrTexIndex].ParameterValue));

            mat.SetTexture("_MainTex", clrTex);
        }

        if (nrmTexIndex != -1)
        {
            string nrmTexPath = GetFileNameFromParameterValue(TextureParameterValues[nrmTexIndex].ParameterValue);
            TextureImporter nrmImporter = (TextureImporter) GetAtPath(nrmTexPath);
            TextureImporterSettings tis = new TextureImporterSettings();
            nrmImporter.ReadTextureSettings(tis);
            tis.textureType = TextureImporterType.NormalMap;
            nrmImporter.SetTextureSettings(tis);
                
            Texture2D nrmTex =
                AssetDatabase.LoadAssetAtPath<Texture2D>(nrmTexPath);

            mat.SetTexture("_BumpMap", nrmTex);
        }

        if (parTexIndex != -1)
        {
            string parTexPath = GetFileNameFromParameterValue(TextureParameterValues[parTexIndex].ParameterValue);
            TextureImporter parImporter = (TextureImporter) GetAtPath(parTexPath);
            TextureImporterSettings tis = new TextureImporterSettings();
            parImporter.ReadTextureSettings(tis);
            tis.sRGBTexture = false;
            parImporter.SetTextureSettings(tis);
            
            Texture2D parTex =
                AssetDatabase.LoadAssetAtPath<Texture2D>(parTexPath);

            mat.EnableKeyword("_PARAMETER_TEXTURE");
            mat.SetTexture("_ParTex", parTex);
        }

        if (emiTexIndex != -1)
        {
            Texture2D emiTex =
                AssetDatabase.LoadAssetAtPath<Texture2D>(
                    GetFileNameFromParameterValue(TextureParameterValues[emiTexIndex].ParameterValue));

            mat.SetTexture("_EmissionMap", emiTex);
            mat.SetColor("_Emission", Color.white);
            mat.SetFloat("_EmissionIntensity", 1.0f);
        }

        mat.DisableKeyword("_SUBSURFACE_SCATTERING");
        mat.EnableKeyword("_OUTLINE_ENABLED");

        string filename = Path.GetFileNameWithoutExtension(context.assetPath);
        if (filename == null)
            return;
        filename = filename.ToLower();

        MaterialPreset preset = body;
        
        if (filename.Contains("body"))
        {
            preset = filename.Contains("skin") ? skin : body;
        }
        else if (filename.Contains("acc"))
        {
            preset = emiTexIndex != -1 ? acc_emit : acc;
        }
        else if (filename.Contains("hair"))
        {
            if (filename.Contains("hairparts"))
            {
                preset = emiTexIndex != -1 ? hairParts_emit : hairParts;
            }
            else
            {
                preset = emiTexIndex != -1 ? hair_emit : hair;
            }
        }
        else if (filename.Contains("wing"))
        {
            preset = wing;
        }
        else if (filename.Contains("face"))
        {
            if (filename.Contains("mayu"))
            {
                preset = face_mayu_r0;
            }
            else if (filename.Contains("r0"))
            {
                preset = filename.Contains("r0t0") ? face_r0t0 : face_mayu_r0;
            }
            else
            {
                preset = skin;
            }
        }
        
        SetMaterialPreset(mat, preset);
    }
}