using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public abstract class DataTableEntry
{
    abstract public int Index { get; }
}

public abstract class DataTable : ScriptableObject
{
    abstract public int Count { get; }
}

public abstract class DataTable<T> : DataTable, ISerializationCallbackReceiver where T : DataTableEntry
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

    public override int Count
    {
        get
        {
            return dict.Count;
        }
    }


    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        if (entries != null)
        {
            dict.Clear();

            for (int i = 0; i < entries.Length; ++i)
            {
                T entry = entries[i];

                int index = entry.Index;

                if (!dict.ContainsKey(index))
                    dict.Add(index, entry);
            }

#if UNITY_EDITOR
#else
            entries = null;
#endif
        }
    }

    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
#if UNITY_EDITOR
        if (entries == null || entries.Length < dict.Count)
            entries = new T[dict.Count];
#else
        entries = new Attribute[attributes.Count];
#endif

        int i = 0;
        foreach (KeyValuePair<int, T> pair in dict)
            entries[i++] = pair.Value;
    }
}
