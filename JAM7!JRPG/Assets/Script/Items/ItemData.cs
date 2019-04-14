using System;
using UnityEngine;

[Serializable] public class ItemData : DataTableEntry
{
    [SerializeField] private int id;
    [SerializeField] private string name;
    [SerializeField] private Sprite icon;
    [SerializeField] private Sprite sprite;
    [SerializeField] private AttributeSet attributes;


    public override int Index
    {
        get
        {
            return id;
        }
    }

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

    public AttributeSet Attributes
    {
        get
        {
            return attributes;
        }
    }
}
