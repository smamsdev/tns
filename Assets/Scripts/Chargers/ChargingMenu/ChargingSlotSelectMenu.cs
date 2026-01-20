using System.Collections.Generic;
using UnityEngine;

public class ChargingSlotMenu : ChargingMenu
{
    public InventorySlotUI[] inventorySlotUIs;

    public override void DisplayMenu(bool on)
    {
        displayContainer.SetActive(on);
    }

    public override void EnterMenu()
    {
        inventorySlotUIs[0].button.Select();
    }

    public void InitialiseEquipSlots()
    {
        var chargingSlots = chargingMenuManager.chargingMainMenu.chargerSO.chargingSlots;

        for (int i = 0; i < chargingSlots.Length; i++)
        {
            var slot = inventorySlotUIs[i]; // cache slot to avoid closure issues

            slot.onHighlighted = () =>
            {
                ChargingSlotHighlighted(slot);
                chargingMenuManager.chargingMainMenu.SetEquipSlotColor(slot, Color.yellow);
            };

            slot.onUnHighlighted = () =>
            {
                chargingMenuManager.chargingMainMenu.SetEquipSlotColor(slot, Color.white);
            };

            if (chargingSlots[i].gearSO == null)
            {
                slot.itemNameTMP.text = "GEAR SLOT " + (i + 1) + ": EMPTY";
            }
            else
            {
                slot.itemNameTMP.text = chargingSlots[i].gearSO.name;
                slot.itemQuantityTMP.text = chargingSlots[i].charge.ToString();
            }
        }
    }

    void ChargingSlotHighlighted(InventorySlotUI inventorySlotUI)
    {
        Debug.Log(inventorySlotUI);
    }

    public override void ExitMenu()
    {
        throw new System.NotImplementedException();
    }

    public override void StateUpdate()
    {
        //press esc do somth
    }
}
