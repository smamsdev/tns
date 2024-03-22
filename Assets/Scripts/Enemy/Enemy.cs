using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public enum Target {body, arms, head};

public class Enemy : MonoBehaviour
{
    [Header("")]
    public string enemyName;
    [Header("")]
    public int enemyHP;

    [Header("")]
    public int attackBase;
    public int fendBase;
    public int attackTotal;
    public int fendTotal;

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
    public int injuryPenalty;

    int moveWeightingTotal = 0;
    public int randomValue;
    public int rng;

    public EnemyMove moveSelected;

    private void OnEnable()
    {
        CombatEvents.SetEnemyBodyPartTarget += SetEnemyBodyPartTarget;
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
                CombatEvents.BodyPartDamageTakenDisplay.Invoke("Body", enemyBodyHP, Mathf.Clamp(enemyBodyHP - attackRemainder, 0, 9999), enemyBodyMaxHP);
                enemyBodyHP = Mathf.Clamp(enemyBodyHP - attackRemainder, 0, 9999);
            }
        }

        if (targetIs == Target.arms)

        {
            if (enemyArmsHP > 0)
            {
                CombatEvents.BodyPartDamageTakenDisplay.Invoke("Arms", enemyArmsHP, Mathf.Clamp(enemyArmsHP - attackRemainder, 0, 9999), enemyArmsMaxHP);
                enemyArmsHP = Mathf.Clamp(enemyArmsHP - attackRemainder, 0, 9999);
            }
        }

        if (targetIs == Target.head)

        {
            if (enemyHeadHP > 0)
            {
                CombatEvents.BodyPartDamageTakenDisplay.Invoke("Head", enemyHeadHP, Mathf.Clamp(enemyHeadHP - attackRemainder, 0, 9999), enemyHeadMaxHP);
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
        enemyHP = enemyHP - damageTotal;

        CombatEvents.UpdateEnemyHPUI.Invoke(enemyHP);
        CombatEvents.ShowEnemyDamageTakenDisplay?.Invoke(damageTotal);

        if (enemyHP <= 0)
        {
            CombatEvents.EnemyIsDead.Invoke(true);
        }
    }

    public int EnemyAttackTotal() 
    
    {
        return attackTotal;
    }

    void SetEnemyBodyPartTarget(int value)
    {
        if (value == 1)
        {
            targetIs = Target.body;
        }
        if (value == 2)
        {
            targetIs = Target.arms;
        }
        if (value == 3)
        {
            targetIs = Target.head;
        }
    }

    public void SelectEnemyMove()

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
                LoadMove(enemyMove);
                return;
            }
        }

        Debug.LogError("Failed to select a move!");
    }

    public void LoadMove(EnemyMove enemyMove)

    {
        moveSelected = enemyMove;
        attackTotal = Mathf.RoundToInt(attackBase * moveSelected.attackMoveModPercent);
        fendTotal = Mathf.RoundToInt(fendBase * moveSelected.fendMoveModPercent);

        rng = Mathf.RoundToInt(attackTotal * Random.Range(-0.2f, 0.2f));

        attackTotal = Mathf.RoundToInt(attackTotal - injuryPenalty) + rng; //throw in some RNG for fun
    }


}
