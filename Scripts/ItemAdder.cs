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
    private SubNode TargetNode;
    private string lastFileName;

    void OnWizardCreate()
    {
        if( Application.isPlaying)
        {
            EditorUtility.DisplayDialog("Error", "Suggest Static Item Import", "OK");
            goto end;
        }

        string lastPath = EditorPrefs.GetString("SubItem_lastPath", "");
        lastFileName = EditorPrefs.GetString("SubItem_lastPath", "");
        Debug.Log(lastPath);
        Debug.Log(lastFileName);
        string expFolder = EditorUtility.SaveFolderPanel("Import Item (By Folder)", lastPath, lastFileName);
        Debug.Log(expFolder);
        if (expFolder.Length > 0)
        {
            var fi = new System.IO.FileInfo(expFolder);
            EditorPrefs.SetString("SubItem_lastPath", fi.Name);
            EditorPrefs.SetString("SubItem_lastPath", fi.Directory.FullName);
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

    //TODO: Verify the Existence in the Path
    void UpdatePath()
    {
        
    }
    void AddItemToJSON()
    {
        SUNCGDataStructure SUNCGData = SUNCGSceneImporter.GenerateData<SUNCGDataStructure>(EditorPrefs.GetString("origin_JSON"));
        Node fatherNode = SUNCGData.levels[0].nodes.ToList().Find((node => node.id == FatherObject.name));
        if (fatherNode.subItems is null) fatherNode.subItems = new SubNode[0];
        
        int id = fatherNode.subItems.Length + 1;
        TargetNode = new SubNode(lastFileName, $"0_{id}_{lastFileName}");
        
        SubNode[] tmpArray = fatherNode.subItems;
        List<SubNode> tmpList = tmpArray.ToList();
        tmpList.Add(TargetNode);
        fatherNode.subItems = tmpList.ToArray();
        
        EditorPrefs.SetString("origin_JSON",JsonConvert.SerializeObject(SUNCGData));
    }

    void AddItemToScene()
    {
        SUNCGSceneBuilder.NewSubItemBuild(TargetNode,FatherObject.transform);
    }



}



[Serializable]
public class ItemImporter: ScriptableWizard
{
    public string TargetName;
    public bool raw = true;
    public GameObject TargetFile;
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

    //TODO: Verify the Existence in the Path
    void UpdatePath()
    {
        
    }
    void AddItemToJSON()
    {
        SUNCGDataStructure SUNCGData = SUNCGSceneImporter.GenerateData<SUNCGDataStructure>(EditorPrefs.GetString("origin_JSON"));
        Node[] tmpArray = SUNCGData.levels[0].nodes;
        List<Node> tmpList = tmpArray.ToList();

        int id = SUNCGData.levels[0].nodes.Length + 1;
        TargetNode = new Node(lastFileName, $"0_{id}");
        
        //Node Room = SUNCGSceneImporter.GenerateSUNCGRoom(SUNCGData)[0];
        //FIXME: May be Bug
        Node Room = SUNCGSceneImporter.SelectRoom(SUNCGData);
            
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
