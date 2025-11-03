using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerMoveManager : MonoBehaviour
{
    public int firstMoveIs;
    public int secondMoveIs;
    public PlayerMoveInventorySO playerMoveInventorySO;

    [SerializeField] GameObject 
    ViolentAttacksFolder, VioletFendsFolder, ViolentFocusesFolder, 
    CautiousAttacksFolder, CautiousFendsFolder, CautiousFocusesFolder, 
    PreciseAttacksFolder, PreciseFendsFolder, PreciseFocusesFolder;

    public PlayerCombat playerCombat;

    public Move[] violentAttackSlots = new Move[5];
    public Move[] violentFendSlots =  new Move[5];
    public Move[] violentFocusSlots = new Move[5];

    public Move[] cautiousAttackSlots = new Move[5];
    public Move[] cautiousFendSlots = new Move[5];
    public Move[] cautiousFocusSlots = new Move[5];

    public Move[] preciseAttackSlots = new Move[5];
    public Move[] preciseFendSlots = new Move[5];
    public Move[] preciseFocusSlots = new Move[5];

    public void InstantiateEquippedMoves()
    {
        foreach (MoveSO moveSO in playerMoveInventorySO.violentAttacksInventory)
        {
            GameObject moveInstanceGO = Instantiate(moveSO.movePrefab);
            moveInstanceGO.name = moveSO.moveName;
            moveInstanceGO.transform.SetParent(ViolentAttacksFolder.transform, false);
            Move moveInstance = moveInstanceGO.GetComponent<Move>();
            moveSO.moveInstance = moveInstance;
        }
    }

    public void CombineStanceAndMove()
    {
        switch (firstMoveIs)
        {
            case 0:
                Debug.Log("set gear as a move, update this");
                break;

            case 1: // Violent stance
                switch (secondMoveIs)
                {
                    case 1: SelectMove(playerMoveInventorySO.violentAttacksEquipped); break;
                    case 2: SelectMove(playerMoveInventorySO.violentFendsEquipped); break;
                    case 3: SelectMove(playerMoveInventorySO.violentFocusesEquipped); break;
                }
                break;

            case 2: // Cautious stance
                switch (secondMoveIs)
                {
                    case 1: SelectMove(playerMoveInventorySO.cautiousAttacksEquipped); break;
                    case 2: SelectMove(playerMoveInventorySO.cautiousFendsEquipped); break;
                    case 3: SelectMove(playerMoveInventorySO.cautiousFocusesEquipped); break;
                }
                break;

            case 3: // Precise stance
                switch (secondMoveIs)
                {
                    case 1: SelectMove(playerMoveInventorySO.preciseAttacksEquipped); break;
                    case 2: SelectMove(playerMoveInventorySO.preciseFendsEquipped); break;
                    case 3: SelectMove(playerMoveInventorySO.preciseFocusesEquipped); break;
                }
                break;
        }
    }

    void SelectMove(MoveSO[] EquippedMoveList)
    {
        int moveWeightingTotal = 0;

        foreach (var move in EquippedMoveList)
        {
            if (move != null && move.moveWeighting > 0)
            {
                moveWeightingTotal += move.moveWeighting;
            }
        }

        if (moveWeightingTotal == 0)
        {
            Debug.LogError("No valid moves available to select!!");
            return;
        }

        int randomValue = UnityEngine.Random.Range(1, moveWeightingTotal + 1);

        foreach (var move in EquippedMoveList)
        {
            if (move == null || move.moveWeighting == 0) continue;

            if (randomValue > move.moveWeighting)
            {
                randomValue -= move.moveWeighting;
            }
            else
            {
                playerCombat.moveSOSelected = move;
                return;
            }
        }

        Debug.LogError("Failed to select a move! This should never happen. Random value was " + randomValue);
    }
}
