using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public struct InventorySlot
{
    public readonly ItemData itemData;
    public readonly int num;

    public InventorySlot(ItemData itemData, int num)
    {
        this.itemData = itemData;
        this.num = num;
    }

    public override int GetHashCode()
    {
        return itemData.GetHashCode();
    }
}


public class EventOnItemNumChange : UnityEvent<ItemData, int, int> { }


public class Inventory : IAttributeCollection, IEnumerable<KeyValuePair<int, int>>
{
    private Dictionary<int, int> items = new Dictionary<int, int>();
    private AttributeSet attributes = new AttributeSet();


    public EventOnItemNumChange OnItemNumChange { get; } = new EventOnItemNumChange();
    public EventOnAttributeChange OnAttributeChange { get; } = new EventOnAttributeChange();

    public float this[int id]
    {
        get
        {
            return items.ContainsKey(id) ? items[id] : 0;
        }
    }

    public float this[AttributeType type]
    {
        get
        {
            return attributes[type];
        }
    }


    public bool Add(int id, int num = 1)
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

            OnItemNumChange.Invoke(itemData, items[id], num);

            return true;
        }

        return false;
    }

    public bool Remove(int id, int num = 1)
    {
        ItemData itemData = DataTableManager.singleton.GetItemData(id);

        if (itemData == null)
            Debug.LogError(LogUtility.MakeLogStringFormat("Inventory", "Try to remove non-existing item (id = {0}).", id));
        else if (items.ContainsKey(id))
        {
            if (items[id] >= num)
            {
                items[id] -= num;

                attributes.Decrement(itemData.Attributes, OnAttributeChange, num);

                OnItemNumChange.Invoke(itemData, items[id], -num);

                return true;
            }
            else if (items[id] == num)
            {
                items.Remove(id);

                attributes.Decrement(itemData.Attributes, OnAttributeChange, num);

                OnItemNumChange.Invoke(itemData, 0, -num);

                return true;
            }
        }

        return false;
    }

    IEnumerator<KeyValuePair<int, int>> IEnumerable<KeyValuePair<int, int>>.GetEnumerator()
    {
        return ((IEnumerable<KeyValuePair<int, int>>)items).GetEnumerator();
    }

    public IEnumerator<KeyValuePair<AttributeType, float>> GetEnumerator()
    {
        return ((IAttributeCollection)attributes).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IAttributeCollection)attributes).GetEnumerator();
    }

    //public IEnumerator<KeyValuePair<AttributeType, float>> GetEnumerator()
    //{
    //    return ((IAttributeCollection)attributes).GetEnumerator();
    //}

    //IEnumerator IEnumerable.GetEnumerator()
    //{
    //    return ((IAttributeCollection)attributes).GetEnumerator();
    //}
}
