using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotWidget : GUIWidget
{
    [Header("References")]
    [SerializeField] private Image iconImage;
    [SerializeField] private Text numText;


    public override void Refresh(params object[] args)
    {
        InventorySlot data = (InventorySlot)args[0];

        iconImage.sprite = data.itemData.Icon;
        numText.text = data.num.ToString();
    }
}
