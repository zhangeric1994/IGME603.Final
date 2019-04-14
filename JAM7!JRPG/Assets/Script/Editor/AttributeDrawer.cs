using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Attribute))]
public class AttributeDrawer : PropertyDrawer
{
    public const float typeWidth = 150;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        float width = position.width;

        position.width = typeWidth;
        EditorGUI.PropertyField(position, property.FindPropertyRelative("type"), GUIContent.none);

        EditorGUI.indentLevel = 0;
        position.x += position.width;
        position.width = width - typeWidth;
        EditorGUI.PropertyField(position, property.FindPropertyRelative("value"), GUIContent.none);
    }
}
