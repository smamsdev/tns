using UnityEngine;
using System.Collections.Generic;

public class PlayerMoveInventorySO : ScriptableObject
{
    public enum MoveType
    { ViolentAttack, ViolentFend, ViolentFocus, CautiousAttack, CautiousFend, CautiousFocus, PreciseAttack, PreciseFend, PreciseFocus }

    public bool isFlawReassignmentEnabled;

    public List<MoveSO> violentAttacksInventory = new List<MoveSO>();
    public List<MoveSO> violentFendsInventory = new List<MoveSO>();
    public List<MoveSO> violentFocusesInventory = new List<MoveSO>();

    public List<MoveSO> cautiousAttacksInventory = new List<MoveSO>();
    public List<MoveSO> cautiousFendsInventory = new List<MoveSO>();
    public List<MoveSO> cautiousFocusesInventory = new List<MoveSO>();

    public List<MoveSO> preciseAttacksInventory = new List<MoveSO>();
    public List<MoveSO> preciseFendsInventory = new List<MoveSO>();
    public List<MoveSO> preciseFocusesInventory = new List<MoveSO>();

    public MoveSO[] violentAttacksEquipped = new MoveSO[5];
    public MoveSO[] violentFendsEquipped = new MoveSO[5];
    public MoveSO[] violentFocusesEquipped = new MoveSO[5];

    public MoveSO[] cautiousAttacksEquipped = new MoveSO[5];
    public MoveSO[] cautiousFendsEquipped = new MoveSO[5];
    public MoveSO[] cautiousFocusesEquipped = new MoveSO[5];

    public MoveSO[] preciseAttacksEquipped = new MoveSO[5];
    public MoveSO[] preciseFendsEquipped = new MoveSO[5];
    public MoveSO[] preciseFocusesEquipped = new MoveSO[5];

    public MoveSO[][] allEquippedMoveArrays;

    public List<MoveSO> GetMoveInventoryListOfType(MoveType moveType)
    {
        switch (moveType)
        {
            case MoveType.ViolentAttack:
                return violentAttacksInventory;

            case MoveType.ViolentFend:
                return violentFendsInventory;

            case MoveType.ViolentFocus:
                return violentFocusesInventory;

            case MoveType.CautiousAttack:
                return cautiousAttacksInventory;

            case MoveType.CautiousFend:
                return cautiousFendsInventory;

            case MoveType.CautiousFocus:
                return cautiousFocusesInventory;

            case MoveType.PreciseAttack:
                return preciseAttacksInventory;

            case MoveType.PreciseFend:
                return preciseFendsInventory;

            case MoveType.PreciseFocus:
                return preciseFocusesInventory;

            default:
                Debug.Log("something went wrong");
                return null;
        }
    }

    public MoveSO[] GetEquippedArrayOfType(MoveType moveType)
    {
        switch (moveType)
        {
            case MoveType.ViolentAttack:
                return violentAttacksEquipped;

            case MoveType.ViolentFend:
                return violentFendsEquipped;

            case MoveType.ViolentFocus:
                return violentFocusesEquipped;

            case MoveType.CautiousAttack:
                return cautiousAttacksEquipped;

            case MoveType.CautiousFend:
                return cautiousFendsEquipped;

            case MoveType.CautiousFocus:
                return cautiousFocusesEquipped;

            case MoveType.PreciseAttack:
                return preciseAttacksEquipped;

            case MoveType.PreciseFend:
                return preciseFendsEquipped;

            case MoveType.PreciseFocus:
                return preciseFocusesEquipped;

            default:
                Debug.Log("something went wrong");
                return null;
        }
    }

    public void EquipMoveToSlot(MoveType movetype, int slotIndex, MoveSO moveSO)
    {
        GetEquippedArrayOfType(movetype)[slotIndex] = moveSO;
        moveSO.isEquipped = true;
    }

    public void UnequipMoveFromSlot(MoveType movetype, MoveSO moveSO)
    {
        var EquipArray = GetEquippedArrayOfType(movetype);
        int slotIndex = System.Array.IndexOf(EquipArray, moveSO);

        EquipArray[slotIndex] = null;
        moveSO.isEquipped = false;
    }
}