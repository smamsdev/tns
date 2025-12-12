using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveInventorySlot : MoveSlot
{
    public Button button;
    public MenuMoveInventory menuMoveInventory;

    public override void UpdateMoveDescriptionText()
    {
        if (moveSO != null)
        {
            menuMoveInventory.movePropertiesTMP.text = "SELECT TO EQUIP TO SLOT";
            menuMoveInventory.moveDescriptionTMP.text = moveSO.MoveDescription;

            if (moveSO.IsFlaw)
            {
          //      movePropertyTMP.text = "FLAW.<br>CANNOT UNASSIGN";
            }

            if (moveSO.isEquipped && !moveSO.IsFlaw)
            {
                menuMoveInventory.movePropertiesTMP.text = "CURRENTLY EQUIPPED";
                menuMoveInventory.moveDescriptionTMP.text = moveSO.MoveDescription;
            }
        }

        if (moveSO == null)

        {
            Debug.Log("this should never be null");
        }
    }
}
