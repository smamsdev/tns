using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class MenuGearEquip : Menu
{
    [SerializeField] Button firstButtonToSelect;
    public InventorySlot inventorySlotSelected;
    public MenuGear menuGear;
    public GearEquipSlot[] gearEquipSlots;

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
            gearEquipSlot.gameObject.SetActive(false);
        }

        for (int i = 0; i < menuGear.playerInventory.inventorySO.equipSlotString.Count; i++)

            if (menuGear.playerInventory.equippedInventory[i] == null)
            {
                gearEquipSlots[i].buttonTMP.text = "EQUIPPED SLOT " + i + ": "+ "EMPTY";
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

    //public void UpdateGearDescriptionField(GearEquipSlot gearEquipSlot)
    //{
    //    if (!gearEquipSlot.gearEquipped.isConsumable)
    //    {
    //        gearEquipSlot.itemTypeTMP.text = "Equipment";
    //    }
    //
    //    else
    //    {
    //        itemTypeTMP.text = "Consumable";
    //    }
    //
    //    if (gearEquipSlot.gearEquipped.isCurrentlyEquipped)
    //    {
    //        descriptionFieldTMP.text = "Currently Equipped.\n" + gear.gearDescription;
    //    }
    //    else
    //    {
    //        descriptionFieldTMP.text = gear.gearDescription;
    //    }
    //}

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
    }
}
