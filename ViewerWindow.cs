#if UNITY_EDITOR
using SUNCGData;
using UnityEditor;
using UnityEngine;

public class ViewerWindow : EditorWindow
{

    
    public string mText = "please input a string";
    public bool mWall;
    [MenuItem("RoomViewer/Window")]
    public static void ShowWindow()
    {
        EditorWindow window = EditorWindow.GetWindow(typeof(ViewerWindow));
        window.Show();
    }

    [MenuItem("RoomViewer/Config")]
    public static void GenerateRoom()
    {
        ScriptableWizard.DisplayWizard("Config Path", typeof(PublicConfig), "Confirm");
    }
    [MenuItem("RoomViewer/Output Obj")]
    static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard("Export OBJ", typeof(OBJExporter), "Export");
    }
            
    [MenuItem("RoomViewer/Clear")]
    public static void ClearRoom()
    {
        GameObject tmpGameObject = GameObject.FindWithTag("Room");
        if (tmpGameObject!=null)
        {
            MonoBehaviour.DestroyImmediate(tmpGameObject);
        }
        else
        {
            Debug.Log("No room exist");
        }
    }
    
    public void OnGUI ()
    {
        if (GUILayout.Button("Generate"))
        {
            //GenerateRoom(this.mText);
            ScriptableWizard.DisplayWizard("Export OBJ", typeof(SceneImporter), "GenerateRoom");
        }
        if (GUILayout.Button("Clear"))
        {
            ClearRoom();
        }
        if (GUILayout.Button("Export JSON"))
        {
            SUNCGSceneExporter.ExportJSON(SUNCGdata,SUNCGAvaRoom);
        }
        if (GUILayout.Button("Export OBJ"))
        {
            ScriptableWizard.DisplayWizard("Export OBJ", typeof(OBJExporter), "Export");
        }
        if (GUILayout.Button("Add SubItem"))
        {
            ScriptableWizard.DisplayWizard("Import SubItem", typeof(SubItemImporter), "Import");
        }
        
        /*//文本标签
        EditorGUILayout.LabelField("RoomId");
        string a = "RoomID";
        a= EditorGUILayout.TextField(a);
        //空一行
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("RoomJson");
        this.mText = EditorGUILayout.TextArea(this.mText);
        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("WallExist");
        this.mWall = EditorGUILayout.Toggle(this.mWall);*/
        
    }
    
    
    //Logic Related
    private static SUNCGDataStructure SUNCGdata;
    private static Node SUNCGAvaRoom;

    public static void UpdateSUNCGData(SUNCGDataStructure s,Node avaRoom)
    {
        SUNCGdata = s;
        SUNCGAvaRoom = avaRoom;
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
    
}
#endif
