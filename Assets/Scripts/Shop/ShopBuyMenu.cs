using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopBuyMenu : ShopMenu
{
    public InventorySlot inventorySlot1;


    public override void DisplayMenu(bool on)
    {
        InitializeInventory();
        displayContainer.SetActive(on);
    }

    public override void EnterMenu()
    {
        throw new System.NotImplementedException();
    }

    public override void ExitMenu()
    {
        throw new System.NotImplementedException();
    }

    public override void StateUpdate()
    {
        throw new System.NotImplementedException();
    }

    void InitializeInventory()
    {
        inventorySlot1.GetComponentInChildren<TextMeshProUGUI>().text = "smams";
    }
}
