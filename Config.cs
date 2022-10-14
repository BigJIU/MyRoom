using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static class Config
{
    public static string ModelPath = @"E:\3DFront\3D-FUTURE-model\3D-FUTURE-model\";
    public static string HomePath = @"E:\3DFront\room\room\";

    public static string sourcePath = @"E:\3DFront\";

    public static string logPath = @"E:\3DFront\";

    public static Texture2D wallTexture;
    public static Texture2D floorTexture;

}

public class PublicConfig : ScriptableWizard
{
    public string ModelPath = Config.ModelPath;
    public string HomePath = Config.HomePath;

    public string sourcePath = Config.sourcePath;

    public string logPath = Config.logPath;
    
    void OnWizardCreate()
    {
        changeConfig();
        end:;
    }

    void changeConfig()
    {
        Config.ModelPath = ModelPath;
        Config.HomePath = HomePath;
        Config.sourcePath = sourcePath;
        Config.logPath = logPath;
    }
    
}