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
    public InventorySlot inventorySlotHighlighted;
    public GameObject timeDisplayGO;
    public GameObject smamsDisplayGO;
    public GameObject UIFieldMenuGearSlotPrefab;
    public GameObject inventorySlotsParent;
    public Dictionary<GearSO, InventorySlot> gearToSlot = new Dictionary<GearSO, InventorySlot>();
    bool initialized = false;

    private void Start()
    {
        playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>().playerInventory;
    }

    public override void DisplayMenu(bool on)
    {
        displayContainer.SetActive(on);
    }

    public void InstantiateUIGearSlots()
    {
        foreach (GearSO gear in playerInventory.inventorySO.gearInventory)
        {
            if (!gearToSlot.ContainsKey(gear))
            {
                GameObject UIgearSlot = Instantiate(UIFieldMenuGearSlotPrefab);
                UIgearSlot.transform.SetParent(inventorySlotsParent.transform, false);

                InventorySlot inventorySlot = UIgearSlot.GetComponent<InventorySlot>();
                inventorySlot.gear = gear;
                inventorySlot.menuGear = this;
                inventorySlot.menuManagerUI = menuManagerUI;

                inventorySlot.itemName.text = gear.gearName;
                inventorySlot.itemQuantity.text = "x" + gear.quantityInInventory;

                float alpha = gear.isCurrentlyEquipped ? 0.5f : 1f;
                menuManagerUI.SetTextAlpha(inventorySlot.itemName, alpha);
                menuManagerUI.SetTextAlpha(inventorySlot.itemQuantity, alpha);

                inventorySlot.button.onClick.AddListener(() => InventorySlotSelected(inventorySlot));

                UIgearSlot.name = gear.gearID;

                gearSlots.Add(inventorySlot);
                slotButtons.Add(inventorySlot.button);
                gearToSlot[gear] = inventorySlot;
            }
        }

        FieldEvents.SetGridNavigationWrapAround(slotButtons, 5);
    }

    public void DeleteAllInventoryUI()
    {
        gearSlots.Clear();
        slotButtons.Clear();
        gearToSlot.Clear();

        for (int i = inventorySlotsParent.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(inventorySlotsParent.transform.GetChild(i).gameObject);
        }
    }

    public override void EnterMenu()
    {
        menuManagerUI.DisplayMenuContainer(this);

        if (initialized == false)
        {
            initialized = true;
            InstantiateUIGearSlots();
        }

        if (firstButtonToSelect == null) { firstButtonToSelect = gearSlots[0].button; }

        menuButtonHighlighted.SetButtonColor(menuButtonHighlighted.highlightedColor);
        menuButtonHighlighted.enabled = false;
        firstButtonToSelect.Select();
        gearPropertiesDisplayGO.SetActive(true);
    }

    public override void ExitMenu()
    {
        initialized = false;
        menuButtonHighlighted.enabled = true;
        menuButtonHighlighted.SetButtonColor(Color.white);
        DeleteAllInventoryUI();
        menuManagerUI.EnterMenu(menuManagerUI.main);
        mainButtonToRevert.Select();
    }

    public void InventorySlotSelected(InventorySlot inventorySlot)
    {
        menuGearEquip.transplantingSlot = null;
        menuGearEquip.inventorySlotSelected = inventorySlot;

        if (inventorySlot.gear.isCurrentlyEquipped)
        {
            menuGearEquip.transplantingSlot = inventorySlot;
        }

        firstButtonToSelect = inventorySlot.button;
        menuManagerUI.EnterMenu(menuManagerUI.gearEquipPage);
    }

    public void GearSlotHighlighted(InventorySlot inventorySlot)
    {
        gearValueTMP.text = "Value: " + inventorySlot.gear.value.ToString() + " $MAMS";
        inventorySlotHighlighted = inventorySlot;

        if (!inventorySlot.gear.isConsumable)
        {
            gearTypeTMP.text = "Type: Accessory";
        }

        else
        {
            gearTypeTMP.text = "Type: Consumable";
        }

        if (inventorySlot.gear.isCurrentlyEquipped)
        {
            gearDescriptionTMP.text = inventorySlot.gear.gearDescription;
            gearEquipStatusTMP.text = "Equipped to Slot " + (playerInventory.inventorySO.equippedGear.IndexOf(inventorySlot.gear) + 1) + ". PRESS CTRL TO REMOVE";
        }
        else
        {
            gearDescriptionTMP.text = inventorySlot.gear.gearDescription;
            gearEquipStatusTMP.text = "SELECT to equip";
        }
    }

    public void UnequipHighlightedGear(GearSO gear)
    {
        playerInventory.UnequipGearFromSlot(gear);

        var slot = gearToSlot[gear];
        gearEquipStatusTMP.text = "Unequipped";
        menuManagerUI.SetTextAlpha(slot.itemName, 1f);
        menuManagerUI.SetTextAlpha(slot.itemQuantity, 1f);
        slot.itemQuantity.text = "x" + gear.quantityInInventory.ToString();
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitMenu();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (inventorySlotHighlighted.gear.isCurrentlyEquipped)
            {
                DisplayMenu(true);
                gearPropertiesDisplayGO.SetActive(true);
                GearSlotHighlighted(inventorySlotHighlighted);
                UnequipHighlightedGear(inventorySlotHighlighted.gear);
            }
        }
    }
}
