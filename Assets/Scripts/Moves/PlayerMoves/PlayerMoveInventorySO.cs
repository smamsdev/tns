using UnityEngine;
using System.Collections.Generic;

public class PlayerMoveInventorySO : ScriptableObject
{
    public enum MoveType
    { ViolentAttack, ViolentFend, ViolentFocus, CautiousAttack, CautiousFend, CautiousFocus, PreciseAttack, PreciseFend, PreciseFocus }

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

    public void BuildEquippedReferences()
    {
        allEquippedMoveArrays = new MoveSO[][]
        {
        violentAttacksEquipped,
        violentFendsEquipped,
        violentFocusesEquipped,

        cautiousAttacksEquipped,
        cautiousFendsEquipped,
        cautiousFocusesEquipped,

        preciseAttacksEquipped,
        preciseFendsEquipped,
        preciseFocusesEquipped
        };
    }
 
    public List<MoveSO> GetMoveListOfType(MoveType moveType)
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

    public void EquipMoveToSlot(MoveSO[] equippedMoveArrayOfType, int moveEquipSlot, MoveSO moveSO)
    {
        equippedMoveArrayOfType[moveEquipSlot] = moveSO;
    }

    public void UnequipMove(MoveSO moveSO)
    {
        BuildEquippedReferences();

        for (int i = 0; i < allEquippedMoveArrays.Length; i++)
        {
            int index = System.Array.IndexOf(allEquippedMoveArrays[i], moveSO);

            if (index != -1)
            {
                MoveSO[] equippedMoveArray = allEquippedMoveArrays[i];
                int equippedSlot = index;

                equippedMoveArray[equippedSlot] = null;
                moveSO.isEquipped = false;

                return; 
            }
        }

        Debug.Log($"{moveSO.name} unable to locate.");
    }
}