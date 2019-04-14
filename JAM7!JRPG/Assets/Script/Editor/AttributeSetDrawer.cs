using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(AttributeSet))]
public class AttributeSetDrawer : PropertyDrawer
{
    private const float buttonWidth = 20;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.PropertyField(position, property, GUIContent.none, false);

        SerializedProperty list = property.FindPropertyRelative("serializedAttributes");

        Rect contentPosition = EditorGUI.PrefixLabel(position, label);
        EditorGUI.indentLevel = 0;

        if (property.isExpanded)
        {
            float x = contentPosition.x;
            float width = contentPosition.width;
            float height = contentPosition.height;

            contentPosition.width = AttributeDrawer.idWidth;
            EditorGUI.PropertyField(contentPosition, list.FindPropertyRelative("Array.size"), GUIContent.none);

            contentPosition.x += AttributeDrawer.idWidth;
            contentPosition.width = buttonWidth;
            if (GUI.Button(contentPosition, "+"))
                list.InsertArrayElementAtIndex(list.arraySize);

            contentPosition.x += buttonWidth;
            if (GUI.Button(contentPosition, "-"))
                list.DeleteArrayElementAtIndex(list.arraySize - 1);

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
    }
}
