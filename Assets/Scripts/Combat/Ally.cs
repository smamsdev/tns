using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally : Combatant
{
    [SerializeField] Move[] moves;

    public override void SelectMove()
    {
        int moveWeightingTotal = 0;

        foreach (var move in moves)
        {
            if (move.moveWeighting > 0)
            {
                moveWeightingTotal += move.moveWeighting;
            }
        }

        if (moveWeightingTotal == 0)
        {
            Debug.LogError("No valid moves available to select! for" + this.gameObject.name);
            return;
        }

        int randomValue = Random.Range(1, moveWeightingTotal + 1);

        foreach (var enemyMove in moves)
        {
            if (enemyMove.moveWeighting == 0) continue;

            if (randomValue > enemyMove.moveWeighting)
            {
                randomValue -= enemyMove.moveWeighting;
            }
            else
            {
                moveSelected = enemyMove;
                return;
            }
        }

        Debug.LogError("Failed to select a move! Random value was " + randomValue);
    }
}
