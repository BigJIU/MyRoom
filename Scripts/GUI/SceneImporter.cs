#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using SUNCGData;
using UnityEngine;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine.UI;

[Serializable]
public class SceneImporter: ScriptableWizard
{
    private SUNCGDataStructure dataSUCNG;
    private List<Node> avaRoom;
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
    
    public static List<Node> GenerateSUNCGRoom (SUNCGDataStructure SUNCGData)
    {
        // Get avaRoom
        List<Node> tmpRoom = new List<Node>();
        Debug.Log("Room JID:"+SUNCGData.levels[0].nodes[0].modelId);
        foreach (Node node in SUNCGData.levels[0].nodes)
        {
            if (node.roomTypes != null&&node.nodeIndices.Length>0)
            {
                tmpRoom.Add(node);
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
        dataSUCNG = GenerateData<SUNCGDataStructure>(EditorPrefs.GetString("origin_JSON"));
        avaRoom = GenerateSUNCGRoom(dataSUCNG);
        Config.floorTexture = floorTexture;
        Config.wallTexture = wallTexture;
        ViewerWindow.ClearRoom();
        SUNCGSceneBuilder.NewRoomBuild(dataSUCNG,avaRoom[0],existWall,existFloor,useRawModel,TargetFloor);
    }

    
    void OnWizardCreate()
    {
        GenerateRoom(stringJson);
    }
    

}
#endif