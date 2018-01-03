using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InventoryController))]
public class InventoryEditor : Editor {
    InventoryController _script;
    private bool[] foldouts = new bool[InventoryController.SlotsCount()];
    SerializedProperty slotsProp;
    static SerializedObject[] slotsSerialized = new SerializedObject[InventoryController.SlotsCount()];
    
    private void Awake()
    {
        _script = (InventoryController)target;
        slotsProp = serializedObject.FindProperty("_slots");

        serializedObject.Update();
        for (int i = 0; i < InventoryController.SlotsCount(); i++)
        {
            if (slotsSerialized[i] != null)
            {
                continue;
            }

            slotsProp.InsertArrayElementAtIndex(i);
            slotsProp.GetArrayElementAtIndex(i).objectReferenceValue = _script.transform.Find("ItemSlots").GetChild(i).GetComponent<Slot>();
            slotsSerialized[i] = new SerializedObject(slotsProp.GetArrayElementAtIndex(i).objectReferenceValue);
        }
        serializedObject.ApplyModifiedProperties();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        for (int i = 0; i < InventoryController.SlotsCount(); i++)
        {
            slotsSerialized[i].Update();
            string labelText = slotsSerialized[i].FindProperty("_item").objectReferenceValue != null ?
                ("Slot " 
                + i 
                + " - Current(Item: " 
                + new SerializedObject(slotsSerialized[i].FindProperty("_item").objectReferenceValue).FindProperty("_type").enumNames[new SerializedObject(slotsSerialized[i].FindProperty("_item").objectReferenceValue).FindProperty("_type").enumValueIndex] 
                + ", Ammount: " 
                + slotsSerialized[i].FindProperty("_ammount").intValue
                + ")")
                : "Slot " + i;
            foldouts[i] = EditorGUILayout.Foldout(foldouts[i], labelText);
            if (foldouts[i])
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(slotsSerialized[i].FindProperty("_item"), true);
                EditorGUILayout.PropertyField(slotsSerialized[i].FindProperty("_ammount"), true);
                EditorGUI.indentLevel--;
            }
            slotsSerialized[i].ApplyModifiedProperties();

            EditorUtility.SetDirty(slotsSerialized[i].targetObject);
        }
        serializedObject.ApplyModifiedProperties();

        EditorUtility.SetDirty(target);
    }
}
