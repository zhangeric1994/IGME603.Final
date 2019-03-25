/**
 * @author SerapH
 */

using UnityEngine;

public struct ResourceUtility
{
    public static T GetPrefab<T>(string directory) where T : Object
    {
        return Resources.Load<T>("Prefabs/" + directory);
    }

    public static GameObject GetPrefab(string directory)
    {
        return GetPrefab<GameObject>(directory);
    }

    public static T GetGUIPrefab<T>(string name) where T : Object
    {
        return Resources.Load<T>("Prefabs/GUI/" + name);
    }
}
