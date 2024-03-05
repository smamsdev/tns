using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class EquippedGearDisplayUI : MonoBehaviour
{
    [SerializeField] Button gearSlot1Button;
    [SerializeField] EquippedGear equippedGear;

    public Button[] buttonsToDisable;
    [SerializeField] GameObject firstMoveContainer;

    int gearToBeEquipped;

    public TextMeshProUGUI[] gearSlotDisplay;


    private void Start()
    {
        for (int i = 0; i < equippedGear.equippedSlot.Length; i++)
        {
            if (equippedGear.equippedSlot[i] != null)
            {
                gearSlotDisplay[i].text = equippedGear.equippedSlot[i].name;
            }

            else gearSlotDisplay[i].text = "Empty";
        }
    }

    public void GearSelectionMenu()

    {
        for (int i = 0; i < buttonsToDisable.Length; i++)
        {
            buttonsToDisable[i].interactable = false;
        }

        gearSlot1Button.Select();
    }

    public void UpdateGearDisplay(int gearSlotToUpdate, string newlyEquippedGear)

    {
        gearSlotDisplay[gearSlotToUpdate].text = newlyEquippedGear;
        firstMoveContainer.SetActive(false);

        for (int i = 0; i < buttonsToDisable.Length; i++)
        {
            buttonsToDisable[i].interactable = true;
        }

    }

}

