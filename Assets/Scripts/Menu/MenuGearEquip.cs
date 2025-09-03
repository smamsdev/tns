using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class MenuGearEquip : Menu
{
    [SerializeField] Button firstButtonToSelect;
    public InventorySlot inventorySlotSelected;
    public MenuGear menuGear;
    public uiGearSlot[] uiGearSlots;
    public List<Button> equipSlotButtons = new List<Button>();
    public TextMeshProUGUI gearDescriptionTMP;
    public TextMeshProUGUI gearTypeTMP;
    public TextMeshProUGUI gearValueTMP;
    public TextMeshProUGUI gearEquipStatusTMP;
    public uiGearSlot gearEquipSlotHighlighted;
    public TextMeshProUGUI pageHeaderTMP;
    public InventorySlot transplantingSlot;

    private void OnEnable()
    {
        DisplayMenu(false);
    }

    public override void DisplayMenu(bool on)
    {
        displayContainer.SetActive(on);
    }

    public void EquipToSlotSelected(uiGearSlot gearEquipSlotSelected)
    {
        gearEquipSlotSelected.Deselect();

        if (transplantingSlot != null && gearEquipSlotSelected.gearEquipped != transplantingSlot.gear)
        {
            menuGear.playerInventory.UnequipGearFromSlot(transplantingSlot.gear);
            transplantingSlot = null;
        }

        GearSO gearGettingReplaced = menuGear.playerInventory.inventorySO.equippedGear[gearEquipSlotSelected.equipSlotNumber];

        if (gearGettingReplaced != null)
        {
            menuGear.playerInventory.UnequipGearFromSlot(gearGettingReplaced);

            var slot = menuGear.gearToSlot[gearGettingReplaced];
            menuManagerUI.SetTextAlpha(slot.itemName, 1f);
            menuManagerUI.SetTextAlpha(slot.itemQuantity, 1f);
        }

        GearSO geartoEquip = inventorySlotSelected.gear;
        menuGear.playerInventory.EquipGearToSlot(geartoEquip, gearEquipSlotSelected.equipSlotNumber);

        menuManagerUI.SetTextAlpha(inventorySlotSelected.itemName, 0.5f);
        menuManagerUI.SetTextAlpha(inventorySlotSelected.itemQuantity, 0.5f);

        ExitMenu();
    }

    public override void EnterMenu()
    {
        pageHeaderTMP.text = "Equip " + inventorySlotSelected.gear.gearID + "?";
        DisplayEquipSlots();
        menuManagerUI.gearPage.displayContainer.SetActive(false);
        DisplayMenu(true);
        firstButtonToSelect.Select();
        //itemDescriptionGO.SetActive(true);
    }

    public void DisplayEquipSlots()
    {
        foreach (uiGearSlot gearEquipSlot in uiGearSlots)
        {
            gearEquipSlot.gearEquipped = null;
            gearEquipSlot.gameObject.SetActive(false);
        }

        for (int i = 0; i < menuGear.playerInventory.inventorySO.equipSlotsAvailable; i++)
        {
            if (menuGear.playerInventory.inventorySO.equippedGear[i] == null)
            {
                uiGearSlots[i].buttonTMP.text = "EQUIPPED SLOT " + (i + 1) + ": " + "EMPTY";
                uiGearSlots[i].gameObject.SetActive(true);
            }

            else
            {
                GearSO gearToLoad = menuGear.playerInventory.inventorySO.equippedGear[i];
                uiGearSlots[i].gearEquipped = gearToLoad;
                uiGearSlots[i].buttonTMP.text = "EQUIPPED SLOT " + (i + 1) + ": " + gearToLoad.gearName;
                uiGearSlots[i].gameObject.SetActive(true);
            }
        }

        FieldEvents.SetGridNavigationWrapAround(equipSlotButtons, menuGear.playerInventory.inventorySO.equipSlotsAvailable);
    }

    public void EquipSlotHighlighted(uiGearSlot gearEquipSlot)
    {
        gearEquipSlotHighlighted = gearEquipSlot;

        if (gearEquipSlot.gearEquipped == null)
        {
            gearDescriptionTMP.text = "Slot free";
            gearTypeTMP.text = "";
            gearEquipStatusTMP.text = "";
            gearValueTMP.text = "";
        }

        else
        
        {
            if (!gearEquipSlot.gearEquipped.isConsumable)
            {
                gearTypeTMP.text = "Type: Accessory";
            }

            else
            {
                gearTypeTMP.text = "Type: Consumable";
            }

            if (gearEquipSlot.gearEquipped.isCurrentlyEquipped)
            {
                gearDescriptionTMP.text = gearEquipSlot.gearEquipped.gearDescription;
                gearEquipStatusTMP.text = "Currently Equipped. PRESS CTRL TO REMOVE";
            }
            else
            {
                gearDescriptionTMP.text = gearEquipSlot.gearEquipped.gearDescription;
            }

            gearValueTMP.text = gearEquipSlot.gearEquipped.value.ToString();
        }
    }

    public override void ExitMenu()
    {
        menuManagerUI.DisplayMenu(menuManagerUI.gearPage);
        menuManagerUI.EnterSubMenu(menuManagerUI.gearPage);
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gearEquipSlotHighlighted.Deselect();
            ExitMenu();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
           if (gearEquipSlotHighlighted.gearEquipped != null)
            {
                menuGear.UnequipHighlightedGear(gearEquipSlotHighlighted.gearEquipped);
                DisplayEquipSlots();
                EquipSlotHighlighted(gearEquipSlotHighlighted);
                transplantingSlot = null;
            }
        }
    }
}
