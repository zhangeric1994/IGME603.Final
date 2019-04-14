using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(AttributeSet))]
public class AttributeSetDrawer : PropertyDrawer
{
    private const float buttonWidth = 20;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        Rect contentPosition = EditorGUI.PrefixLabel(position, label);

        position.width = contentPosition.x - position.x;
        EditorGUI.PropertyField(position, property, GUIContent.none, false);

        SerializedProperty list = property.FindPropertyRelative("serializedAttributes");

        int indentLevel = EditorGUI.indentLevel;

        EditorGUI.indentLevel = 0;

        if (property.isExpanded)
        {
            int N = list.arraySize;

            float x = contentPosition.x;
            float width = contentPosition.width;
            float height = contentPosition.height / (N + 1);

            contentPosition.height = height;

            contentPosition.width = AttributeDrawer.typeWidth;
            EditorGUI.PropertyField(contentPosition, list.FindPropertyRelative("Array.size"), GUIContent.none);

            contentPosition.x += AttributeDrawer.typeWidth;
            contentPosition.width = buttonWidth;
            if (GUI.Button(contentPosition, "+"))
                list.InsertArrayElementAtIndex(N);

            contentPosition.x += buttonWidth;
            if (GUI.Button(contentPosition, "-"))
                list.DeleteArrayElementAtIndex(N - 1);

            contentPosition.x = x;
            contentPosition.width = width;
            for (int i = 0; i < list.arraySize; i++)
            {
                contentPosition.y += height;
                EditorGUI.PropertyField(contentPosition, list.GetArrayElementAtIndex(i));
            }
        }
        else
            EditorGUI.LabelField(contentPosition, list.arraySize.ToString());

        EditorGUI.indentLevel = indentLevel;
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float height = EditorGUI.GetPropertyHeight(property.FindPropertyRelative("serializedAttributes"), label);
        return property.isExpanded ? (1 + property.FindPropertyRelative("serializedAttributes").arraySize) * height : height; 
    }
}
