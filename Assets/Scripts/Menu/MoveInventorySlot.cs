using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveInventorySlot : MoveSlot
{
    public override void UpdateMoveDescriptionText()
    {
        if (moveSO != null)
        {
            movePropertyTMP.text = "SELECT TO EQUIP TO SLOT";
            moveDescriptions.text = moveSO.MoveDescription;

            if (moveSO.IsFlaw)
            {
                movePropertyTMP.text = "FLAW.<br>CANNOT UNASSIGN";
            }

            if (moveSO.isEquipped && !moveSO.IsFlaw)
            {
                movePropertyTMP.text = "CURRENTLY EQUIPPED";
                moveDescriptions.text = moveSO.MoveDescription;
            }
        }

        if (moveSO == null)

        {
            movePropertyTMP.text = "";
            moveDescriptions.text = "";
        }
    }
}
