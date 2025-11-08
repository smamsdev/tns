using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally : Combatant
{
    public List<MoveSO> moveList = new List<MoveSO>();
    private List<Move> moveInstances = new List<Move>();
    public GameObject movesFolderGO;

    public void InstantiateMoves()
    {
        foreach (MoveSO moveSO in moveList)
        {
            GameObject moveInstanceGO = Instantiate(moveSO.MovePrefab);
            moveInstanceGO.name = moveSO.MoveName;
            moveInstanceGO.transform.SetParent(movesFolderGO.transform, false);
            Move moveInstance = moveInstanceGO.GetComponent<Move>();
            Move move = moveInstanceGO.GetComponent<Move>();
            moveInstance.moveSO = moveSO;
            moveInstances.Add(move);
        }
    }

   public override void SelectMove()
    {
        int MoveWeightingTotal = 0;

        foreach (Move move in moveInstances)
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

        int randomValue = Random.Range(1, MoveWeightingTotal + 1);

        foreach (Move move in moveInstances)
        {
            if (move == null || move.moveSO.MoveWeighting == 0) continue;

            if (randomValue > move.moveSO.MoveWeighting)
            {
                randomValue -= move.moveSO.MoveWeighting;
            }
            else
            {
                moveSelected = move;
                return;
            }
        }

        Debug.LogError("Failed to select a move! This should never happen. Random value was " + randomValue);
    }
}
