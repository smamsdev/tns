using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerMoveManager : MonoBehaviour
{
    public int firstMoveIs;
    public int secondMoveIs;
    public PlayerMoveListSO playerMoveListSO;
    public PlayerEquippedMoves playerEquippedMoves;

    public PlayerMove selectedPlayerMove;

    public List<ViolentMove> violentAttacks = new List<ViolentMove>();
    public List<ViolentMove> violentFends = new List<ViolentMove>();
    public List<ViolentMove> violentFocuses = new List<ViolentMove>();

    public List<CautiousMove> cautiousAttacks = new List<CautiousMove>();
    public List<CautiousMove> cautiousFends = new List<CautiousMove>();
    public List<CautiousMove> cautiousFocuses = new List<CautiousMove>();

    public List<PreciseMove> preciseAttacks = new List<PreciseMove>();
    public List<PreciseMove> preciseFends = new List<PreciseMove>();
    public List<PreciseMove> preciseFocuses = new List<PreciseMove>();

    private void Start()
    {
        LoadMoveListFromSO();
    }

    private void SaveMoveListToSO()
    {
     //   for (int i = 0; i < playerMoveInventory.Count; i++)
     //   {
     //       if (!playerMoveListSO.moveListString.Contains(playerMoveInventory[i].moveName))
     //           playerMoveListSO.moveListString.Add(playerMoveInventory[i].moveName);
     //   }
    }

    public void LoadMoveListFromSO()
    {
        LoadMovesOfType<ViolentMove>(playerMoveListSO.violentAttacksListString, violentAttacks);
        LoadMovesOfType<ViolentMove>(playerMoveListSO.violentFendsListString, violentFends);
        LoadMovesOfType<ViolentMove>(playerMoveListSO.violentFocusesListString, violentFocuses);

        LoadMovesOfType<CautiousMove>(playerMoveListSO.cautiousAttackssListString, cautiousAttacks);
        LoadMovesOfType<CautiousMove>(playerMoveListSO.cautiousFendsListString, cautiousFends);
        LoadMovesOfType<CautiousMove>(playerMoveListSO.cautiousFocusesListString, cautiousFocuses);

        LoadMovesOfType<PreciseMove>(playerMoveListSO.preciseAttacksListString, preciseAttacks);
        LoadMovesOfType<PreciseMove>(playerMoveListSO.preciseFendsListString, preciseFends);
        LoadMovesOfType<PreciseMove>(playerMoveListSO.preciseFocusesListString, preciseFocuses);
    }

    private void LoadMovesOfType<T>(List<string> moveNames, List<T> moveList) where T : Component
    {
        foreach (string moveName in moveNames)
        {
            var moveGameObject = GameObject.Find(moveName);
            var moveComponent = moveGameObject.GetComponent<T>();
            if (moveComponent != null)
            {
                moveList.Add(moveComponent);
            }
        }
    }

    public void CombineStanceAndMove()

    {
        if (firstMoveIs == 0) {GearMove();}

        if (firstMoveIs == 1 && secondMoveIs == 1) { SelectMoveFromEquippedMoves(violentAttacks); }
        if (firstMoveIs == 1 && secondMoveIs == 2) { SelectMoveFromEquippedMoves(violentFends); }
        if (firstMoveIs == 1 && secondMoveIs == 3) { SelectMoveFromEquippedMoves(violentFocuses); }

        if (firstMoveIs == 2 && secondMoveIs == 1) { SelectMoveFromEquippedMoves(cautiousAttacks); }
        if (firstMoveIs == 2 && secondMoveIs == 2) { SelectMoveFromEquippedMoves(cautiousFends); }
        if (firstMoveIs == 2 && secondMoveIs == 3) { SelectMoveFromEquippedMoves(cautiousFocuses); }

        if (firstMoveIs == 3 && secondMoveIs == 1) { SelectMoveFromEquippedMoves(preciseAttacks); }
        if (firstMoveIs == 3 && secondMoveIs == 2) { SelectMoveFromEquippedMoves(preciseFends); }
        if (firstMoveIs == 3 && secondMoveIs == 3) { SelectMoveFromEquippedMoves(preciseFocuses); }
    }

    void SelectMoveFromEquippedMoves<T>(List<T> equippedMoveList) where T : PlayerMove
    {
        int moveWeightingTotal = 0;
        int randomValue = 0;

        foreach (var playerMove in equippedMoveList)
        {
            moveWeightingTotal += playerMove.moveWeighting;
        }

        randomValue = Mathf.RoundToInt(UnityEngine.Random.Range(0f, moveWeightingTotal));

        foreach (var playerMove in equippedMoveList)
        {
            if (randomValue >= playerMove.moveWeighting)
            {
                randomValue -= playerMove.moveWeighting;
            }
            else
            {
                selectedPlayerMove = playerMove;
                return;
            }
        }
        Debug.LogError("Failed to select a move!");
    }

    public PlayerMove GetSelectedPlayerMove() 
    
    { 
        return selectedPlayerMove;
    }

    public void GearMove()

    {
        {
            CombatEvents.UpdateNarrator.Invoke("");
        }
    }

}
