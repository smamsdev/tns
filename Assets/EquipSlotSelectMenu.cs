using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EquipSlotSelectMenu : MonoBehaviour
{
    public CombatManager combatManager;
    PlayerInventory playerInventory;
    public List<UIGearEquipSlot> uIGearEquipSlots = new List<UIGearEquipSlot>();
    public List<Button> uIGearEquipSlotButtons = new List<Button>();
    public UIGearEquipSlot equipSlotSelected;

    [SerializeField] Move equipGearMove; //player needs a move assigned to complete their turn

    public void DisplayEquipSlots()
    {
        playerInventory = combatManager.playerCombat.playerInventory;

        foreach (UIGearEquipSlot gearEquipSlot in uIGearEquipSlots)
        {
            gearEquipSlot.gearEquipped = null;
            gearEquipSlot.gameObject.SetActive(false);
        }

        for (int i = 0; i < playerInventory.inventorySO.equipSlotsAvailable; i++)
        {
            if (playerInventory.inventorySO.equippedGear[i] == null)
            {
                uIGearEquipSlots[i].buttonTMP.text = "SLOT " + (i + 1) + ": " + "EMPTY";
                uIGearEquipSlots[i].gameObject.SetActive(true);
            }

            else
            {
                GearSO gearToLoad = playerInventory.inventorySO.equippedGear[i];
                uIGearEquipSlots[i].gearEquipped = gearToLoad;
                uIGearEquipSlots[i].buttonTMP.text = "SLOT " + (i + 1) + ": " + gearToLoad.gearName;
                uIGearEquipSlots[i].gameObject.SetActive(true);
            }
        }

        FieldEvents.SetGridNavigationWrapAround(uIGearEquipSlotButtons, playerInventory.inventorySO.equipSlotsAvailable);
    }

    public void EquipSlotSelected(UIGearEquipSlot gearEquipSlot)
    {
        equipSlotSelected = gearEquipSlot;
        combatManager.SetState(combatManager.gearSelectCombatState);
        CombatEvents.UpdateNarrator.Invoke("");
    }
}

