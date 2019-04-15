using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : GUIWindow
{
    [Header("References")]
    [SerializeField] private InventoryWidget inventoryWidget;

    private Action closeCallback;
    private Player player;


    public override void OnOpen(params object[] args)
    {
        closeCallback = (Action)args[0];

        player = Player.GetPlayer(0);

        inventoryWidget.Initialize(player.inventory);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Start"))
            Close();

        for (int id = 1; id < 8; ++id)
            if (Input.GetKeyDown(id.ToString()))
                player.inventory.Add(id, 1);
    }

    private void OnDestroy()
    {
        closeCallback?.Invoke();
    }
}
