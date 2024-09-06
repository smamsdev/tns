using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerMoveManager : MonoBehaviour
{
    public int firstMoveIs;
    public int secondMoveIs;
    //public PlayerMoveInventorySO playerMoveInventorySO;
    [SerializeField] PlayerEquippedMovesSO playerEquippedMovesSO;

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
        LoadMovesOfType<ViolentMove>(playerEquippedMovesSO.violentAttacksListString, violentAttacks);
        LoadMovesOfType<ViolentMove>(playerEquippedMovesSO.violentFendsListString, violentFends);
        LoadMovesOfType<ViolentMove>(playerEquippedMovesSO.violentFocusesListString, violentFocuses);

        LoadMovesOfType<CautiousMove>(playerEquippedMovesSO.cautiousAttackssListString, cautiousAttacks);
        LoadMovesOfType<CautiousMove>(playerEquippedMovesSO.cautiousFendsListString, cautiousFends);
        LoadMovesOfType<CautiousMove>(playerEquippedMovesSO.cautiousFocusesListString, cautiousFocuses);

        LoadMovesOfType<PreciseMove>(playerEquippedMovesSO.preciseAttacksListString, preciseAttacks);
        LoadMovesOfType<PreciseMove>(playerEquippedMovesSO.preciseFendsListString, preciseFends);
        LoadMovesOfType<PreciseMove>(playerEquippedMovesSO.preciseFocusesListString, preciseFocuses);
    }

    private void LoadMovesOfType<T>(string[] moveNames, List<T> moveList) where T : Component
    {
        foreach (string moveName in moveNames)
        {
            if (!string.IsNullOrEmpty(moveName))

            {
                var moveGameObject = GameObject.Find(moveName);
                var moveComponent = moveGameObject.GetComponent<T>();
                
                if (moveComponent != null)
                {
                    moveList.Add(moveComponent);
                }

                else
                { 
                    Debug.Log("no move of this name found in project" + " = " + moveName);
                }
            }
        }
    }

    public void CombineStanceAndMove()

    {
        if (firstMoveIs == 0) { Debug.Log("set gear as a move"); }

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
}
