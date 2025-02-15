using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopMenuSell : ShopMenu
{
    [SerializeField] Button firstButtonToSelect;
    public PlayerInventory playerInventory;
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
        var player = GameObject.FindGameObjectWithTag("Player");
        playerInventory = player.GetComponentInChildren<PlayerInventory>();

        DisableAllSlots();

        for (int i = 0; i < playerInventory.inventory.Count; i++)

        {
            Gear gearToLoad = playerInventory.inventory[i].GetComponent<Gear>();
            inventorySlot[i].gear = gearToLoad;
            inventorySlot[i].itemName.text = gearToLoad.gearID;
            inventorySlot[i].itemQuantity.text = " x " + gearToLoad.quantityInInventory;
            inventorySlot[i].gameObject.SetActive(true);
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
        shopButtonHighlighted.SetButtonColor(shopButtonHighlighted.highlightedColor);
        shopButtonHighlighted.enabled = false;
        itemDescriptionGO.SetActive(true);
        firstButtonToSelect.Select();
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
        playerInventory.inventorySO.inventoryString.Remove(gearToSell.name);
        playerInventory.LoadInventoryFromSO();
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
