#if UNITY_EDITOR
using System.Collections;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

[CanEditMultipleObjects]
public class CastBoxEditor : Editor {
 
    private SerializedProperty m_script;
    private SerializedProperty m_center;
    private SerializedProperty m_size;
 
    private void OnEnable () {
        m_script = serializedObject.FindProperty("m_Script");
        m_center = serializedObject.FindProperty("center");
        m_size = serializedObject.FindProperty("size");
    }
 
    public override void OnInspectorGUI () {
        serializedObject.Update();
 
        GUI.enabled = false;
        EditorGUILayout.PropertyField(m_script);
        GUI.enabled = true;
        EditorGUILayout.EditorToolbarForTarget(EditorGUIUtility.TrTempContent("Edit Shape"), target);
        EditorGUILayout.PropertyField(m_center);
        EditorGUILayout.PropertyField(m_size);
 
        serializedObject.ApplyModifiedProperties();
    }
}
#endif