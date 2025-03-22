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
            Debug.Log("no ally fighting position set" + this.gameObject.name);
            fightingPosition = new GameObject(this.gameObject.name + " Ally Fighting Position");
            fightingPosition.transform.position = this.transform.position;
            fightingPosition.transform.SetParent(null);

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
        var randomValue = Mathf.RoundToInt(Random.Range(0f, moveWeightingTotal));

        foreach (AllyMove move in moves)
        {

            if (randomValue >= move.moveWeighting)
            {
                randomValue -= move.moveWeighting;
            }
            else
            {
                moveSelected = move;
                return;
            }
        }
        Debug.LogError("Failed to select a move!");
    }

    public int AllyAttackTotal()

    {
        return attackTotal;
    }
}
