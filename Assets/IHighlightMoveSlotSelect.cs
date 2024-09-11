using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class IHighlightMoveSlotSelect : MonoBehaviour, ISelectHandler
{
    public MenuSlotSelect menuSlotSelect;
    public MoveSlot moveSlot;
    public TextMeshProUGUI moveDescriptions;

    private void OnEnable()
    {
        moveSlot = GetComponent<MoveSlot>();
    }

    public void OnSelect(BaseEventData eventData)
    {
        menuSlotSelect.moveSlotGOHighlighted = this.gameObject;
        UpdateDescriptionText();
    }

    public void UpdateDescriptionText()
    {
        if (moveSlot.move != null)
        {
            menuSlotSelect.DisplayFlawText(moveSlot.move.isFlaw);
            moveDescriptions.text = moveSlot.move.moveDescription;
        }

        if (moveSlot.move == null)
        {
            moveDescriptions.text = "Assign a move";
            menuSlotSelect.DisplayFlawText(false);
        }
    }
}
