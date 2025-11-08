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

    [SerializeField] List<Move> violentAttackInstances = new List<Move>();
    [SerializeField] List<Move> violentFendInstances =  new List<Move>();
    [SerializeField] List<Move> violentFocusInstances = new List<Move>();
    [SerializeField] List<Move> cautiousAttackInstances = new List<Move>();
    [SerializeField] List<Move> cautiousFendInstances = new List<Move>();
    [SerializeField] List<Move> cautiousFocusInstances =new List<Move>(); 
    [SerializeField] List<Move> preciseAttackInstances =new List<Move>(); 
    [SerializeField] List<Move> preciseFendInstances = new List<Move>();
    [SerializeField] List<Move> preciseFocusInstances = new List<Move>();

    public void InstantiateAllEquippedMoves()
    {
        InstantiateMovesOfType(playerMoveInventorySO.violentAttacksEquipped, ViolentAttacksFolder, violentAttackInstances);
        InstantiateMovesOfType(playerMoveInventorySO.violentFendsEquipped, VioletFendsFolder, violentFendInstances);
        InstantiateMovesOfType(playerMoveInventorySO.violentFocusesEquipped, ViolentFocusesFolder, violentFocusInstances);

        InstantiateMovesOfType(playerMoveInventorySO.cautiousAttacksEquipped, CautiousAttacksFolder, cautiousAttackInstances);
        InstantiateMovesOfType(playerMoveInventorySO.cautiousFendsEquipped, CautiousFendsFolder, cautiousFendInstances);
        InstantiateMovesOfType(playerMoveInventorySO.cautiousFocusesEquipped, CautiousFocusesFolder, cautiousFocusInstances);

        InstantiateMovesOfType(playerMoveInventorySO.preciseAttacksEquipped, PreciseAttacksFolder, preciseAttackInstances);
        InstantiateMovesOfType(playerMoveInventorySO.preciseFendsEquipped, PreciseFendsFolder, preciseFendInstances);
        InstantiateMovesOfType(playerMoveInventorySO.preciseFocusesEquipped, PreciseFocusesFolder, preciseFocusInstances);
    }

    void InstantiateMovesOfType(MoveSO[] equippedMovesOfType, GameObject moveTypeFolder, List<Move> equippedInstancesOfType)
    {
        foreach (MoveSO moveSO in equippedMovesOfType)
        {
            if (moveSO != null)
            {
                GameObject moveInstanceGO = Instantiate(moveSO.MovePrefab);
                moveInstanceGO.name = moveSO.MoveName;
                moveInstanceGO.transform.SetParent(moveTypeFolder.transform, false);
                Move moveInstance = moveInstanceGO.GetComponent<Move>();
                moveInstance.moveSO = moveSO;
                equippedInstancesOfType.Add(moveInstance);
            }
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
                    case 1: SelectMove(violentAttackInstances); break;
                    case 2: SelectMove(violentFendInstances); break;
                    case 3: SelectMove(violentFocusInstances); break;
                }
                break;

            case 2: // Cautious stance
                switch (secondMoveIs)
                {
                    case 1: SelectMove(cautiousAttackInstances); break;
                    case 2: SelectMove(cautiousFendInstances); break;
                    case 3: SelectMove(cautiousFocusInstances); break;
                }
                break;

            case 3: // Precise stance
                switch (secondMoveIs)
                {
                    case 1: SelectMove(preciseAttackInstances); break;
                    case 2: SelectMove(preciseFendInstances); break;
                    case 3: SelectMove(preciseFocusInstances); break;
                }
                break;
        }
    }

    void SelectMove(List<Move> moveInstancesOfType)
    {
        int MoveWeightingTotal = 0;

        foreach (Move move in moveInstancesOfType)
        {
            if (move != null && move.moveSO.MoveWeighting > 0)
            {
                MoveWeightingTotal += move.moveSO.MoveWeighting;
            }
        }

        if (MoveWeightingTotal == 0)
        {
            Debug.LogError("No valid moves available to select!!");
            return;
        }

        int randomValue = UnityEngine.Random.Range(1, MoveWeightingTotal + 1);

        foreach (Move move in moveInstancesOfType)
        {
            if (move == null || move.moveSO.MoveWeighting == 0) continue;

            if (randomValue > move.moveSO.MoveWeighting)
            {
                randomValue -= move.moveSO.MoveWeighting;
            }
            else
            {
                playerCombat.moveSelected = move;
                return;
            }
        }

        Debug.LogError("Failed to select a move! This should never happen. Random value was " + randomValue);
    }
}
