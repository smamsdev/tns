using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class MenuGear : Menu
{
    public Button firstButtonToSelect;
    public PlayerInventory playerInventory;
    public GameObject itemDescriptionGO;
    public InventorySlot[] inventorySlot;
    public MenuGearEquip menuGearEquip;

    public override void DisplayMenu(bool on)
    {
        itemDescriptionGO.SetActive(false);
        var player = GameObject.Find("Player");
        playerInventory = player.GetComponentInChildren<PlayerInventory>();

        DisableAllSlots();

        // Combine inventories into one list
        List<Gear> combinedList = new List<Gear>();

        // Add items from inventory
        foreach (Gear gearToLoad in playerInventory.inventory)
        {
            if (gearToLoad != null && !combinedList.Contains(gearToLoad))
            {
                combinedList.Add(gearToLoad); // Add to combined list only if not already added
            }
        }

        // Add items from equippedInventory
        foreach (Gear gearToLoad in playerInventory.equippedInventory)
        {
            if (gearToLoad != null && !combinedList.Contains(gearToLoad))
            {
                combinedList.Add(gearToLoad); // Add to combined list only if not already added
            }
        }

        // Sort combined list alphabetically by gearID (or any other property you prefer)
        combinedList.Sort((gear1, gear2) => string.Compare(gear1.gearID, gear2.gearID));

        // Now render the sorted list to the inventory slots
        int i = 0;
        foreach (Gear gear in combinedList)
        {
            if (i >= inventorySlot.Length) break; // Prevent going out of bounds if there are more items than slots

            inventorySlot[i].gear = gear;
            inventorySlot[i].itemName.text = gear.gearID; // Or use gear.itemName if you have it
            inventorySlot[i].itemQuantity.text = " x " + gear.quantityInInventory;
            menuManagerUI.SetTextAlpha(inventorySlot[i].itemName, gear.isCurrentlyEquipped ? 0.5f : 1f);
            menuManagerUI.SetTextAlpha(inventorySlot[i].itemQuantity, gear.isCurrentlyEquipped ? 0.5f : 1f);
            inventorySlot[i].gameObject.SetActive(true); // Show the slot

            i++;
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

    public void InventorySlotSelected(InventorySlot inventorySlot)
    {
        if (!inventorySlot.gear.isCurrentlyEquipped)
        { 
            menuGearEquip.inventorySlotSelected = inventorySlot;
            firstButtonToSelect = inventorySlot.GetComponent<Button>();

            menuManagerUI.EnterSubMenu(menuManagerUI.gearEquipPage);
        }
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
