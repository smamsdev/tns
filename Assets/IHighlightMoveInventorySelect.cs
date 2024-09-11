using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class IHighlightMoveInventorySelect : MonoBehaviour, ISelectHandler
{
    public MoveSlot moveSlot;
    public TextMeshProUGUI moveInventoryDescriptionTMP;
    public TextMeshProUGUI moveInventoryPropertyTMP;

    private void OnEnable()
    {
        moveSlot = GetComponent<MoveSlot>();
    }

    public void OnSelect(BaseEventData eventData)
    {
        UpdateDescriptionText();
    }

    public void UpdateDescriptionText()
    {
        moveInventoryDescriptionTMP.text = moveSlot.move.moveDescription;

        if (moveSlot.move.isFlaw)

        {
            moveInventoryPropertyTMP.text = "Flaw: Cannot Assign";
            return;
        }

        if (moveSlot.move.isEquipped)
        {
            moveInventoryPropertyTMP.text = "Currently Equipped";
        }

        if (!moveSlot.move.isEquipped)
        {
            moveInventoryPropertyTMP.text = "Select to Assign Move";
        }


    }
}
