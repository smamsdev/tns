using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public enum Target {body, arms, head};

public class Enemy : Combatant
{
    public EnemyUI enemyUI;

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

    public EnemyMove moveSelected;

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
        CombatEvents.SetEnemyBodyPartTarget += SetEnemyBodyPartTarget;

        if (fightingPosition == null)
        {
            fightingPosition = new GameObject(this.gameObject.name + " Enemy Fighting Position");
            fightingPosition.transform.position = this.transform.position;
            fightingPosition.transform.SetParent(null); 
        }
    }

    private void OnDisable()
    {
        CombatEvents.SetEnemyBodyPartTarget += SetEnemyBodyPartTarget;
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
        DamageToParts(Mathf.RoundToInt(attackRemainder * damageToBodyMod));
    }

    public void DamageToParts(int attackRemainder)
    {
        if (targetIs == Target.body) 
        
        {
            if (enemyBodyHP > 0)
            {
                enemyUI.bodyPartsDamageTakenDisplay.BodyPartDamageTakenDisplay("Body", enemyBodyHP, Mathf.Clamp(enemyBodyHP - attackRemainder, 0, 9999), enemyBodyMaxHP);
                enemyBodyHP = Mathf.Clamp(enemyBodyHP - attackRemainder, 0, 9999);
            }
        }

        if (targetIs == Target.arms)

        {
            if (enemyArmsHP > 0)
            {
                enemyUI.bodyPartsDamageTakenDisplay.BodyPartDamageTakenDisplay("Arms", enemyArmsHP, Mathf.Clamp(enemyArmsHP - attackRemainder, 0, 9999), enemyArmsMaxHP);
                enemyArmsHP = Mathf.Clamp(enemyArmsHP - attackRemainder, 0, 9999);
            }
        }

        if (targetIs == Target.head)

        {
            if (enemyHeadHP > 0)
            {
                enemyUI.bodyPartsDamageTakenDisplay.BodyPartDamageTakenDisplay("Head", enemyHeadHP, Mathf.Clamp(enemyHeadHP - attackRemainder, 0, 9999), enemyHeadMaxHP);
                enemyHeadHP = Mathf.Clamp(enemyHeadHP - attackRemainder, 0, 9999);
            }
        }

        if (enemyBodyHP == 0)
        {
            damageReceivedInjuryBonus = attackRemainder;
        }

        if (enemyArmsHP == 0)
        {
            injuryPenalty = attackTotal / 2;
        }

        if (enemyHeadHP == 0)
        {
            injuryPenalty = attackTotal / 2;
        }
    }

    void DamageToHP(int damageTotal)
    {
        currentHP = currentHP - damageTotal;

        enemyUI.enemyStatsDisplay.UpdateEnemyHPDisplay(currentHP);
        enemyUI.enemyDamageTakenDisplay.ShowEnemyDamageDisplay(damageTotal);

        if (currentHP <= 0)
        {
           // CombatEvents.EnemyIsDead.Invoke(true); fix this
        }
    }

    public int EnemyAttackTotal() 
    
    {
        return attackTotal;
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
        randomValue = Mathf.RoundToInt(Random.Range(0f, moveWeightingTotal));

        foreach (var enemyMove in enemyMoves)
        {

            if (randomValue >= enemyMove.moveWeighting)
            {
                randomValue -= enemyMove.moveWeighting;
            }
            else 
            {
                moveSelected = enemyMove;
                moveSelected.LoadMove(this);
                return;
            }
        }

        Debug.LogError("Failed to select a move!");
    }

}
