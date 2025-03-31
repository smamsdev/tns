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
    public TextMeshProUGUI itemTypeTMP;
    public TextMeshProUGUI itemvalue;
    public TextMeshProUGUI descriptionFieldTMP;
    Gear gearHighlighted;

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
            menuManagerUI.SetTextAlpha(inventorySlot[i].itemName, gearToLoad.isCurrentlyEquipped ? 0.5f : 1f);
            menuManagerUI.SetTextAlpha(inventorySlot[i].itemQuantity, gearToLoad.isCurrentlyEquipped ? 0.5f : 1f);
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

    public void InventorySlotSelected(InventorySlot inventorySlot)
    {
        if (!inventorySlot.gear.isCurrentlyEquipped)
        {
            menuGearEquip.inventorySlotSelected = inventorySlot;
            firstButtonToSelect = inventorySlot.GetComponent<Button>();

            menuManagerUI.EnterSubMenu(menuManagerUI.gearEquipPage);
        }
    }

    public void GearHighlighted(Gear gear)
    {
        itemvalue.text = gear.value.ToString();
        gearHighlighted = gear;

        if (!gear.isConsumable)
        {
            itemTypeTMP.text = "Equipment";
        }

        else
        {
            itemTypeTMP.text = "Consumable";
        }

        if (gear.isCurrentlyEquipped)
        {
            descriptionFieldTMP.text = "Currently Equipped. PRESS CTRL TO REMOVE\n" + gear.gearDescription;
        }
        else
        {
            descriptionFieldTMP.text = gear.gearDescription;
        }
    }

    void UnequipHighlightedGear()
    {
        playerInventory.UnequipGearFromSlot(gearHighlighted);
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

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (gearHighlighted.isCurrentlyEquipped)
            {
                UnequipHighlightedGear();
                DisplayMenu(true);
            }
        }
    }
}
