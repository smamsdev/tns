using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class MoveSlot : MonoBehaviour, ISelectHandler
{
    public MoveSO moveSO;
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
       if (moveSO != null)
       {
           if (moveSO.IsFlaw)
           {
               movePropertyTMP.text = "Flaw - Cannot unassign";
               moveDescriptions.text = moveSO.MoveDescription;
           }
       
           else
           {
               movePropertyTMP.text = "Press CTRL to unassign";
               moveDescriptions.text = moveSO.MoveDescription;
           }
       }

        if (moveSO == null)
        {
            moveDescriptions.text = "";
            movePropertyTMP.text = "Select to assign a move";
        }
    }
}
