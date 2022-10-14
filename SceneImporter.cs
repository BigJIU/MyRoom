
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
    // Start is called before the first frame update
    //public static FrontDataStructure data;
    
    private SUNCGDataStructure dataSUCNG;
    private List<Node> avaRoom;
    public  Texture2D floorTexture;
    public  Texture2D wallTexture;
    public  string houseId = "76";
    public string stringJson;
    public  bool existWall = true;
    public  bool useRawModel = true;
    
    public Text DebugText;
    
    void Generate(string j = null)
    {
        
        if (!string.IsNullOrEmpty(j))
        {
            stringJson = j;
        }

        
        string path = $@"E:\3DFront\json\{houseId}.json";
        if (string.IsNullOrEmpty(stringJson))
        {
            dataSUCNG = JsonConvert.DeserializeObject<SUNCGDataStructure>(File.ReadAllText(path));
        }
        else
        {
            dataSUCNG = JsonConvert.DeserializeObject<SUNCGDataStructure>(stringJson);
        }


        avaRoom = new List<Node>();
        Debug.Log("Room JID:"+dataSUCNG.levels[0].nodes[0].modelId);
        foreach (Node node in dataSUCNG.levels[0].nodes)
        {
            if (node.roomTypes != null&&node.nodeIndices.Length>0)
            {
                avaRoom.Add(node);
                break;
            }
        }

        Config.floorTexture = floorTexture;
        Config.wallTexture = wallTexture;
    }

    public void NewRoom(GameObject json)
    {
        Generate(json.GetComponent<InputField>().text);
        var center = SUNCGSceneBuilder.NewRoomBuild(dataSUCNG,avaRoom[0],existWall,useRawModel);
        //FindObjectOfType<FirstPersonController>().center = center;
    }
    
    [MenuItem("RoomViewer/Generate")]
    void GenerateRoom(string input = null)
    {
        Debug.Log("wow");
        Generate(input);
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



}
