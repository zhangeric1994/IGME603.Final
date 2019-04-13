using UnityEngine;

public class Item2 : ScriptableObject
{
    [SerializeField] private int id;
    [SerializeField] private Sprite icon;
    [SerializeField] private AttributeSet attributes;

    public int Id
    {
        get
        {
            return id;
        }
    }

    public Sprite Icon
    {
        get
        {
            return icon;
        }
    }

    public AttributeSet Attributes { get; internal set; }


#if UNITY_EDITOR
    [UnityEditor.MenuItem("Assets/Create/Item")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<Item2>();
    }
#endif
}
