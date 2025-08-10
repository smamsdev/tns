using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public enum Target {body, arms, head};

public class Enemy : Combatant
{
    [Header("")]
    public int enemyXP;

    [Header("Parts")]
    public int enemyBodyHP;
    public int enemyArmsHP;
    public int enemyHeadHP;

    [HideInInspector] int enemyBodyMaxHP;
    [HideInInspector] int enemyArmsMaxHP;
    [HideInInspector] int enemyHeadMaxHP;

    [Header("Moves")]

    [SerializeField] EnemyMove[] enemyMoves;

    [Header("Misc")]
    public int damageReceivedInjuryBonus;
    public int totalDamage;
    public Target targetIs;

    int moveWeightingTotal = 0;
    public int randomValue;
    public int rng;

    private void OnEnable()
    {
        if (fightingPosition == null)
        {
            fightingPosition = new GameObject(this.gameObject.name + " Enemy Fighting Position");
            fightingPosition.transform.position = this.transform.position;
            fightingPosition.transform.SetParent(this.transform); 
        }
    }

    private void Start()
    {
        injuryPenalty = 0;
        damageReceivedInjuryBonus = 0;

        enemyBodyMaxHP = enemyBodyHP; 
        enemyArmsMaxHP = enemyArmsHP; 
        enemyHeadMaxHP = enemyHeadHP;

        foreach (var enemyMoves in enemyMoves)
        {
            moveWeightingTotal += enemyMoves.moveWeighting;
        }
    }

    public void DamageTaken(int attackRemainder, float damageToBodyMod)

    {
        DamageToHP(attackRemainder + damageReceivedInjuryBonus);
        //DamageToParts(Mathf.RoundToInt(attackRemainder * damageToBodyMod));
    }

    void DamageToHP(int damageTotal)
    {
        CurrentHP = CurrentHP - damageTotal;

        combatantUI.statsDisplay.UpdateHPDisplay(CurrentHP);
        StartCoroutine(combatantUI.damageTakenDisplay.ShowDamageDisplayCoro(damageTotal));

        if (CurrentHP <= 0)
        {
           // CombatEvents.EnemyIsDead.Invoke(true); fix this
        }
    }

    public void SetEnemyBodyPartTarget(int value)
    {
        if (value == 0)
        {
            targetIs = Target.body;
        }
        if (value == 1)
        {
            targetIs = Target.arms;
        }
        if (value == 2)
        {
            targetIs = Target.head;
        }
    }

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

}
