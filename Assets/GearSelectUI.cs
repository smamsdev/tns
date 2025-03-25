using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class GearSelectUI : MonoBehaviour
{
    public Button gearSlot1Button;
    [SerializeField] CombatManager combatManager;
    public Button[] buttonsToDisable;
    [SerializeField] GameObject firstMoveContainer;
    PlayerInventory playerInventory;

    int gearToBeEquipped;
    public TextMeshProUGUI[] gearSlotDisplay;

    private void OnEnable()
    {
        CombatEvents.GearSlotButtonHighlighted += ButtonHighlighted;
    }

    private void OnDisable()
    {
        CombatEvents.GearSlotButtonHighlighted -= ButtonHighlighted;
    }

    private void Start()
    {
        playerInventory = combatManager.player.GetComponentInChildren<PlayerInventory>();

      //  for (int i = 0; i < playerInventory.equippedSlot.equippedSlot.Length; i++)
      //  {
      //      if (equippedGear.equippedSlot[i] != null)
      //      {
      //          gearSlotDisplay[i].text = equippedGear.equippedSlot[i].name;
      //      }
      //
      //      else gearSlotDisplay[i].text = "Slot Free";
      //  }
    }

    public void ShowGearSelectionMenu()
    {
        for (int i = 0; i < buttonsToDisable.Length; i++)
        {
            buttonsToDisable[i].interactable = false;
        }
        //wtf is this
    }

    public void UpdateGearDisplay(int gearSlotToUpdate, string newlyEquippedGear)
    {
        gearSlotDisplay[gearSlotToUpdate].text = newlyEquippedGear;
        EnableFirstMoveButtons();
    }

    public void EnableFirstMoveButtons()
    {
        for (int i = 0; i < buttonsToDisable.Length; i++)
        {
            buttonsToDisable[i].interactable = true;
        }

    }

    void ButtonHighlighted(int gearSlot)
    {
      //  if (equippedGear.equippedSlot[gearSlot] != null)
      //  {
      //      CombatEvents.UpdateNarrator(equippedGear.equippedSlot[gearSlot].GetComponent<Gear>().gearDescription);
      //  }
      //
      //  CombatEvents.UpdateNarrator("Select Item");
    }
}

