using System.Collections;
using System.Collections.Generic;

public class AttributeSet : IAttributeCollection
{
    private Dictionary<int, float> attributes;

    public float this[int id]
    {
        get
        {
            return attributes.ContainsKey(id) ? attributes[id] : 0;
        }
    }

    public float this[AttributeType attribute]
    {
        get
        {
            return this[(int)attribute];
        }
    }

    public AttributeSet()
    {
        attributes = new Dictionary<int, float>();
    }

    public AttributeSet(params object[] args) : this()
    {
        for (int i = 0; i < args.Length; i += 2)
            Set((AttributeType)args[i], (float)args[i + 1]);
    }

    private AttributeSet(string s) : this()
    {
        foreach (string field in s.Split(';'))
        {
            string[] values = field.Split(':');

            Set(int.Parse(values[0]), float.Parse(values[1]));
        }
    }

    public static AttributeSet Parse(string s)
    {
        return new AttributeSet(s);
    }

    public static AttributeSet Sum(params IAttributeCollection[] attributeSets)
    {
        AttributeSet attributeSet = new AttributeSet();

        foreach (IAttributeCollection attributes in attributeSets)
            foreach (KeyValuePair<int, float> attribute in attributes)
                attributeSet.Add(attribute.Key, attribute.Value);

        return attributeSet;
    }

    public static float Sum(AttributeType attribute, params IAttributeCollection[] attributeSets)
    {
        float sum = 0;

        foreach (IAttributeCollection attributeSet in attributeSets)
            sum += attributeSet[attribute];

        return sum;
    }

    public float Add(int id, float value)
    {
        if (!attributes.ContainsKey(id))
            attributes.Add(id, value);
        else
            attributes[id] += value;

        return attributes[id];
    }

    public void Set(int id, float value)
    {
        if (!attributes.ContainsKey(id))
            attributes.Add(id, value);
        else
            attributes[id] = value;
    }

    public void Set(AttributeType attribute, float value)
    {
        Set((int)attribute, value);
    }

    public IEnumerator<KeyValuePair<int, float>> GetEnumerator()
    {
        return attributes.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return attributes.GetEnumerator();
    }

    public override string ToString()
    {
        string s = "";

        foreach (KeyValuePair<int, float> attribute in attributes)
            s += ";" + (AttributeType)attribute.Key + ":" + attribute.Value;

        return s.Substring(1);
    }
}
