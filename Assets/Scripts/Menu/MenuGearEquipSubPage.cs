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
    public TextMeshProUGUI gearDescriptionTMP;
    public TextMeshProUGUI chargeAmountTMP;
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

            menuGearMainPage.playerInventory.inventorySO.EquipGearToSlot(inventorySlot.gearInstance, equipSlots.IndexOf(gearEquipSlotSelected));
            InitialiseEquipSlots();
            menuGearInventorySubPage.InitialiseInventoryUI();
            menuGearMainPage.displayContainer.SetActive(true);
            equipPageHeaderGO.SetActive(false);
            menuGearMainPage.lastParentButtonSelected = menuGearMainPage.equippedHighlightedButton;

            menuGearMainPage.lastParentButtonSelected.button.onClick.Invoke();
            menuGearMainPage.inventoryHighlightedButton.SetButtonNormalColor(Color.white);

            firstButtonToSelect = equipSlots[highlightedButtonIndex].button;
            firstButtonToSelect.Select();

            isEquipping = false;
        }

        else
        {
            EventSystem.current.SetSelectedGameObject(null);
            ExitMenu();
            menuGearMainPage.inventoryHighlightedButton.button.Select();
            menuGearMainPage.inventoryHighlightedButton.button.onClick.Invoke();
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

        var gearInstanceEquipped = menuGearMainPage.playerInventory.inventorySO.gearInstanceEquipped;

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
                equipSlot.itemNameTMP.text = "GEAR SLOT " + (i + 1) + ": " + "EMPTY";
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
                SetEquipSlotColor(equipSlot, Color.white);
            };

            equipSlots.Add(equipSlot);
        }

        List<Button> equipSlotButtons = new List<Button>();
        foreach (var equipSlot in equipSlots)
            equipSlotButtons.Add(equipSlot.button);

        FieldEvents.SetGridNavigationWrapAround(equipSlotButtons, gearInstanceEquipped.Count);
    }

    public void SetEquipSlotColor(InventorySlotUI inventorySlot, Color normalColor)
    {
        inventorySlot.itemNameTMP.color = normalColor;
        inventorySlot.itemQuantityTMP.color = normalColor;
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
        SetEquipSlotColor(inventorySlotUI, Color.yellow);
        highlightedButtonIndex = equipSlots.IndexOf(inventorySlotUI);

        var gearInstance = inventorySlotUI.gearInstance;

        if (gearInstance.gearSO == null)
        {
            gearDescriptionTMP.text = "";
            chargeAmountTMP.text = "";
            gearEquipStatusTMP.text = "Select to equip to Slot " + (highlightedButtonIndex+1);
            gearValueTMP.text = "";
            return;
        }

        gearDescriptionTMP.text = "Description: " + gearInstance.gearSO.gearDescription;
        gearValueTMP.text = "Sell Value: " + gearInstance.gearSO.value.ToString("N0") + " $MAMS";

        if (gearInstance is EquipmentInstance equipmentInstance) chargeAmountTMP.text = equipmentInstance.ChargeTotalString();
        else chargeAmountTMP.text = "";

        if (inventorySlotUI.gearInstance.isCurrentlyEquipped)
            gearEquipStatusTMP.text = "CTRL to unequip";
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
