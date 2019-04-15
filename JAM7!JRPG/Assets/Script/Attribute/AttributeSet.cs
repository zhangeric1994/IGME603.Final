using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable] public struct Attribute
{
    public AttributeType type;
    public float value;

    public Attribute(KeyValuePair<AttributeType, float> pair)
    {
        type = pair.Key;
        value = pair.Value;
    }
}

public class EventOnAttributeChange : UnityEvent<AttributeType, float, float> { }

[Serializable] public class AttributeSet : IAttributeCollection, ISerializationCallbackReceiver
{
    [SerializeField] Attribute[] serializedAttributes;

    private Dictionary<AttributeType, float> attributes = new Dictionary<AttributeType, float>();

    public EventOnAttributeChange OnAttributeChange
    {
        get
        {
            return null;
        }
    }

    //public float this[int id]
    //{
    //    get
    //    {
    //        return attributes.ContainsKey(id) ? attributes[id] : 0;
    //    }
    //}

    public float this[AttributeType type]
    {
        get
        {
            return attributes.ContainsKey(type) ? attributes[type] : 0;
        }
    }

    public AttributeSet()
    {
        serializedAttributes = null;
    }

    public AttributeSet(params object[] args) : this()
    {
        for (int i = 0; i < args.Length; i += 2)
            Set((AttributeType)args[i], (float)args[i + 1]);
    }

    //private AttributeSet(string s) : this()
    //{
    //    foreach (string field in s.Split(';'))
    //    {
    //        string[] values = field.Split(':');

    //        Set(int.Parse(values[0]), float.Parse(values[1]));
    //    }
    //}

    public static AttributeSet Parse(string s)
    {
        return new AttributeSet(s);
    }

    public static AttributeSet Sum(params IAttributeCollection[] attributeSets)
    {
        AttributeSet attributeSet = new AttributeSet();

        foreach (IAttributeCollection attributes in attributeSets)
            foreach (KeyValuePair<AttributeType, float> attribute in attributes)
                attributeSet.Add(attribute.Key, attribute.Value);

        return attributeSet;
    }

    public static float Sum(AttributeType type, params IAttributeCollection[] attributeSets)
    {
        float sum = 0;

        foreach (IAttributeCollection attributeSet in attributeSets)
            sum += attributeSet[type];

        return sum;
    }

    //public float Add(int id, float value)
    //{
    //    if (!attributes.ContainsKey(id))
    //        attributes.Add(id, value);
    //    else
    //        attributes[id] += value;

    //    return attributes[id];
    //}

    public float Add(AttributeType type, float value)
    {
        if (!attributes.ContainsKey(type))
            attributes.Add(type, value);
        else
            attributes[type] += value;

        return attributes[type];
    }

    //public void Set(int id, float value)
    //{
    //    if (!attributes.ContainsKey(id))
    //        attributes.Add(id, value);
    //    else
    //        attributes[id] = value;
    //}

    public void Set(AttributeType type, float value)
    {
        if (!attributes.ContainsKey(type))
            attributes.Add(type, value);
        else
            attributes[type] = value;
    }

    public void Set(AttributeType type, float value, EventOnAttributeChange onAttributeChange)
    {
        if (!attributes.ContainsKey(type))
        {
            if (value != 0)
            {
                attributes.Add(type, value);

                onAttributeChange.Invoke(type, 0, value);
            }
        }
        else
        {
            float previousValue = attributes[type];

            if (value != previousValue)
            {
                attributes[type] = value;

                onAttributeChange.Invoke(type, previousValue, value);
            }
        }
    }

    public void Modify(AttributeType type, float value, int t = 1)
    {
        if (!attributes.ContainsKey(type))
            attributes.Add(type, value * t);
        else
            attributes[type] += value * t;
    }

    public void Modify(AttributeType type, float value, EventOnAttributeChange onAttributeChange, int t = 1)
    {
        float previousValue;

        if (!attributes.ContainsKey(type))
        {
            previousValue = 0;
            attributes.Add(type, value * t);
        }
        else
        {
            previousValue = attributes[type];
            attributes[type] += value * t;
        }

        onAttributeChange.Invoke(type, previousValue, attributes[type]);
    }

    public void Increment(IAttributeCollection attributes, int t = 1)
    {
        foreach (KeyValuePair<AttributeType, float> attribute in attributes)
            Modify(attribute.Key, attribute.Value, t);
    }

    public void Increment(IAttributeCollection attributes, EventOnAttributeChange onAttributeChange, int t = 1)
    {
        foreach (KeyValuePair<AttributeType, float> attribute in attributes)
            Modify(attribute.Key, attribute.Value, onAttributeChange, t);
    }

    public void Decrement(IAttributeCollection attributes, int t = 1)
    {
        foreach (KeyValuePair<AttributeType, float> attribute in attributes)
            Modify(attribute.Key, -attribute.Value, t);
    }

    public void Decrement(IAttributeCollection attributes, EventOnAttributeChange onAttributeChange, int t = 1)
    {
        foreach (KeyValuePair<AttributeType, float> attribute in attributes)
            Modify(attribute.Key, -attribute.Value, onAttributeChange, t);
    }


    public IEnumerator<KeyValuePair<AttributeType, float>> GetEnumerator()
    {
        return attributes.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return attributes.GetEnumerator();
    }

    public void OnBeforeSerialize()
    {
#if UNITY_EDITOR
        if (serializedAttributes == null || serializedAttributes.Length < attributes.Count)
            serializedAttributes = new Attribute[attributes.Count];
#else
        serializedAttributes = new Attribute[attributes.Count];
#endif

        int i = 0;
        foreach (KeyValuePair<AttributeType, float> attribute in attributes)
            serializedAttributes[i++] = new Attribute(attribute);
    }

    public void OnAfterDeserialize()
    {
        if (serializedAttributes != null)
        {
            attributes.Clear();

            for (int i = 0; i < serializedAttributes.Length; ++i)
            {
                Attribute attribute = serializedAttributes[i];

                if (!attributes.ContainsKey(attribute.type))
                    attributes.Add(attribute.type, attribute.value);
            }

#if UNITY_EDITOR
#else
            serializedAttributes = null;
#endif
        }
    }

    public override string ToString()
    {
        string s = "";

        foreach (KeyValuePair<AttributeType, float> attribute in attributes)
            s += ";" + attribute.Key + ":" + attribute.Value;

        return s.Substring(1);
    }
}
