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
    public GameObject gearPropertiesDisplayGO;
    public InventorySlot[] inventorySlot;
    public MenuGearEquip menuGearEquip;
    public TextMeshProUGUI gearDescriptionTMP;
    public TextMeshProUGUI gearTypeTMP;
    public TextMeshProUGUI gearValueTMP;
    public TextMeshProUGUI gearEquipStatusTMP;
    public GearSO gearHighlighted;
    public GameObject timeDisplayGO;
    public GameObject smamsDisplayGO;

    public override void DisplayMenu(bool on)
    {
        if (on) 
        {
            gearPropertiesDisplayGO.SetActive(false);
            var player = GameObject.Find("Player");
            playerInventory = player.GetComponentInChildren<PlayerInventory>();

            DisableAllSlots();

            for (int i = 0; i < playerInventory.inventorySO.gearInventory.Count; i++)
            {
                GearSO gear = playerInventory.inventorySO.gearInventory[i];
                inventorySlot[i].gear = gear;
                inventorySlot[i].itemName.text = gear.gearName;
                inventorySlot[i].itemQuantity.text = " x " + gear.quantityInInventory;
                menuManagerUI.SetTextAlpha(inventorySlot[i].itemName, gear.isCurrentlyEquipped ? 0.5f : 1f);
                menuManagerUI.SetTextAlpha(inventorySlot[i].itemQuantity, gear.isCurrentlyEquipped ? 0.5f : 1f);
                inventorySlot[i].gameObject.SetActive(true);
            }
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
        gearPropertiesDisplayGO.SetActive(true);
        timeDisplayGO.SetActive(false);
        smamsDisplayGO.SetActive(false);
    }

    public void InventorySlotSelected(InventorySlot inventorySlot)
    {
        menuGearEquip.transplanting = null;

        menuGearEquip.inventorySlotSelected = inventorySlot;
        firstButtonToSelect = inventorySlot.GetComponent<Button>();
        menuManagerUI.EnterSubMenu(menuManagerUI.gearEquipPage);

        if (inventorySlot.gear.isCurrentlyEquipped)
        {
            menuGearEquip.transplanting = inventorySlot.gear;
        }
    }

    public void GearHighlighted(GearSO gear)
    {
        gearValueTMP.text = "Value: " + gear.value.ToString() + " $MAMS";
        gearHighlighted = gear;

        if (!gear.isConsumable)
        {
            gearTypeTMP.text = "Type: Accessory";
        }

        else
        {
            gearTypeTMP.text = "Type: Consumable";
        }

        if (gear.isCurrentlyEquipped)
        {
            gearDescriptionTMP.text = gear.gearDescription;
            gearEquipStatusTMP.text = "Equipped to Slot " + (gear.equipSlotNumber + 1) + ". PRESS CTRL TO REMOVE";
        }
        else
        {
            gearDescriptionTMP.text = gear.gearDescription;
            gearEquipStatusTMP.text = "Unequipped";
        }
    }

    public void UnequipHighlightedGear()
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
                gearPropertiesDisplayGO.SetActive(true);
                GearHighlighted(gearHighlighted);
            }
        }
    }
}
