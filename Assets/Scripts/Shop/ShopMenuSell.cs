using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopMenuSell : ShopMenu
{
    [SerializeField] Button firstButtonToSelect;
    public GameObject itemDescriptionGO;
    public InventorySlot[] inventorySlot;

    public override void DisplayMenu(bool on)

    {
        itemDescriptionGO.SetActive(false);
        LoadInventory();
        displayContainer.SetActive(on);
    }

    void LoadInventory()

    {
        DisableAllSlots();

        for (int i = 0; i < menuManagerUI.playerInventory.inventory.Count; i++)

        {
            Gear gearToLoad = menuManagerUI.playerInventory.inventory[i].GetComponent<Gear>();
            inventorySlot[i].gear = gearToLoad;
            inventorySlot[i].itemName.text = gearToLoad.gearID;
            inventorySlot[i].itemQuantity.text = " x " + gearToLoad.quantityInInventory;
            inventorySlot[i].gameObject.SetActive(true);
        }

        if (menuManagerUI.playerInventory.inventory.Count == 0)

        {
            inventorySlot[0].gameObject.SetActive(true);
            inventorySlot[0].itemName.text = "No items to sell";
            inventorySlot[0].itemQuantity.text = "";
        }
    }

    void DisableAllSlots()
    {
        for (int i = 0; i < inventorySlot.Length; i++)
        {
            inventorySlot[i].gameObject.SetActive(false);
        }
    }

    public override void EnterMenu()
    {
        if (menuManagerUI.playerInventory.inventory.Count != 0)

        {
            shopButtonHighlighted.SetButtonColor(shopButtonHighlighted.highlightedColor);
            shopButtonHighlighted.enabled = false;
            itemDescriptionGO.SetActive(true);
            firstButtonToSelect.Select();
        }

        if (menuManagerUI.playerInventory.inventory.Count == 0) 
        
        {
            return;
        }
    }

    public override void ExitMenu()

    {
        shopButtonHighlighted.enabled = true;
        shopButtonHighlighted.SetButtonColor(Color.white);
        mainButtonToRevert.Select();
        menuManagerUI.menuUpdateMethod = menuManagerUI.main;
    }

    public void SellGearInSlot(InventorySlot inventorySlot)

    { 
        Gear gearToSell = inventorySlot.gear;
        menuManagerUI.playerInventory.inventorySO.inventoryString.Remove(gearToSell.name);
        menuManagerUI.playerInventory.LoadInventoryFromSO();
        LoadInventory();
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitMenu();
        }
    }
}
