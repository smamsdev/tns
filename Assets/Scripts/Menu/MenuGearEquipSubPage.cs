using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuGearEquipSubPage : Menu
{
    [SerializeField] Button firstButtonToSelect;
    public InventorySlot inventorySlotSelected;
    public MenuGearPageSelection menuGearPageSelection;
    public MenuGearInventorySubPage menuGearInventorySubPage;
    public UIGearEquipSlot[] uIGearEquipSlots;
    public List<Button> equipSlotButtons = new List<Button>();
    public TextMeshProUGUI gearDescriptionTMP;
    public TextMeshProUGUI gearTypeTMP;
    public TextMeshProUGUI gearValueTMP;
    public TextMeshProUGUI gearEquipStatusTMP;
    public UIGearEquipSlot gearEquipSlotHighlighted;
    public TextMeshProUGUI pageHeaderTMP;
    public bool isEquipping = false;

    private void OnEnable()
    {
        DisplayMenu(false);
    }

    public override void DisplayMenu(bool on)
    {
        displayContainer.SetActive(on);
    }

    public void EquipSlotSelected(UIGearEquipSlot gearEquipSlotSelected)
    {
        EventSystem.current.SetSelectedGameObject(null);

        ExitMenu();
        menuGearPageSelection.inventoryHighlightedButton.button.Select();
        menuGearPageSelection.inventoryHighlightedButton.button.onClick.Invoke();

        // GearSO gearGettingReplaced = menuGearInventorySubPage.playerInventory.inventorySO.equippedGear[gearEquipSlotSelected.equipSlotNumber];

        //if (gearGettingReplaced != null)
        //{
        //    menuGearInventorySubPage.playerInventory.UnequipGearFromSlot(gearGettingReplaced);
        //
        //    InventorySlot slot = menuGearInventorySubPage.gearToSlot[gearGettingReplaced];
        //    menuManagerUI.SetTextAlpha(slot.itemName, 1f);
        //    menuManagerUI.SetTextAlpha(slot.itemQuantity, 1f);
        //    slot.itemQuantity.text = "x" + gearGettingReplaced.quantityInInventory.ToString();
        //}

        //GearSO geartoEquip = inventorySlotSelected.gear;
        //menuGearInventorySubPage.playerInventory.EquipGearToSlot(geartoEquip, gearEquipSlotSelected.equipSlotNumber);
        //
        //menuManagerUI.SetTextAlpha(inventorySlotSelected.itemName, 0.5f);
        //menuManagerUI.SetTextAlpha(inventorySlotSelected.itemQuantity, 0.5f);
        //inventorySlotSelected.itemQuantity.text = "x" + geartoEquip.quantityInInventory.ToString();


    }

    public override void EnterMenu()
    {
        //pageHeaderTMP.text = "Equip " + inventorySlotSelected.gear.gearName + "?";
        DisplayMenu(true);
        firstButtonToSelect.Select();
    }

    public void InitialiseEquipSlots()
    {
        var inventorySO = menuGearInventorySubPage.playerInventory.inventorySO;

        foreach (UIGearEquipSlot gearEquipSlot in uIGearEquipSlots)
        {
            gearEquipSlot.gearEquipped = null;
            gearEquipSlot.gameObject.SetActive(false);
        }

        for (int i = 0; i < inventorySO.equipSlotsAvailable; i++)
        {
            if (inventorySO.equippedGear[i] == null)
            {
                uIGearEquipSlots[i].buttonTMP.text = "GEAR SLOT " + (i + 1) + ": " + "EMPTY";
                uIGearEquipSlots[i].gameObject.SetActive(true);
            }

            else
            {
                GearSO gearToLoad = inventorySO.equippedGear[i];
                uIGearEquipSlots[i].gearEquipped = gearToLoad;
                uIGearEquipSlots[i].buttonTMP.text = "GEAR SLOT " + (i + 1) + ": " + gearToLoad.gearName;
                uIGearEquipSlots[i].gameObject.SetActive(true);
            }
        }

        FieldEvents.SetGridNavigationWrapAround(equipSlotButtons, inventorySO.equipSlotsAvailable);
    }

    public void EquipSlotHighlighted(UIGearEquipSlot gearEquipSlot)
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
        menuManagerUI.EnterMenu(menuManagerUI.gearPageSelection);
        menuManagerUI.menuUpdateMethod.lastParentButtonSelected.SetButtonNormalColor(Color.white);
        menuManagerUI.menuUpdateMethod.lastParentButtonSelected.button.Select();
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
                menuGearInventorySubPage.UnequipHighlightedGear(gearEquipSlotHighlighted.gearEquipped);
                InitialiseEquipSlots();
                EquipSlotHighlighted(gearEquipSlotHighlighted);
            }
        }
    }
}
