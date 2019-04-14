using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(DataTableEntry), true)]
public class DataTableEntryDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.isExpanded)
        {
            int depth = property.depth;

            EditorGUI.indentLevel = 0;


            foreach (SerializedProperty col in property)
                if (col.depth == depth + 1)
                {
                    EditorGUI.PropertyField(position, col);
                    position.y += position.height;
                }
        }
    }
}
