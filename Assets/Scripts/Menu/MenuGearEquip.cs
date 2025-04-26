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
    public GearEquipSlot[] gearEquipSlots;
    public TextMeshProUGUI itemTypeTMP;
    public TextMeshProUGUI itemvalue;
    public TextMeshProUGUI descriptionFieldTMP;
    GearEquipSlot gearEquipSlotHighlighted;
    public TextMeshProUGUI pageHeaderTMP;

    private void OnEnable()
    {
        DisplayMenu(false);
    }

    public override void DisplayMenu(bool on)
    {
        displayContainer.SetActive(on);
    }

    public void EquipSlotSelected(GearEquipSlot gearEquipSlotSelected)
    {
        var geartoEquip = inventorySlotSelected.gear;

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
        foreach (GearEquipSlot gearEquipSlot in gearEquipSlots)
        {
            gearEquipSlot.gearEquipped = null;
            gearEquipSlot.gameObject.SetActive(false);
        }

        for (int i = 0; i < menuGear.playerInventory.inventorySO.equipSlotString.Count; i++)

            if (menuGear.playerInventory.equippedInventory[i] == null)
            {
                gearEquipSlots[i].buttonTMP.text = "EQUIPPED SLOT " + (i + 1) + ": "+ "EMPTY";
                gearEquipSlots[i].gameObject.SetActive(true);
            }

            else
            {
                Gear gearToLoad = menuGear.playerInventory.equippedInventory[i].GetComponent<Gear>();
                gearEquipSlots[i].gearEquipped = gearToLoad;
                gearEquipSlots[i].buttonTMP.text = "EQUIPPED SLOT " + (i+1) + ": " + gearToLoad.gearID;
                gearEquipSlots[i].gameObject.SetActive(true);
            }
    }

    public void EquipSlotHighlighted(GearEquipSlot gearEquipSlot)
    {
        gearEquipSlotHighlighted = gearEquipSlot;

        if (gearEquipSlot.gearEquipped == null)
        {
            itemTypeTMP.text = "";
            descriptionFieldTMP.text = "Slot free.";
            itemvalue.text = "";
        }

        else
        
        {
            if (!gearEquipSlot.gearEquipped.isConsumable)
            {
                itemTypeTMP.text = "Equipment";
            }

            else
            {
                itemTypeTMP.text = "Consumable";
            }

            if (gearEquipSlot.gearEquipped.isCurrentlyEquipped)
            {
                descriptionFieldTMP.text = "Currently Equipped. PRESS CTRL TO REMOVE\n" + gearEquipSlot.gearEquipped.gearDescription;
            }
            else
            {
                descriptionFieldTMP.text = gearEquipSlot.gearEquipped.gearDescription;
            }

            itemvalue.text = gearEquipSlot.gearEquipped.value.ToString();
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
            ExitMenu();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
           if (gearEquipSlotHighlighted.gearEquipped != null)
            {
                menuGear.gearHighlighted = gearEquipSlotHighlighted.gearEquipped;
                menuGear.UnequipHighlightedGear();
                DisplayEquipSlots();

                itemTypeTMP.text = "";
                descriptionFieldTMP.text = "Slot free.";
                itemvalue.text = "";
            }
        }
    }
}
