#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using SUNCGData;
using UnityEngine;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine.UI;

[Serializable]
public class SceneImporter: ScriptableWizard
{
    private SUNCGDataStructure dataSUCNG;
    private List<Node> avaRoom;
    public  Texture2D floorTexture;
    public  Texture2D wallTexture;
    public  string houseId = "76";
    public string stringJson;
    public  bool existWall = true;
    public  bool useRawModel = true;

    public static T GenerateData<T>(string inputStr,string inputHouseId)
    {
        // Get dataSUNCG
        if (string.IsNullOrEmpty(inputStr))
        {
            // Read by houseID
            string path = $@"E:\3DFront\json\{inputHouseId}.json";
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
        }
        else
        {
            // Read by stringJson Content
            return JsonConvert.DeserializeObject<T>(inputStr);
        }
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
    
    
    [MenuItem("RoomViewer/Generate")]
    void GenerateRoom(string input = null)
    {
        Debug.Log("GenerateRoom");
        dataSUCNG = GenerateData<SUNCGDataStructure>(input,houseId);
        avaRoom = GenerateSUNCGRoom(dataSUCNG);
        Config.floorTexture = floorTexture;
        Config.wallTexture = wallTexture;
        ViewerWindow.ClearRoom();
        SUNCGSceneBuilder.NewRoomBuild(dataSUCNG,avaRoom[0],existWall,useRawModel);
    }
    
    void OnWizardCreate()
    {
        GenerateRoom(stringJson);
        ViewerWindow.UpdateSUNCGData(dataSUCNG,avaRoom[0]);
        end:;
    }

    //Debug Use
    /*public static int getMeshByUid(string uid)
    {

        Generate();
        var center = SUNCGSceneBuilder.NewRoomBuild(dataSUCNG,avaRoom[0],existWall,useRawModel);
        return 0;
    }
    public static void DebugLog(string text)
    {
        if (DebugText!=null)
        {
            DebugText.text = text;
        }
        else
        {
            Debug.Log(text);
        }
    }*/
    
    // public static Texture2D GetTexrture2DFromPath(string imgPath)
    // {
    //     //读取文件
    //     FileStream fs = new FileStream(imgPath,FileMode.Open,FileAccess.Read);
    //     int byteLength = (int)fs.Length;
    //     byte[] imgBytes = new byte[byteLength];
    //     fs.Read(imgBytes,0,byteLength);
    //     fs.Close();
    //     fs.Dispose();
    //     //转化为Texture2D
    //     Image img = Image.FromStream(new MemoryStream(imgBytes));
    //     Texture2D t2d = new Texture2D(img.Width,img.Height);
    //     img.Dispose();
    //     t2d.LoadImage(imgBytes);
    //     t2d.Apply();
    //     return t2d;
    // }

    /*public void NewRoom(GameObject json)
    {
        Generate(json.GetComponent<InputField>().text);
        var center = SUNCGSceneBuilder.NewRoomBuild(dataSUCNG,avaRoom[0],existWall,useRawModel);
        //FindObjectOfType<FirstPersonController>().center = center;
    }*/

}
#endif