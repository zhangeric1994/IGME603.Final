using UnityEditor;

[CustomEditor(typeof(Attribute))]
public class AttributeEditor : Editor
{
    SerializedProperty id;
    SerializedProperty value;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();


        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(id);
        EditorGUILayout.PropertyField(value);
        EditorGUILayout.EndHorizontal();


        serializedObject.ApplyModifiedProperties();
    }

    internal void OnEnable()
    {
        id = serializedObject.FindProperty("id");
        value = serializedObject.FindProperty("value");
    }
}
