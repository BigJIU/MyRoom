#if UNITY_EDITOR
using SUNCGData;
using UnityEditor;
using System.Reflection;
using UnityEditor.EditorTools;
using UnityEditor.IMGUI.Controls;
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
            Debug.Log("Clear, But no room exist");
        }
    }
    
    public void OnGUI ()
    {
        
        if (GUILayout.Button("Generate SUNCG"))
        {
            //GenerateRoom(this.mText);
            ScriptableWizard.DisplayWizard("Generate Room", typeof(SUNCGSceneImporter), "GenerateRoom");
        }
        if (GUILayout.Button("Generate 3DFront"))
        {
            //GenerateRoom(this.mText);
            ScriptableWizard.DisplayWizard("Generate Room", typeof(FrontSceneImporter), "GenerateRoom");
        }
        if (GUILayout.Button("Clear"))
        {
            ClearRoom();
        }
        if (GUILayout.Button("Export JSON"))
        {
            ;
            SUNCGSceneExporter.ExportJsonFromData(SUNCGSceneExporter.GenerateDataFromScene());
        }
        if (GUILayout.Button("Export OBJ"))
        {
            ScriptableWizard.DisplayWizard("Export OBJ", typeof(OBJExporter), "Export");
        }
        if (GUILayout.Button("Add Item"))
        {
            ScriptableWizard.DisplayWizard("Import Item", typeof(SubItemImporter), "Import");
        }
        if (GUILayout.Button("Add Furniture"))
        {
            ScriptableWizard.DisplayWizard("Import Furniture", typeof(ItemImporter), "Import");
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
    
    /*public override void OnToolGUI (EditorWindow window) {
        foreach (var obj in targets) {
            if (!(obj is T castShape) || Mathf.Approximately(castShape.transform.lossyScale.sqrMagnitude, 0f))
                continue;
 
            // collider matrix is center multiplied by transform's matrix with custom postmultiplied lossy scale matrix
            using (new Handles.DrawingScope(Matrix4x4.TRS(castShape.transform.position, castShape.transform.rotation, Vector3.one))) {
                CopyColliderPropertiesToHandle(castShape);
 
                boundsHandle.SetColor(castShape.enabled ? m_handleEnableColor : m_handleDisableColor);
 
                EditorGUI.BeginChangeCheck();
 
                boundsHandle.DrawHandle();
 
                if (EditorGUI.EndChangeCheck()) {
                    Undo.RecordObject(castShape, string.Format("Modify {0}", ObjectNames.NicifyVariableName(target.GetType().Name)));
                    CopyHandlePropertiesToCollider(castShape);
                }
            }
        }
    }*/
    
    public class PublicConfig : ScriptableWizard
    {
        public string ModelPath = Config.ModelPath;
        public string HomePath = Config.HomePath;

        public string sourcePath = Config.sourcePath;

        public string logPath = Config.logPath;
    
        void OnWizardCreate()
        {
            changeConfig();
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
[EditorTool("Move Right")]
class PlatformTool : EditorTool
{
    
    public override void OnToolGUI(EditorWindow window)
    {
        window.ShowNotification(new GUIContent("Move Right"));
 
        EditorGUI.BeginChangeCheck();
 
        Vector3 position = Tools.handlePosition;
 
        using (new Handles.DrawingScope(Color.green))
        {
            position = Handles.Slider(position, Vector3.right);
        }
 
        //如果在调用 EditorGUI.BeginChangeCheck 后 GUI 状态发生更改，则返回 true，否则返回 false。
        if (EditorGUI.EndChangeCheck())
        {
            Vector3 delta = position - Tools.handlePosition;
 
            Undo.RecordObjects(Selection.transforms, "Move Right");
 
            foreach (var transform in Selection.transforms)
                transform.position += delta;
        }
    }
}

#endif
