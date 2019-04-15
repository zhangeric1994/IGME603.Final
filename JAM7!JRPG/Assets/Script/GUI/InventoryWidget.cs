using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(GUIGrid))]
public class InventoryWidget : GUIAutoList<InventorySlotWidget, InventorySlot>
{
    private Inventory inventory;

    public override void Initialize(params object[] args)
    {
        inventory = (Inventory)args[0];

        foreach (KeyValuePair<int, int> pair in (IEnumerable<KeyValuePair<int, int>>)inventory)
            AddListItem(new InventorySlot(DataTableManager.singleton.GetItemData(pair.Key), pair.Value));

        GetComponent<GUIGrid>().Refresh();

        inventory.OnItemNumChange.AddListener(UpdateItem);
    }

    protected override void UpdateListItem(InventorySlotWidget listItem, InventorySlot listItemData)
    {
        listItem.Refresh(listItemData);
    }

    private void UpdateItem(ItemData itemData, int currentNum, int change)
    {
        if (currentNum == 0)
        {
            if (RemoveListItem(new InventorySlot(itemData, 0)))
                GetComponent<GUIGrid>().Refresh();
        }
        else if (currentNum == change)
        {
            if (AddListItem(new InventorySlot(itemData, currentNum)))
                GetComponent<GUIGrid>().Refresh();
        }
        else
            UpdateListItem(new InventorySlot(itemData, currentNum));
    }

    private void OnDestroy()
    {
        if (inventory != null)
            inventory.OnItemNumChange.RemoveListener(UpdateItem);
    }
}
