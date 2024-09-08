using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerMoveManager : MonoBehaviour
{
    public int firstMoveIs;
    public int secondMoveIs;
    [SerializeField] PlayerEquippedMovesSO playerEquippedMovesSO;

    public PlayerMove selectedPlayerMove;

    public ViolentMove[] violentAttackSlots = new ViolentMove[5];
    public ViolentMove[] violentFendSlots =  new ViolentMove[5];
    public ViolentMove[] violentFocusSlots = new ViolentMove[5];

    public CautiousMove[] cautiousAttackSlots = new CautiousMove[5];
    public CautiousMove[] cautiousFendSlots = new CautiousMove[5];
    public CautiousMove[] cautiousFocusSlots = new CautiousMove[5];

    public PreciseMove[] preciseAttackSlots = new PreciseMove[5];
    public PreciseMove[] preciseFendSlots = new PreciseMove[5];
    public PreciseMove[] preciseFocusSlots = new PreciseMove[5];

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
        LoadMovesOfType<ViolentMove>(playerEquippedMovesSO.violentAttacksListString, violentAttackSlots);
        LoadMovesOfType<ViolentMove>(playerEquippedMovesSO.violentFendsListString, violentFendSlots);
        LoadMovesOfType<ViolentMove>(playerEquippedMovesSO.violentFocusesListString, violentFocusSlots);
        LoadMovesOfType<CautiousMove>(playerEquippedMovesSO.cautiousAttackssListString, cautiousAttackSlots);
        LoadMovesOfType<CautiousMove>(playerEquippedMovesSO.cautiousFendsListString, cautiousFendSlots);
        LoadMovesOfType<CautiousMove>(playerEquippedMovesSO.cautiousFocusesListString, cautiousFocusSlots);
        LoadMovesOfType<PreciseMove>(playerEquippedMovesSO.preciseAttacksListString, preciseAttackSlots);
        LoadMovesOfType<PreciseMove>(playerEquippedMovesSO.preciseFendsListString, preciseFendSlots);
        LoadMovesOfType<PreciseMove>(playerEquippedMovesSO.preciseFocusesListString, preciseFocusSlots);
    }

    public void LoadMovesOfType<T>(string[] moveName, T[] moveSlot)
    {
        for (int i = 0; i < moveSlot.Length; i++)
        {
            if (!string.IsNullOrEmpty(moveName[i]))

            {
                var moveGameObject = GameObject.Find(moveName[i]);
                var moveComponent = moveGameObject.GetComponent<T>();
                
                if (moveComponent != null)
                {
                    moveSlot[i] = moveComponent;
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
    // if (firstMoveIs == 0) { Debug.Log("set gear as a move"); }
    //
    // if (firstMoveIs == 1 && secondMoveIs == 1) { SelectMoveFromEquippedMoves(violentAttacks); }
    // if (firstMoveIs == 1 && secondMoveIs == 2) { SelectMoveFromEquippedMoves(violentFends); }
    // if (firstMoveIs == 1 && secondMoveIs == 3) { SelectMoveFromEquippedMoves(violentFocuses); }
    //
    // if (firstMoveIs == 2 && secondMoveIs == 1) { SelectMoveFromEquippedMoves(cautiousAttacks); }
    // if (firstMoveIs == 2 && secondMoveIs == 2) { SelectMoveFromEquippedMoves(cautiousFends); }
    // if (firstMoveIs == 2 && secondMoveIs == 3) { SelectMoveFromEquippedMoves(cautiousFocuses); }
    //
    // if (firstMoveIs == 3 && secondMoveIs == 1) { SelectMoveFromEquippedMoves(preciseAttacks); }
    // if (firstMoveIs == 3 && secondMoveIs == 2) { SelectMoveFromEquippedMoves(preciseFends); }
    // if (firstMoveIs == 3 && secondMoveIs == 3) { SelectMoveFromEquippedMoves(preciseFocuses); }
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
