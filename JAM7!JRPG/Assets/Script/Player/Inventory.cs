using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : IAttributeCollection
{
    private Dictionary<int, int> items = new Dictionary<int, int>();
    private AttributeSet attributes = new AttributeSet();


    public EventOnAttributeChange OnAttributeChange { get; } = new EventOnAttributeChange();

    public float this[AttributeType type]
    {
        get
        {
            return attributes[type];
        }
    }


    public void Add(int id, int num = 1)
    {
        ItemData itemData = DataTableManager.singleton.GetItemData(id);

        if (itemData == null)
            Debug.LogError(LogUtility.MakeLogStringFormat("Inventory", "Try to add non-existing item (id = {0}).", id));
        else
        {
            if (items.ContainsKey(id))
                items[id] += num;
            else
                items.Add(id, num);

            attributes.Increment(itemData.Attributes, OnAttributeChange, num);
        }
    }

    public bool Remove(int id, int num = 1)
    {
        ItemData itemData = DataTableManager.singleton.GetItemData(id);

        if (itemData == null)
            Debug.LogError(LogUtility.MakeLogStringFormat("Inventory", "Try to remove non-existing item (id = {0}).", id));
        else if (items.ContainsKey(id) && items[id] >= num)
        {
            items[id] -= num;

            attributes.Decrement(itemData.Attributes, OnAttributeChange, num);

            return true;
        }

        return false;
    }


    public IEnumerator<KeyValuePair<AttributeType, float>> GetEnumerator()
    {
        return attributes.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return attributes.GetEnumerator();
    }
}
