using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerMoveManager : MonoBehaviour
{
    public int firstMoveIs;
    public int secondMoveIs;
    public PlayerEquippedMovesSO playerEquippedMovesSO;

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
        LoadEquippedMoveListFromSO();
    }

    public void LoadEquippedMoveListFromSO()
    {
        ClearAllMoveSlots();

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

    private void ClearAllMoveSlots()
    {
        Array.Clear(violentAttackSlots, 0, violentAttackSlots.Length);
        Array.Clear(violentFendSlots, 0, violentFendSlots.Length);
        Array.Clear(violentFocusSlots, 0, violentFocusSlots.Length);

        Array.Clear(cautiousAttackSlots, 0, cautiousAttackSlots.Length);
        Array.Clear(cautiousFendSlots, 0, cautiousFendSlots.Length);
        Array.Clear(cautiousFocusSlots, 0, cautiousFocusSlots.Length);

        Array.Clear(preciseAttackSlots, 0, preciseAttackSlots.Length);
        Array.Clear(preciseFendSlots, 0, preciseFendSlots.Length);
        Array.Clear(preciseFocusSlots, 0, preciseFocusSlots.Length);
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
    if (firstMoveIs == 0) { Debug.Log("set gear as a move"); }
    
    if (firstMoveIs == 1 && secondMoveIs == 1) { SelectMoveFromEquippedMoves(violentAttackSlots); }
    if (firstMoveIs == 1 && secondMoveIs == 2) { SelectMoveFromEquippedMoves(violentFendSlots); }
    if (firstMoveIs == 1 && secondMoveIs == 3) { SelectMoveFromEquippedMoves(violentFocusSlots); }
    
    if (firstMoveIs == 2 && secondMoveIs == 1) { SelectMoveFromEquippedMoves(cautiousAttackSlots); }
    if (firstMoveIs == 2 && secondMoveIs == 2) { SelectMoveFromEquippedMoves(cautiousFendSlots); }
    if (firstMoveIs == 2 && secondMoveIs == 3) { SelectMoveFromEquippedMoves(cautiousFocusSlots); }
    
    if (firstMoveIs == 3 && secondMoveIs == 1) { SelectMoveFromEquippedMoves(preciseAttackSlots); }
    if (firstMoveIs == 3 && secondMoveIs == 2) { SelectMoveFromEquippedMoves(preciseFendSlots); }
    if (firstMoveIs == 3 && secondMoveIs == 3) { SelectMoveFromEquippedMoves(preciseFocusSlots); }
    }

    void SelectMoveFromEquippedMoves<T>(T[] equippedMoveList) where T : PlayerMove

    {
        int moveWeightingTotal = 0;
        int randomValue = 0;

        foreach (var playerMove in equippedMoveList)
        {
            if (playerMove != null) // Skip null slots
            {
                moveWeightingTotal += playerMove.moveWeighting;
            }
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
