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
    public TextMeshProUGUI gearDescriptionTMP;
    public TextMeshProUGUI gearTypeTMP;
    public TextMeshProUGUI gearValueTMP;
    public TextMeshProUGUI gearEquipStatusTMP;
    uiGearSlot gearEquipSlotHighlighted;
    public TextMeshProUGUI pageHeaderTMP;
    public GearSO transplanting;

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

        if (transplanting != null && gearEquipSlotSelected.gearEquipped != transplanting)
        {
            menuGear.playerInventory.UnequipGearFromSlot(transplanting);
            transplanting = null;
        }

        GearSO geartoEquip = inventorySlotSelected.gear;
        menuGear.playerInventory.EquipGearToSlot(geartoEquip, gearEquipSlotSelected.equipSlotNumber);
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

            if (menuGear.playerInventory.inventorySO.equippedGear[i] == null)
            {
                uiGearSlots[i].buttonTMP.text = "EQUIPPED SLOT " + (i + 1) + ": "+ "EMPTY";
                uiGearSlots[i].gameObject.SetActive(true);
            }

            else
            {
                GearSO gearToLoad = menuGear.playerInventory.inventorySO.equippedGear[i];
                uiGearSlots[i].gearEquipped = gearToLoad;
                uiGearSlots[i].buttonTMP.text = "EQUIPPED SLOT " + (i+1) + ": " + gearToLoad.gearName;
                uiGearSlots[i].gameObject.SetActive(true);
            }
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
                menuGear.gearHighlighted = gearEquipSlotHighlighted.gearEquipped;
                menuGear.UnequipHighlightedGear();
                DisplayEquipSlots();
                EquipSlotHighlighted(gearEquipSlotHighlighted);
                transplanting = null;
            }
        }
    }
}
