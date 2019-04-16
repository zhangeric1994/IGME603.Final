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
    [SerializeField] private T[] serializedEntries;


    private Dictionary<int, T> entries = new Dictionary<int, T>();


    /// <summary>
    /// Get the data at a specific index
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public T this[int index]
    {
        get
        {
            return entries[index];
        }
    }

    public override int Count
    {
        get
        {
            return entries.Count;
        }
    }


    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        if (serializedEntries != null)
        {
            entries.Clear();

            for (int i = 0; i < serializedEntries.Length; ++i)
            {
                T entry = serializedEntries[i];

                int index = entry.Index;

                if (!entries.ContainsKey(index))
                    entries.Add(index, entry);
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
        if (serializedEntries == null || serializedEntries.Length < entries.Count)
            serializedEntries = new T[entries.Count];
#else
        serializedEntries = new T[entries.Count];
#endif

        int i = 0;
        foreach (KeyValuePair<int, T> pair in entries)
            serializedEntries[i++] = pair.Value;
    }
}
