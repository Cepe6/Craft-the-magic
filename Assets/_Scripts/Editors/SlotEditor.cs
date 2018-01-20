using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Slot))]
public class SlotEditor : Editor
{
    private Slot _slot;
    
    public override void OnInspectorGUI()
    {

        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_inventorySlot"), true);


        _slot = (Slot)target;
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_filtered"), true);
        serializedObject.ApplyModifiedProperties();


        serializedObject.Update();
        if (_slot.IsFiltered())
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_allowedItems"), true);
        }
        serializedObject.ApplyModifiedProperties();
    }
}