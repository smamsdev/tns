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
    public int enemyAttack;
    public int enemyAttackTotal;
    public int fend;

    [Header("")]
    public int enemyXP;

    [Header("Parts")]
    public int enemyBodyHP;
    public int enemyArmsHP;
    public int enemyHeadHP;

    [HideInInspector] int enemyBodyMaxHP;
    [HideInInspector] int enemyArmsMaxHP;
    [HideInInspector] int enemyHeadMaxHP;

    [Header("Misc")]
    public int damageReceivedInjuryBonus;
    public int totalDamage;
    public Target targetIs;
    public int injuryPenalty;

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
}

    public void DamageTaken(int attackRemainder)

    {
        DamageToHP(attackRemainder + damageReceivedInjuryBonus);
        DamageToParts(attackRemainder);
    }


    public void DamageToParts(int attackRemainder)
    {

        if (targetIs == Target.body) 
        
        {
            CombatEvents.BodyPartDamageTakenDisplay.Invoke("Body", enemyBodyHP, enemyBodyHP-attackRemainder, enemyBodyMaxHP);
            enemyBodyHP = Mathf.Clamp(enemyBodyHP - attackRemainder, 0, 9999);
        }

        if (targetIs == Target.arms)

        {
            CombatEvents.BodyPartDamageTakenDisplay.Invoke("Arms", enemyArmsHP, enemyArmsHP - attackRemainder, enemyArmsMaxHP);
            enemyArmsHP = Mathf.Clamp(enemyArmsHP - attackRemainder, 0, 9999);
        }

        if (targetIs == Target.head)

        {
            CombatEvents.BodyPartDamageTakenDisplay.Invoke("Head", enemyHeadHP, enemyHeadHP - attackRemainder, enemyHeadMaxHP);
            enemyHeadHP = Mathf.Clamp(enemyHeadHP - attackRemainder, 0,9999);
        }

        if (enemyBodyHP == 0)
        {
            damageReceivedInjuryBonus = attackRemainder;
        }

        if (enemyArmsHP == 0)
        {
            injuryPenalty = enemyAttack / 2;
        }

        if (enemyHeadHP == 0)
        {
            injuryPenalty = enemyAttack / 2;
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
        enemyAttackTotal = enemyAttack - injuryPenalty;
        return enemyAttackTotal;
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

}
