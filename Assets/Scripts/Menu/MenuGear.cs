using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class MenuGear : Menu
{
    public Button firstButtonToSelect;
    public PlayerInventory playerInventory;
    public GameObject gearPropertiesDisplayGO;
    public List<InventorySlot> gearSlots = new List<InventorySlot>();
    List<Button> slotButtons = new List<Button>();
    public MenuGearEquip menuGearEquip;
    public TextMeshProUGUI gearDescriptionTMP;
    public TextMeshProUGUI gearTypeTMP;
    public TextMeshProUGUI gearValueTMP;
    public TextMeshProUGUI gearEquipStatusTMP;
    public GearSO gearHighlighted;
    public GameObject timeDisplayGO;
    public GameObject smamsDisplayGO;
    public GameObject UIFieldMenuGearSlotPrefab;
    public GameObject inventorySlotsParent;

    public override void DisplayMenu(bool on)
    {
        if (on)
        {
            gearPropertiesDisplayGO.SetActive(false);
            var player = GameObject.FindGameObjectWithTag("Player");
            playerInventory = player.GetComponentInChildren<PlayerInventory>();
            InstantiateUIGearSlots();
        }

        displayContainer.SetActive(on);
    }

    void InstantiateUIGearSlots()
    {
        HashSet<GearSO> gearSet = new HashSet<GearSO>();
        gearSlots.Clear();
        slotButtons.Clear();

        foreach (GearSO gear in playerInventory.inventorySO.gearInventory)
        {
            if (gearSet.Add(gear))
            {
                GameObject UIgearSlot = Instantiate(UIFieldMenuGearSlotPrefab);
                UIgearSlot.transform.SetParent(inventorySlotsParent.transform);
                InventorySlot inventorySlot = UIgearSlot.GetComponent<InventorySlot>();
                gearSlots.Add(inventorySlot);
                inventorySlot.gear = gear;
                inventorySlot.itemName.text = gear.gearName;
                inventorySlot.itemQuantity.text = " x " + gear.quantityInInventory;
                inventorySlot.menuGear = this;
                inventorySlot.menuManagerUI = menuManagerUI;
                menuManagerUI.SetTextAlpha(inventorySlot.itemName, gear.isCurrentlyEquipped ? 0.5f : 1f);
                menuManagerUI.SetTextAlpha(inventorySlot.itemQuantity, gear.isCurrentlyEquipped ? 0.5f : 1f);
                UIgearSlot.name = inventorySlot.gear.gearID;

                slotButtons.Add(inventorySlot.button);
                FieldEvents.SetGridNavigationWrapAround(slotButtons, 5);
            }
        }
    }

    public override void EnterMenu()
    {
        if (firstButtonToSelect == null) { firstButtonToSelect = gearSlots[0].button; }

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
