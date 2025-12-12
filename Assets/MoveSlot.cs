using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class MoveSlot : MonoBehaviour, ISelectHandler
{
    public MoveSO moveSO;
    public TextMeshProUGUI slotText;
    public MenuSlotSelect menuSlotSelect;
    public MenuMoves menuMoves;

    void ISelectHandler.OnSelect(BaseEventData eventData)
    {
        UpdateMoveDescriptionText();
    }

    public virtual void UpdateMoveDescriptionText()
    {
       if (moveSO != null)
       {
           if (moveSO.IsFlaw)
           {
                menuMoves.movePropertyTMP.text = "Flaw - Cannot unassign";
                menuMoves.moveDescriptions.text = moveSO.MoveDescription;
           }
       
           else
           {
                menuMoves.movePropertyTMP.text = "Press CTRL to unassign";
                menuMoves.moveDescriptions.text = moveSO.MoveDescription;
           }
       }
      
        if (moveSO == null)
        {
            menuMoves.moveDescriptions.text = "";
            menuMoves.movePropertyTMP.text = "Select to assign a move";
        }
    }
}
