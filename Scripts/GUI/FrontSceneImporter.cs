#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using FrontData;
using UnityEngine;
using Newtonsoft.Json;
using UnityEditor;


[Serializable]
public class FrontSceneImporter: ScriptableWizard
{
    private FrontDataStructure dataFront;
    private FrontRoom avaRoom;
    public  Texture2D floorTexture;
    public  Texture2D wallTexture;
    public  string houseId;
    public string stringJson;
    public  bool existWall = true;
    public  bool existFloor = true;
    public  bool useRawModel = true;
    public GameObject TargetFloor = null;
    //DrawingScope

    public static T GenerateData<T>(string inputStr)
    {
        return JsonConvert.DeserializeObject<T>(inputStr);
    }
    
    public static FrontRoom GenerateFrontRoom (FrontDataStructure FrontData,string type = "")
    {
        // Get avaRoom
        FrontRoom tmpRoom = null;
        if (type == "")
        {
            type = "LivingRoom";
        }
        //Debug.Log("Room ref:"+FrontData.levels[0].nodes[0].modelId);
        foreach (FrontRoom node in FrontData.scene.room)
        {
            if (node.type == type)
            {
                tmpRoom = node;
                break;
            }
        }

        return tmpRoom;
    }
    
    
    void GenerateRoom(string input = null)
    {
        Debug.Log("GenerateRoom");
        // Get dataSUNCG
        if (string.IsNullOrEmpty(input))
        {
            // Read by houseID
            if (!string.IsNullOrEmpty(houseId))
            {
                Debug.Log("Read from houseID");
                string path = $@"E:\3DFront\json\{houseId}.json";
                EditorPrefs.SetString("origin_JSON",File.ReadAllText(path));
            }
            else
            {
                Debug.Log("Null input");
                return;
            }
        }
        else
        {
            // Read by stringJson Content
            Debug.Log("Read from str");
            EditorPrefs.SetString("origin_JSON",input);
        }
        dataFront = GenerateData<FrontDataStructure>(EditorPrefs.GetString("origin_JSON"));
        avaRoom = GenerateFrontRoom(dataFront,"LivingRoom");
        Config.floorTexture = floorTexture;
        Config.wallTexture = wallTexture;
        ViewerWindow.ClearRoom();
        FrontSceneBuilder.NewRoomBuild(dataFront,avaRoom,existWall,existFloor,useRawModel,TargetFloor);
    }

    
    void OnWizardCreate()
    {
        GenerateRoom(stringJson);
    }
    

}
#endif