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
    public List<InventorySlotUI> equipSlots = new List<InventorySlotUI>();
    public TextMeshProUGUI gearDescriptionTMP;
    public TextMeshProUGUI gearTypeTMP;
    public TextMeshProUGUI gearValueTMP;
    public TextMeshProUGUI gearEquipStatusTMP;
    public int highlightedButtonIndex;


    public GameObject equipPageHeaderGO;
    public TextMeshProUGUI pageHeaderTMP;
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

    public void EquipSlotSelected(InventorySlotUI gearEquipSlotSelected)
    {
        var slot = equipSlots[highlightedButtonIndex];

        if (slot.gearInstance != null && slot.gearInstance.gearSO != null)
            return;

        else if (isEquipping)
        {
            InventorySlotUI inventorySlot = menuGearInventorySubPage.inventorySlots[menuGearInventorySubPage.highlightedButtonIndex];

            menuGearInventorySubPage.playerInventory.EquipGearToSlot(inventorySlot.gearInstance, equipSlots.IndexOf(gearEquipSlotSelected));
            InitialiseEquipSlots();
            menuGearInventorySubPage.InstantiateUIInventorySlots();
            menuGearPageSelection.displayContainer.SetActive(true);
            equipPageHeaderGO.SetActive(false);
            menuGearPageSelection.lastParentButtonSelected = menuGearPageSelection.equippedHighlightedButton;

            menuGearPageSelection.lastParentButtonSelected.button.onClick.Invoke();
            menuGearPageSelection.inventoryHighlightedButton.SetButtonNormalColor(Color.white);

            firstButtonToSelect = equipSlots[highlightedButtonIndex].button;
            firstButtonToSelect.Select();

            isEquipping = false;
        }

        else
        {
            EventSystem.current.SetSelectedGameObject(null);
            ExitMenu();
            menuGearPageSelection.inventoryHighlightedButton.button.Select();
            menuGearPageSelection.inventoryHighlightedButton.button.onClick.Invoke();
        }
    }

    public override void EnterMenu()
    {
        DisplayMenu(true);
        if (firstButtonToSelect == null)
            firstButtonToSelect = equipSlots[0].button;

        firstButtonToSelect.Select();
    }

    public void InitialiseEquipSlots()
    {
        DeleteAllInventoryUI();

        var gearInstanceEquipped = menuGearInventorySubPage.playerInventory.inventorySO.gearInstanceEquipped;

        for (int i = 0; i < gearInstanceEquipped.Count ; i++)
        {
            GameObject UIEquipSlotGO = Instantiate(UIInventorySlotPrefab);
            UIEquipSlotGO.transform.SetParent(equipSlotsParent.transform, false);
            UIEquipSlotGO.name = "Equip slot " + (i + 1);

            InventorySlotUI equipSlot = UIEquipSlotGO.GetComponent<InventorySlotUI>();

            equipSlot.button.onClick.AddListener(() => EquipSlotSelected(equipSlot));


            if (gearInstanceEquipped[i] == null || gearInstanceEquipped[i].gearSO == null)
            {
                equipSlot.itemNameTMP.text = "GEAR SLOT " + (i + 1) + ": " + "EMPTY";
            }

            else
            {
                equipSlot.gearInstance = gearInstanceEquipped[i];
                equipSlot.itemNameTMP.text = equipSlot.gearInstance.gearSO.gearName;

                equipSlot.itemQuantityTMP.text = ItemQuantityRemaining(equipSlot.gearInstance);

                bool isEquipment = equipSlot.gearInstance is EquipmentInstance;
            }

            SetEquipSlotColor(equipSlot, GetEquipSlotBaseColor(equipSlot));

            equipSlot.onHighlighted = () =>
            {
                EquipSlotHighlighted(equipSlot);
                SetEquipSlotColor(equipSlot, Color.yellow);
            };

            equipSlot.onUnHighlighted = () =>
            {
                SetEquipSlotColor(equipSlot, GetEquipSlotBaseColor(equipSlot));
            };

            equipSlot.itemQuantityTMP.text = "";


            equipSlots.Add(equipSlot);
        }

        List<Button> equipSlotButtons = new List<Button>();
        foreach (var equipSlot in equipSlots)
            equipSlotButtons.Add(equipSlot.button);

        FieldEvents.SetGridNavigationWrapAround(equipSlotButtons, 5);
    }

    Color GetEquipSlotBaseColor(InventorySlotUI equipSlot)
    {
        if (equipSlot.gearInstance == null || equipSlot.gearInstance.gearSO == null)
            return Color.white;

        if (equipSlot.gearInstance is ConsumableInstance)
            return equipSlot.consumableColor;

        else return equipSlot.equipmentColor;
    }

    public void SetEquipSlotColor(InventorySlotUI inventorySlot, Color normalColor)
    {
        inventorySlot.itemNameTMP.color = normalColor;
        inventorySlot.itemQuantityTMP.color = normalColor;
    }

    string ItemQuantityRemaining(GearInstance gearInstance)
    {
        if (gearInstance is EquipmentInstance equipmentInstance)
            return ": " + equipmentInstance.ChargePercentage() + "%";
        else if (gearInstance is ConsumableInstance consumableInstance)
            return "";
        else
        { 
            Debug.Log("something broke");
            return "";
        }
    }

    public void DeleteAllInventoryUI()
    {
        equipSlots.Clear();

        for (int i = equipSlotsParent.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(equipSlotsParent.transform.GetChild(i).gameObject);
        }
    }

    public void EquipSlotHighlighted(InventorySlotUI gearEquipSlot)
    {
        highlightedButtonIndex = equipSlots.IndexOf(gearEquipSlot);

        if (gearEquipSlot.gearInstance == null || gearEquipSlot.gearInstance.gearSO == null)
        {
            gearDescriptionTMP.text = "Slot free";
            gearTypeTMP.text = "";
            gearEquipStatusTMP.text = "";
            gearValueTMP.text = "";

            return;
        }

        if (gearEquipSlot.gearInstance is EquipmentInstance)
            gearTypeTMP.text = "Type: Equipment";

        else
            gearTypeTMP.text = "Type: Consumable";

        if (gearEquipSlot.gearInstance.isCurrentlyEquipped)
        {
            gearEquipStatusTMP.text = "CTRL to unequip";
        }

        gearDescriptionTMP.text = "Description: " + gearEquipSlot.gearInstance.gearSO.gearDescription;
        gearValueTMP.text = "Sell Value: " + gearEquipSlot.gearInstance.gearSO.value.ToString("N0") + " $MAMS";
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
            equipSlots[highlightedButtonIndex].onUnHighlighted();
            ExitMenu();
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
