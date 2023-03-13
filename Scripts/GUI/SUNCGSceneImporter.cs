#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using SUNCGData;
using UnityEngine;

using UnityEditor;

[Serializable]
public class SUNCGSceneImporter: SceneImporter
{

    /*
    public  Texture2D floorTexture;
    public  Texture2D wallTexture;
    public  string houseId;
    public  bool existWall = true;
    public  bool existFloor = true;
    public  bool useRawModel = true;
    public GameObject TargetFloor = null;
    */
    


    public void BuildRoom()
    {
        Config.floorTexture = floorTexture;
        Config.wallTexture = wallTexture;
        ViewerWindow.ClearRoom();
        SUNCGDataStructure ds = ImportRoom<SUNCGDataStructure>();
        Node avaRoom = SelectRoom(ds);
        SUNCGSceneBuilder.NewRoomBuild(ds,avaRoom,existWall,existFloor,useRawModel,TargetFloor);
        
    }
    
    public static Node SelectRoom (SUNCGDataStructure ds) 
    {
        // Get avaRoom
        Debug.Log("Importing SUNCG Data");
        List<Node> tmpRoom = new List<Node>();
        Debug.Log("Room JID:"+ds.levels[0].nodes[0].modelId);
        foreach (Node node in ds.levels[0].nodes)
        {
            if (node.roomTypes != null&&node.nodeIndices.Length>0)
            {
                tmpRoom.Add(node);
                break;
            }
        }
        return tmpRoom[0];
    }
    void OnWizardCreate()
    {
        BuildRoom();
        //BuildRooms();
    }

    

}
#endif