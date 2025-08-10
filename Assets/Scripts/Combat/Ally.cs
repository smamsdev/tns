using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally : Combatant
{
    int moveWeightingTotal = 0;

    [SerializeField] AllyMove[] moves;

    private void Start()
    {
        if (fightingPosition == null)
        {
            fightingPosition = new GameObject(this.gameObject.name + " Ally Fighting Position");
            fightingPosition.transform.position = this.transform.position;
            fightingPosition.transform.SetParent(this.transform);

        }
        foreach (AllyMove moves in moves)
        {
            moveWeightingTotal += moves.moveWeighting;
        }
        if (moves.Length <= 1)
        { Debug.Log("assign more moves for " + this.gameObject.name); }
    }

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
}
