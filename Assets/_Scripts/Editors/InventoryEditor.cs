using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(StaticInventory), true)]
public class InventoryEditor : Editor {
    InventoryAbstract _script;
    private List<bool> foldouts = new List<bool>();
    SerializedProperty slotsProp;
    private List<SerializedObject> slotsSerialized = new List<SerializedObject> ();

    private void Awake()
    {
        _script = (InventoryAbstract)target;
        slotsProp = serializedObject.FindProperty("_slots");

        serializedObject.Update();
        for (int i = 0; i < _script.SlotsCount(); i++)
        {
            if (slotsSerialized.Count == i)
            {
                foldouts.Add(false);
                try
                {
                    slotsProp.InsertArrayElementAtIndex(i);
                    slotsProp.GetArrayElementAtIndex(i).objectReferenceValue = _script.transform.GetComponentsInChildren<Slot>()[i];
                    slotsSerialized.Add(new SerializedObject(slotsProp.GetArrayElementAtIndex(i).objectReferenceValue));
                }
                catch (IndexOutOfRangeException)
                {
                    throw new Exception("Not enough slots in current parent. Present: " + i + ", Needed: " + _script.SlotsCount());
                }
            }
            serializedObject.ApplyModifiedProperties();
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        for (int i = 0; i < _script.SlotsCount(); i++)
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