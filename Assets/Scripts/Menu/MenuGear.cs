using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class MenuGear : Menu
{
    [SerializeField] Button firstButtonToSelect;
    public PlayerInventory playerInventory;
    public GameObject itemDescriptionGO;
    public InventorySlot[] inventorySlot;

    public override void DisplayMenu(bool on)

    {
        itemDescriptionGO.SetActive(false);
        var player = GameObject.Find("Player");
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

        displayContainer.SetActive(on);
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
        menuButtonHighlighted.SetButtonColor(menuButtonHighlighted.highlightedColor);
        menuButtonHighlighted.enabled = false;
        firstButtonToSelect.Select();
        itemDescriptionGO.SetActive(true);
    }

    public override void ExitMenu()

    {
        menuButtonHighlighted.enabled = true;
        menuButtonHighlighted.SetButtonColor(Color.white);
        mainButtonToRevert.Select();
        menuManagerUI.menuUpdateMethod = menuManagerUI.main;
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitMenu();
        }
    }
}
