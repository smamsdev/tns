using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveInventorySlot : MoveSlot
{
    public override void UpdateMoveDescriptionText()
    {
        if (move != null)
        {
            movePropertyTMP.text = "SELECT TO EQUIP TO SLOT";
            moveDescriptions.text = move.moveDescription;

            if (move.isFlaw)
            {
                movePropertyTMP.text = "FLAW.<br>CANNOT UNASSIGN";
            }

            if (move.isEquipped && !move.isFlaw)
            {
                movePropertyTMP.text = "CURRENTLY EQUIPPED";
                moveDescriptions.text = move.moveDescription;
            }
        }

        if (move == null)

        {
            movePropertyTMP.text = "";
            moveDescriptions.text = "";
        }
    }
}
