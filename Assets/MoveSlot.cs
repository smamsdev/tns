using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class MoveSlot : MonoBehaviour, ISelectHandler
{
    public MoveSO move;
    public TextMeshProUGUI slotText;
    public TextMeshProUGUI movePropertyTMP;
    public TextMeshProUGUI moveDescriptions;
    public MenuSlotSelect menuSlotSelect;

    void ISelectHandler.OnSelect(BaseEventData eventData)
    {
        UpdateMoveDescriptionText();
    }

    public virtual void UpdateMoveDescriptionText()
    {
        Debug.Log("adasd");
       //if (move != null)
       //{
       //    if (move.isFlaw)
       //    {
       //        movePropertyTMP.text = "Flaw - Cannot unassign";
       //        moveDescriptions.text = move.moveDescription;
       //    }
       //
       //    else
       //    {
       //        movePropertyTMP.text = "Press CTRL to unassign";
       //        moveDescriptions.text = move.moveDescription;
       //    }
       //}

        if (move == null)
        {
            moveDescriptions.text = "";
            movePropertyTMP.text = "Select to assign a move";
        }
    }

}
