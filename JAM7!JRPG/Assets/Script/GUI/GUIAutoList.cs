using System.Collections.Generic;
using UnityEngine;


public abstract class GUIAutoList<ListItemType, ListItemDataType> : GUIWidget where ListItemType : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private ListItemType prefab;
    [SerializeField] private int capacity = 0;

    private Stack<ListItemType> reusableListItems = new Stack<ListItemType>();
    protected Dictionary<ListItemDataType, ListItemType> listItems = new Dictionary<ListItemDataType, ListItemType>();
    protected List<ListItemType> listItemsByOrder = new List<ListItemType>();

    protected void Refresh(List<ListItemDataType> listItemDataList)
    {
        HashSet<ListItemDataType> removedListItemDataSet = new HashSet<ListItemDataType>();
        foreach (KeyValuePair<ListItemDataType, ListItemType> listItem in listItems)
            removedListItemDataSet.Add(listItem.Key);

        foreach (ListItemDataType listItemData in listItemDataList)
        {
            if (listItems.ContainsKey(listItemData))
                UpdateListItem(listItemData);
            else
                AddListItem(listItemData);
        }

        foreach (ListItemDataType removedListItemData in removedListItemDataSet)
            RemoveListItem(removedListItemData);
    }

    protected ListItemType AddListItem(ListItemDataType listItemData)
    {
        if (!listItems.ContainsKey(listItemData))
        {
            if (capacity == 0 || listItemsByOrder.Count < capacity)
            {
                int numExistedUIListItem = transform.childCount;

                ListItemType listItem = reusableListItems.Count == 0 ? listItem = Instantiate(prefab, transform) : reusableListItems.Pop();

                listItem.gameObject.SetActive(true);

                listItems.Add(listItemData, listItem);
                listItemsByOrder.Add(listItem);

                UpdateListItem(listItem, listItemData);

                return listItem;
            }
        }

        return null;
    }

    protected void UpdateListItem(ListItemDataType listItemData)
    {
        UpdateListItem(listItems[listItemData], listItemData);
    }

    protected virtual void UpdateListItem(ListItemType listItem, ListItemDataType listItemData) { }

    protected bool RemoveListItem(ListItemDataType listItemData)
    {
        if (listItems.ContainsKey(listItemData))
        {
            ListItemType listItem = listItems[listItemData];

            listItem.gameObject.SetActive(false);

            listItems.Remove(listItemData);
            listItemsByOrder.Remove(listItem);
            reusableListItems.Push(listItem);

            return true;
        }

        return false;
    }
}
