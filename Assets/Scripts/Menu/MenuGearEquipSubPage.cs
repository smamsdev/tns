using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuGearEquipSubPage : PauseMenu
{
    public Button firstButtonToSelect;
    public MenuGearMainPage menuGearMainPage;
    public MenuGearInventorySubPage menuGearInventorySubPage;
    public List<InventorySlotUI> equipSlots = new List<InventorySlotUI>();
    public int highlightedButtonIndex;
    public GameObject UIInventorySlotPrefab, equipSlotsParent;
    public bool isEquipping = false;

    private void OnEnable()
    {
        DisplayMenu(false);
    }

    public override void DisplayMenu(bool on)
    {
        displayContainer.SetActive(on);
    }

    public override void EnterMenu()
    {
        menuGearMainPage.ShowMainButtons(false);
        DisplayMenu(true);
        if (firstButtonToSelect == null)
            firstButtonToSelect = equipSlots[0].button;

        foreach (var slot in equipSlots)
            menuGearMainPage.SetSlotAlpha(slot, alphaCondition: slot.gearInstance.gearSO == null);

        firstButtonToSelect.Select();
    }

    public void InitialiseEquipSlots()
    {
        DeleteAllInventoryUI();

        var gearInstanceEquipped = menuGearMainPage.playerInventorySO.gearInstanceEquipped;

        for (int i = 0; i < gearInstanceEquipped.Count ; i++)
        {
            GameObject UIEquipSlotGO = Instantiate(UIInventorySlotPrefab);
            UIEquipSlotGO.transform.SetParent(equipSlotsParent.transform, false);
            UIEquipSlotGO.name = "Equip slot " + (i + 1);

            InventorySlotUI equipSlot = UIEquipSlotGO.GetComponent<InventorySlotUI>();

            equipSlot.button.onClick.AddListener(() => EquipSlotSelected(equipSlot));
            equipSlot.icon.sprite = equipSlot.equipmentIcon;

            if (gearInstanceEquipped[i] == null || gearInstanceEquipped[i].gearSO == null)
            {
                equipSlot.itemNameTMP.text = "Slot " + (i + 1) + ": " + "EMPTY";
                equipSlot.itemQuantityTMP.text = "";
                equipSlot.icon.sprite = equipSlot.freeIcon;
                equipSlot.gearInstance = new GearInstance();
            }

            else
            {
                equipSlot.gearInstance = gearInstanceEquipped[i];
                equipSlot.itemNameTMP.text = equipSlot.gearInstance.gearSO.gearName;

                equipSlot.itemQuantityTMP.text = equipSlot.gearInstance.GearQuantityRemainingString();

                bool isEquipment = equipSlot.gearInstance is EquipmentInstance;
                equipSlot.icon.sprite = isEquipment? equipSlot.equipmentIcon: equipSlot.consumableIcon;
            }

            equipSlot.onHighlighted = () =>
            {
                EquipSlotHighlighted(equipSlot);
            };

            equipSlot.onUnHighlighted = () =>
            {
                menuGearMainPage.SetSlotColor(equipSlot, Color.white);
            };

            equipSlots.Add(equipSlot);
        }

        List<Button> equipSlotButtons = new List<Button>();
        foreach (var equipSlot in equipSlots)
            equipSlotButtons.Add(equipSlot.button);

        FieldEvents.SetGridNavigationWrapAround(equipSlotButtons, gearInstanceEquipped.Count);
    }

    public void DeleteAllInventoryUI()
    {
        equipSlots.Clear();

        for (int i = equipSlotsParent.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(equipSlotsParent.transform.GetChild(i).gameObject);
        }
    }

    public void EquipSlotHighlighted(InventorySlotUI inventorySlotUI)
    {
        var gearInstance = inventorySlotUI.gearInstance;

        highlightedButtonIndex = equipSlots.IndexOf(inventorySlotUI);
        menuGearMainPage.SetSlotColor(inventorySlotUI, Color.yellow);
        menuGearMainPage.UpdateGearDescriptionTMPs(gearInstance);

        bool slotOccupied = inventorySlotUI.gearInstance.gearSO != null;

        if (slotOccupied)
        {
            menuGearMainPage.UpdateHeaderTMP("Slot occupied");
            return;
        }

        if (!isEquipping)
        {
            menuGearMainPage.UpdateHeaderTMP("Equip to slot " + (highlightedButtonIndex + 1) + "?");
            return;
        }

        var gear = menuGearInventorySubPage.inventorySlots[menuGearInventorySubPage.highlightedButtonIndex].gearInstance;

        menuGearMainPage.UpdateHeaderTMP("Equip " + gear.gearSO.gearName + " to Slot " + (highlightedButtonIndex + 1) + "?");
    }

    public void EquipSlotSelected(InventorySlotUI gearEquipSlotSelected)
    {
        var slot = equipSlots[highlightedButtonIndex];

        if (slot.gearInstance != null && slot.gearInstance.gearSO != null)
            return;

        else if (isEquipping)
        {
            GearInstance gearInstanceToEquip = menuGearInventorySubPage.inventorySlots[menuGearInventorySubPage.highlightedButtonIndex].gearInstance;

            menuGearMainPage.playerInventorySO.EquipGearToSlot(gearInstanceToEquip, equipSlots.IndexOf(gearEquipSlotSelected));
            menuGearMainPage.playerInventorySO.SortInventory();
            InitialiseEquipSlots();
            menuGearInventorySubPage.InitialiseInventoryUI();
            menuGearMainPage.lastParentButtonSelected = menuGearMainPage.equippedHighlightedButton;

            menuGearMainPage.lastParentButtonSelected.button.onClick.Invoke();
            menuGearMainPage.inventoryHighlightedButton.SetButtonNormalColor(Color.white);

            firstButtonToSelect = equipSlots[highlightedButtonIndex].button;
            isEquipping = false;

            firstButtonToSelect.Select();
        }

        else
        {
            //do i need this?
            EventSystem.current.SetSelectedGameObject(null);
            ExitMenu();
            menuGearMainPage.inventoryHighlightedButton.button.Select();
            //do i need this?
            menuGearMainPage.inventoryHighlightedButton.button.onClick.Invoke();
        }
    }

    public override void ExitMenu()
    {
        pauseMenuManager.EnterMenu(pauseMenuManager.gearPageSelection);
        pauseMenuManager.menuUpdateMethod.lastParentButtonSelected.SetButtonNormalColor(Color.white);
        pauseMenuManager.menuUpdateMethod.lastParentButtonSelected.button.Select();
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            equipSlots[highlightedButtonIndex].onUnHighlighted();
            ExitMenu();

            if (isEquipping)
            {
                EventSystem.current.SetSelectedGameObject(null);
                menuGearMainPage.inventoryHighlightedButton.button.Select();
                menuGearMainPage.inventoryHighlightedButton.button.onClick.Invoke();
                isEquipping = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
           if (equipSlots[highlightedButtonIndex].gearInstance.gearSO != null)
            {
                menuGearInventorySubPage.UnequipHighlightedGearInstance(equipSlots[highlightedButtonIndex].gearInstance);
                InitialiseEquipSlots();
                equipSlots[highlightedButtonIndex].button.Select();
            }
        }
    }
}
