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
    public Button firstButtonToSelect;
    public MenuGearPageSelection menuGearPageSelection;
    public MenuGearInventorySubPage menuGearInventorySubPage;
    public UIGearEquipSlot[] uIGearEquipSlots;
    public List<Button> equipSlotButtons = new List<Button>();
    public TextMeshProUGUI gearDescriptionTMP;
    public TextMeshProUGUI gearTypeTMP;
    public TextMeshProUGUI gearValueTMP;
    public TextMeshProUGUI gearEquipStatusTMP;
    public UIGearEquipSlot gearEquipSlotHighlighted;
    public GameObject equipPageHeaderGO;
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
        if (gearEquipSlotHighlighted.gearEquipped != null) 
        return;

        else if (!isEquipping)
        {
            EventSystem.current.SetSelectedGameObject(null);
            ExitMenu();
            firstButtonToSelect = gearEquipSlotHighlighted.GetComponent<Button>();
            menuGearPageSelection.inventoryHighlightedButton.button.Select();
            menuGearPageSelection.inventoryHighlightedButton.button.onClick.Invoke();
        }

        else
        {
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

            var inventorySlot = menuGearInventorySubPage.inventorySlotSelected;

            GearSO geartoEquip = inventorySlot.gear;
            menuGearInventorySubPage.playerInventory.EquipGearToSlot(geartoEquip, gearEquipSlotSelected.equipSlotNumber);
            
            FieldEvents.SetTextAlpha(inventorySlot.itemNameTMP, 0.5f);
            FieldEvents.SetTextAlpha(inventorySlot.itemQuantityTMP, 0.5f);

            if (inventorySlot.gear is EquipmentSO equipment)
            {
                inventorySlot.itemQuantityTMP.text = equipment.Potential + "%";
            }

            else if (inventorySlot.gear is ConsumbableSO consumable)
            {
                inventorySlot.itemQuantityTMP.text = "x" + consumable.quantityInInventory;
            }

            InitialiseEquipSlots();
            gearEquipSlotHighlighted.GetComponent<Button>().Select();
            menuGearPageSelection.displayContainer.SetActive(true);
            equipPageHeaderGO.SetActive(false);
            menuGearPageSelection.lastParentButtonSelected = menuGearPageSelection.equippedHighlightedButton;
            firstButtonToSelect = gearEquipSlotHighlighted.GetComponent<Button>();
            menuGearPageSelection.lastParentButtonSelected.button.onClick.Invoke();
            menuGearPageSelection.inventoryHighlightedButton.SetButtonNormalColor(Color.white);
            menuGearInventorySubPage.InstantiateUIInventorySlots();

            isEquipping = false;
        }
    }

    public override void EnterMenu()
    {
        DisplayMenu(true);
        if (!isEquipping) { firstButtonToSelect = uIGearEquipSlots[0].GetComponent<Button>(); };

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
                if (gearToLoad is EquipmentSO equipment)
                {
                    uIGearEquipSlots[i].buttonTMP.text += " " + equipment.Potential + "%";
                }

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
            if (gearEquipSlot.gearEquipped is EquipmentSO)
            {
                gearTypeTMP.text = "Type: Equipment";
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

            gearValueTMP.text = "$MAMS: " + gearEquipSlot.gearEquipped.value.ToString();
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
