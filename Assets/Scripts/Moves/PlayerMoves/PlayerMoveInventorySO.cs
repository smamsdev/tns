using UnityEngine;
using System.Collections.Generic;

public class PlayerMoveInventorySO : ScriptableObject
{
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