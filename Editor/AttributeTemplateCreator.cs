#if UNITY_EDITOR
using System.IO;
using UnityEditor;

public static class AttributeTemplateCreator
{
    static string TEMPLATE_BASE_PATH =  @"Packages/com.partisanprogrammer.base-templates/Editor/Templates/";
    const string ASSET_PATH = "Assets/Create/Templates/Attributes/";
    
    [MenuItem(ASSET_PATH + "Attribute", priority = 30)]
    public static void CreateAttributeMenuItem()
    {
        var filename = "AttributeTemplate.txt";
        CreateScriptAssetFromTemplateFile(filename);
    }
    
    public static void CreateScriptAssetFromTemplateFile(string templateName)
    {
        var createdFileName = templateName.Insert(0, "New")
            .Replace("Template.txt", ".cs");
        var fullTemplateFilePath = Path.Join (TEMPLATE_BASE_PATH, templateName);
        ProjectWindowUtil.CreateScriptAssetFromTemplateFile(fullTemplateFilePath, createdFileName);
    }
}
#endif