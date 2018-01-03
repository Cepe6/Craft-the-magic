using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(Item))]
public class ItemEditor : Editor {
    Item _script;
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.BeginVertical();
        _script = (Item)target;

        EditorGUILayout.PropertyField(serializedObject.FindProperty("_type"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_itemIcon"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_isStackable"), true);
        EditorGUILayout.EndVertical();
        serializedObject.ApplyModifiedProperties();

        AssetDatabase.SaveAssets();
    }
}
