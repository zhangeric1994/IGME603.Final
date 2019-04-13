using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public abstract class TableDataEntry
{
    abstract public int GetIndex();
}

public abstract class TableDataPage : ScriptableObject
{
}

public abstract class TableDataPage<T> : TableDataPage, ISerializationCallbackReceiver where T : TableDataEntry
{
    
    [SerializeField] private T[] entries;


    private Dictionary<int, T> dict = new Dictionary<int, T>();


    /// <summary>
    /// Get the data at a specific index
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public T this[int index]
    {
        get
        {
            return dict[index];
        }
    }


    public int CountRow()
    {
        return dict.Count;
    }

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        if (entries != null)
        {
            dict.Clear();

            for (int i = 0; i < entries.Length; ++i)
            {
                T entry = entries[i];

                int index = entry.GetIndex();

                if (!dict.ContainsKey(index))
                    dict.Add(index, entry);
            }

#if UNITY_EDITOR
#else
            serializedAttributes = null;
#endif
        }
    }

    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
#if UNITY_EDITOR
        if (entries == null || entries.Length < dict.Count)
            entries = new T[dict.Count];
#else
        serializedAttributes = new Attribute[attributes.Count];
#endif

        int i = 0;
        foreach (KeyValuePair<int, T> pair in dict)
            entries[i++] = pair.Value;
    }

    ///// <summary>
    ///// Add a entry at a given index
    ///// </summary>
    ///// <param name="index"> The index where to add the given data entry </param>
    ///// <param name="entry"> The data entry to add </param>
    //internal void AddEntry(int index, T entry)
    //{
    //    dict.Add(index, entry);
    //}
}
