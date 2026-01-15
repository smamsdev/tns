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
    public List<Button> equipSlotButtons = new List<Button>();
    List<InventorySlotUI> equipSlots = new List<InventorySlotUI>();
    public TextMeshProUGUI gearDescriptionTMP;
    public TextMeshProUGUI gearTypeTMP;
    public TextMeshProUGUI gearValueTMP;
    public TextMeshProUGUI gearEquipStatusTMP;
    public InventorySlotUI gearEquipSlotHighlighted;
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
        if (gearEquipSlotHighlighted.gearInstance.gearSO != null) 
        return;

        else if (isEquipping)
        {
            var inventorySlot = menuGearInventorySubPage.inventorySlotSelected;
            int i = equipSlots.IndexOf(gearEquipSlotHighlighted);

            menuGearInventorySubPage.playerInventory.EquipGearToSlot(inventorySlot.gearInstance, equipSlots.IndexOf(gearEquipSlotSelected));

            if (inventorySlot.gearInstance.gearSO is EquipmentSO equipment)
            {
                inventorySlot.itemQuantityTMP.text = equipment.Potential + "%";
            }

            else if (inventorySlot.gearInstance.gearSO is ConsumbableSO consumable)
            {
                inventorySlot.itemQuantityTMP.text = "x" + consumable.quantityAvailable;
            }

            InitialiseEquipSlots();
            gearEquipSlotHighlighted = equipSlots[i];
            firstButtonToSelect = gearEquipSlotHighlighted.button;


            menuGearPageSelection.displayContainer.SetActive(true);
            equipPageHeaderGO.SetActive(false);
            menuGearPageSelection.lastParentButtonSelected = menuGearPageSelection.equippedHighlightedButton;

            menuGearPageSelection.lastParentButtonSelected.button.onClick.Invoke();
            menuGearPageSelection.inventoryHighlightedButton.SetButtonNormalColor(Color.white);
            menuGearInventorySubPage.InstantiateUIInventorySlots();

            isEquipping = false;
        }

        else
        {
            EventSystem.current.SetSelectedGameObject(null);
            ExitMenu();
            firstButtonToSelect = gearEquipSlotHighlighted.GetComponent<Button>();
            menuGearPageSelection.inventoryHighlightedButton.button.Select();
            menuGearPageSelection.inventoryHighlightedButton.button.onClick.Invoke();
        }
    }

    public string ItemQuantityRemaining(GearSO gearSO)
    {
        string itemQuantity = "";

        if (gearSO is EquipmentSO equipment)
        {
            itemQuantity = equipment.Potential + "%";
        }

        else if (gearSO is ConsumbableSO consumable)
        {
            itemQuantity = "x " + consumable.quantityAvailable;
        }

        else itemQuantity = null;

        return itemQuantity;
    }

    public override void EnterMenu()
    {
        DisplayMenu(true);
        if (!isEquipping)
            firstButtonToSelect = equipSlotButtons[0];

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

            equipSlot.onHighlighted = () =>
            {
                EquipSlotHighlighted(equipSlot);
            };

            equipSlot.onUnHighlighted = () =>
            {
                //
            };

            equipSlot.itemQuantityTMP.text = "";

            if (gearInstanceEquipped[i] == null || gearInstanceEquipped[i].gearSO == null)
            {
                equipSlot.itemNameTMP.text = "GEAR SLOT " + (i + 1) + ": " + "EMPTY";
            }

            else
            {
                equipSlot.gearInstance = gearInstanceEquipped[i];
                equipSlot.itemNameTMP.text = equipSlot.gearInstance.gearSO.gearName;

                equipSlot.itemQuantityTMP.text = ItemQuantityRemaining(equipSlot.gearInstance);

                string ItemQuantityRemaining(GearInstance gearInstance)
                {
                    if (gearInstance is EquipmentInstance equipmentInstance)
                        return ": " + equipmentInstance.charge + "%";
                    if (gearInstance is ConsumableInstance consumableInstance)
                        return "x " + consumableInstance.quantityAvailable;
                    return "";
                }

                bool isEquipment = equipSlot.gearInstance is EquipmentInstance;
            }

            equipSlotButtons.Add(equipSlot.button);
            equipSlots.Add(equipSlot);
        }

        FieldEvents.SetGridNavigationWrapAround(equipSlotButtons, 5);
    }

    public void DeleteAllInventoryUI()
    {
        equipSlotButtons.Clear();
        equipSlots.Clear();

        for (int i = equipSlotsParent.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(equipSlotsParent.transform.GetChild(i).gameObject);
        }
    }

    public void EquipSlotHighlighted(InventorySlotUI gearEquipSlot)
    {
        gearEquipSlotHighlighted = gearEquipSlot;

        if (gearEquipSlot.gearInstance.gearSO == null)
        {
            gearDescriptionTMP.text = "Slot free";
            gearTypeTMP.text = "";
            gearEquipStatusTMP.text = "";
            gearValueTMP.text = "";
        }

        else
        
        {
            if (gearEquipSlot.gearInstance is EquipmentInstance)
            {
                gearTypeTMP.text = "Type: Equipment";
            }

            else
            {
                gearTypeTMP.text = "Type: Consumable";
            }

            if (gearEquipSlot.gearInstance.isCurrentlyEquipped)
            {
                gearDescriptionTMP.text = gearEquipSlot.gearInstance.gearSO.gearDescription;
                gearEquipStatusTMP.text = "Currently Equipped. PRESS CTRL TO REMOVE";
            }
            else
            {
                gearDescriptionTMP.text = gearEquipSlot.gearInstance.gearSO.gearDescription;
            }

            gearValueTMP.text = "$MAMS: " + gearEquipSlot.gearInstance.gearSO.value.ToString();
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
            ExitMenu();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
           if (gearEquipSlotHighlighted.gearInstance != null)
            {
                menuGearInventorySubPage.UnequipHighlightedGearInstance(gearEquipSlotHighlighted.gearInstance);
                int i = equipSlots.IndexOf(gearEquipSlotHighlighted);
                InitialiseEquipSlots();
                equipSlots[i].button.Select();
            }
        }
    }
}
