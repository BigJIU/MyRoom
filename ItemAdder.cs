#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SUNCGData;
using UnityEngine;
using UnityEditor;



[Serializable]
public class SubItemImporter: ScriptableWizard
{
    public string TargetName;
    public GameObject FatherObject;
    




}



[Serializable]
public class ItemImporter: ScriptableWizard
{
    public string TargetName;
    public bool raw = true;
    private Node TargetNode;
    private string lastFileName;
    void OnWizardCreate()
    {
        if( Application.isPlaying)
        {
            EditorUtility.DisplayDialog("Error", "Suggest Static Item Import", "OK");
            goto end;
        }

        string lastPath = EditorPrefs.GetString("Item_lastPath", "");
        lastFileName = EditorPrefs.GetString("Item_lastFile", "");
        Debug.Log(lastPath);
        Debug.Log(lastFileName);
        string expFolder = EditorUtility.SaveFolderPanel("Import Item (By Folder)", lastPath, lastFileName);
        Debug.Log(expFolder);
        if (expFolder.Length > 0)
        {
            var fi = new System.IO.FileInfo(expFolder);
            EditorPrefs.SetString("Item_lastFile", fi.Name);
            EditorPrefs.SetString("Item_lastPath", fi.Directory.FullName);
            lastFileName = fi.Name;
            AddItem();
        }
        end:;
    }

    void AddItem()
    {
        //SUNCGDataStructure SUNCGData = SUNCGSceneExporter.GenerateDataFromScene();
        //Update Config Path
        UpdatePath();
        AddItemToJSON();
        AddItemToScene();
    }

    void UpdatePath()
    {
        
    }
    void AddItemToJSON()
    {
        SUNCGDataStructure SUNCGData = SceneImporter.GenerateData<SUNCGDataStructure>(EditorPrefs.GetString("origin_JSON"));
        Node[] tmpArray = SUNCGData.levels[0].nodes;
        List<Node> tmpList = tmpArray.ToList();

        int id = SUNCGData.levels[0].nodes.Length + 1;
        TargetNode = new Node(lastFileName, $"0_{id}");
        
        Node Room = SceneImporter.GenerateSUNCGRoom(SUNCGData)[0];
        List<int> tmpNodeList = Room.nodeIndices.ToList();
        tmpNodeList.Add(id);
        Room.nodeIndices = tmpNodeList.ToArray();
        
        tmpList.Add(TargetNode);
        SUNCGData.levels[0].nodes = tmpList.ToArray();
        
        EditorPrefs.SetString("origin_JSON",JsonConvert.SerializeObject(SUNCGData));
    }

    void AddItemToScene()
    {
        SUNCGSceneBuilder.NewFurBuild(TargetNode,GameObject.FindGameObjectWithTag("Room").transform,raw);
    }

}


#endif
