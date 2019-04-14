using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Attribute))]
public class AttributeDrawer : PropertyDrawer
{
    public const float idWidth = 50;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        float width = position.width;

        position.width = idWidth;
        EditorGUI.PropertyField(position, property.FindPropertyRelative("id"), GUIContent.none);

        EditorGUI.indentLevel = 0;
        position.x += position.width;
        position.width = width - idWidth;
        EditorGUI.PropertyField(position, property.FindPropertyRelative("value"), GUIContent.none);
    }
}
