using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally : Combatant
{
    [SerializeField] AllyMove[] moves;
    public AllyPermanentStats allyPermanentStats;

    private void OnEnable()
    {
        movementScript = GetComponent<MovementScript>();
    }

    public override void SelectMove()
    {
        int moveWeightingTotal = 0;

        foreach (var allyMove in moves)
        {
            if (allyMove.moveWeighting > 0)
            {
                moveWeightingTotal += allyMove.moveWeighting;
            }
        }

        if (moveWeightingTotal == 0)
        {
            Debug.LogError("No valid moves available to select!");
            return;
        }

        int randomValue = Random.Range(1, moveWeightingTotal + 1);

        foreach (var allyMove in moves)
        {
            if (allyMove.moveWeighting == 0) continue;

            if (randomValue > allyMove.moveWeighting)
            {
                randomValue -= allyMove.moveWeighting;
            }
            else
            {
                moveSelected = allyMove;
                return;
            }
        }

        Debug.LogError("Failed to select a move! Random value was " + randomValue);
    }


    //not sure ineed the below anymore??
    public void DamageTaken(int attackRemainder)

    {
        DamageToHP(attackRemainder);
    }

    void DamageToHP(int damageTotal)
    {
        CurrentHP = CurrentHP - damageTotal;

        //combatantUI.statsDisplay.UpdateAllyHPDisplay(currentHP);
        //combatantUI.ally.ShowEnemyDamageDisplay(damageTotal);

        //fix!!

        if (CurrentHP <= 0)
        {
            // CombatEvents.EnemyIsDead.Invoke(true); fix this
        }
    }

 
}
