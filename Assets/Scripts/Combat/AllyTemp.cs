using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyTemp : Combatant
{
    [SerializeField] EnemyMove[] enemyMoves;


    public override void SelectMove()
    {
        int moveWeightingTotal = 0;

        foreach (var enemyMove in enemyMoves)
        {
            if (enemyMove.moveWeighting > 0)
            {
                moveWeightingTotal += enemyMove.moveWeighting;
            }
        }

        if (moveWeightingTotal == 0)
        {
            Debug.LogError("No valid moves available to select!");
            return;
        }

        int randomValue = Random.Range(1, moveWeightingTotal + 1);

        foreach (var enemyMove in enemyMoves)
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
