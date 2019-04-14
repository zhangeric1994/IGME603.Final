using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DataTable), true, isFallback = true)]
public class DataTableEditor : Editor
{
    private SerializedProperty serializedEntries; 

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        //Debug.Log(serializedEntries.arrayElementType);
        //for (int i = 0; i < serializedEntries.arraySize; i++)
        //    EditorGUILayout.PropertyField(serializedEntries.GetArrayElementAtIndex(i));
    }

    private void OnEnable()
    {
        serializedEntries = serializedObject.FindProperty("serializedEntries");
    }
}
