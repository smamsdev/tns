using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally : MonoBehaviour
{
    public GameObject allyFightingPosition;
    public AllyUI allyUI;

    public Vector2 forceLookDirection;

    public string allyName;

    public int attackBase;
    public int fendBase;
    public int attackPowerBase;
    public int maxHP;
    public int currentHP;

    public int attackTotal;
    public int fendTotal;

    public AllyMove moveSelected;
    int moveWeightingTotal = 0;

    [SerializeField] AllyMove[] moves;

    private void Start()
    {
        if (allyFightingPosition == null)
        {
            Debug.Log("no ally fighting position set" + this.gameObject.name);
            allyFightingPosition = new GameObject(this.gameObject.name + " Ally Fighting Position");
            allyFightingPosition.transform.position = this.transform.position;
            allyFightingPosition.transform.SetParent(null);

        }
        foreach (AllyMove moves in moves)
        {
            moveWeightingTotal += moves.moveWeighting;
        }
    }

    public void DamageTaken(int attackRemainder)

    {
        DamageToHP(attackRemainder);
    }

    void DamageToHP(int damageTotal)
    {
        currentHP = currentHP - damageTotal;

        //allyUI.allyStatsDisplay.UpdateAllyHPDisplay(currentHP);
        //allyUI.ally.ShowEnemyDamageDisplay(damageTotal);

        //fix!!

        if (currentHP <= 0)
        {
            // CombatEvents.EnemyIsDead.Invoke(true); fix this
        }
    }

    public void SelectAllyMove()
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
                moveSelected.LoadMove(this);
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
