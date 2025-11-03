using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally : Combatant
{
    public List<MoveSO> moveList = new List<MoveSO>();
    public GameObject movesFolderGO;

    public void InstantiateMoves()
    {
        foreach (MoveSO moveSO in moveList)
        {
            GameObject moveInstanceGO = Instantiate(moveSO.movePrefab);
            moveInstanceGO.name = moveSO.moveName;
            moveInstanceGO.transform.SetParent(movesFolderGO.transform, false);
            Move moveInstance = moveInstanceGO.GetComponent<Move>();
            moveSO.moveInstance = moveInstance;
        }
    }

   public override void SelectMove()
    {
        int moveWeightingTotal = 0;

        foreach (var move in moveList)
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

        int randomValue = Random.Range(1, moveWeightingTotal + 1);

        foreach (var move in moveList)
        {
            if (move == null || move.moveWeighting == 0) continue;

            if (randomValue > move.moveWeighting)
            {
                randomValue -= move.moveWeighting;
            }
            else
            {
                moveSOSelected = move;
                return;
            }
        }

        Debug.LogError("Failed to select a move! This should never happen. Random value was " + randomValue);
    }
}
