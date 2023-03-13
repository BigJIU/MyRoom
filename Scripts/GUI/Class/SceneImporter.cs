#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public abstract class SceneImporter: ScriptableWizard
{
    public  Texture2D floorTexture;
    public  Texture2D wallTexture;
    public  string houseId;
    public string JsonStr;
    public  bool existWall = true;
    public  bool existFloor = true;
    public  bool useRawModel = true;
    public GameObject TargetFloor = null;

    //DrawingScope

    /*public SceneImporter()
    {
        
    }*/
    public static T GenerateData<T>(string inputStr)
    {
        return JsonConvert.DeserializeObject<T>(inputStr);
    }

    public virtual T2 SelectRoom<T1,T2> (T1 ds)
    {
        Debug.LogError("WARNING: Default Select Policy");
        return (T2)default(T2);
    }
    
    public T ImportRoom<T>(string input = null)
    {
        Debug.Log("GenerateRoom");
        // Get dataSUNCG
        if (string.IsNullOrEmpty(input))
        {
            // Read by houseID
            if (!string.IsNullOrEmpty(houseId))
            {
                Debug.Log("Read from houseID");
                string path = $@"{Config.sourcePath}\{houseId}.json";
                EditorPrefs.SetString("origin_JSON",File.ReadAllText(path));
            }
            else
            {
                Debug.Log("Null input");
            }
        }
        else
        {
            // Read by stringJson Content
            Debug.Log("Read from str");
            EditorPrefs.SetString("origin_JSON",input);
        }
        T ds = GenerateData<T>(EditorPrefs.GetString("origin_JSON"));
        
        return ds;
    }
    void OnWizardCreate()
    {
        BuildRoom();
        //BuildRooms();
    }

    public virtual void BuildRoom()
    {
        
    }
    
}
#endif