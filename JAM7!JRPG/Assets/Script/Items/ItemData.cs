using System;
using UnityEngine;

[Serializable] public class ItemData : TableDataEntry
{
    [SerializeField] public int id;
    [SerializeField] public string name;
    [SerializeField] public Sprite icon;
    [SerializeField] public Sprite sprite;
    [SerializeField] public AttributeSet attributes;

    //public int Id
    //{
    //    get
    //    {
    //        return id;
    //    }
    //}

    //public Sprite Icon
    //{
    //    get
    //    {
    //        return icon;
    //    }
    //}

    //public AttributeSet Attributes { get; internal set; }

    public override int GetIndex()
    {
        return id;
    }
}
