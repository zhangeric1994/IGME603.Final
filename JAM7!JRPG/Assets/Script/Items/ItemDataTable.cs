public class ItemDataTable : DataTable<ItemData>
{
#if UNITY_EDITOR
    [UnityEditor.MenuItem("Assets/Create/DataPage/Item")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<ItemDataTable>();
    }
#endif
}
