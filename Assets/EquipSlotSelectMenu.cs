using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EquipSlotSelectMenu : MonoBehaviour
{
    public CombatManager combatManager;
    PlayerInventorySO playerInventorySO;
    //public List<UIGearEquipSlot> uIGearEquipSlots = new List<UIGearEquipSlot>();
    public List<Button> uIGearEquipSlotButtons = new List<Button>();
    //public UIGearEquipSlot equipSlotSelected;

    public void DisplayEquipSlots()
    {
        Debug.Log("redo this whole thing");
      //  playerInventorySO = combatManager.playerCombat.playerInventorySO;
      //
      //  foreach (UIGearEquipSlot gearEquipSlot in uIGearEquipSlots)
      //  {
      //      gearEquipSlot.gearEquipped = null;
      //      gearEquipSlot.gameObject.SetActive(false);
      //  }
      //
      //  for (int i = 0; i < playerInventorySO.equipSlotsAvailable; i++)
      //  {
      //      if (playerInventorySO.equippedGearInstances[i] == null)
      //      {
      //          uIGearEquipSlots[i].buttonTMP.text = "SLOT " + (i + 1) + ": " + "EMPTY";
      //          uIGearEquipSlots[i].gameObject.SetActive(true);
      //      }
      //
      //      else
      //      {
      //          GearSO gearToLoad = playerInventorySO.equippedGearInstances[i];
      //          uIGearEquipSlots[i].gearEquipped = gearToLoad;
      //          uIGearEquipSlots[i].buttonTMP.text = "SLOT " + (i + 1) + ": " + gearToLoad.gearName;
      //          uIGearEquipSlots[i].gameObject.SetActive(true);
      //      }
      //  }

        FieldEvents.SetGridNavigationWrapAround(uIGearEquipSlotButtons, playerInventorySO.gearInstanceEquipped.Count);
    }

    //public void EquipSlotSelected(UIGearEquipSlot gearEquipSlot)
    //{
    //    //equipSlotSelected = gearEquipSlot;
    //    //combatManager.SetState(combatManager.gearSelectCombatState);
    //    //CombatEvents.UpdateNarrator.Invoke("");
    //}
}

