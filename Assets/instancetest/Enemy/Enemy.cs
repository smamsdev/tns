using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum Target {body, arms, head};

public class Enemy : MonoBehaviour
{
    public int enemyHP;
    public int enemyAttack;
    public int enemyAttackTotal;
    public string enemyName;
    public int enemyBodyHP;
    public int enemyArmsHP;
    public int enemyHeadHP;

    public int damageReceivedInjuryBonus;
    public int totalDamage;

    public Target targetIs;

    public int injuryPenalty;


    SpriteRenderer spriteRenderer;

    private void OnEnable()
    {
        CombatEvents.CalculateEnemyDamageTaken += CalculateEnemyDamageTaken;
        CombatEvents.UpdateEnemyHP += UpdateEnemyHP;
        CombatEvents.IsEnemyDefeated += CheckForEnemyDefeated;
        CombatEvents.GetEnemyAttackPower += SendEnemyRawAttackPowerIS;
        CombatEvents.SetEnemyBodyPartTarget += SetEnemyBodyPartTarget;
    }

    private void OnDisable()
    {
        CombatEvents.CalculateEnemyDamageTaken -= CalculateEnemyDamageTaken;
        CombatEvents.UpdateEnemyHP += UpdateEnemyHP;
        CombatEvents.IsEnemyDefeated -= CheckForEnemyDefeated;
        CombatEvents.GetEnemyAttackPower += SendEnemyRawAttackPowerIS;
        CombatEvents.SetEnemyBodyPartTarget += SetEnemyBodyPartTarget;
    }

    private void Start()
    {
        injuryPenalty = 0;
        damageReceivedInjuryBonus = 0;

    }

    public void CalculateEnemyDamageTaken(int value)
    {

        if (targetIs == Target.body) 
        
        {
            totalDamage = value + damageReceivedInjuryBonus;
            enemyBodyHP = Mathf.Clamp(enemyBodyHP - value, 0, 9999);
        }

        if (targetIs == Target.arms)

        {
            totalDamage = Mathf.CeilToInt((value * 0.6f) + damageReceivedInjuryBonus);

            enemyArmsHP = Mathf.Clamp(enemyArmsHP - value, 0, 9999);
            injuryPenalty += Mathf.CeilToInt(value * 0.1f);
        }

        if (targetIs == Target.head)

        {
            totalDamage = Mathf.CeilToInt((value * 0.4f) + damageReceivedInjuryBonus);
            enemyHeadHP = Mathf.Clamp(enemyHeadHP - value, 0,9999);
        }

        if (enemyBodyHP == 0)
        {
            damageReceivedInjuryBonus = value;
            this.transform.GetChild(3).GetChild(0).gameObject.SetActive(false);

        }

        if (enemyArmsHP == 0)
        {
            injuryPenalty = enemyAttack / 2;
            this.transform.GetChild(3).GetChild(2).gameObject.SetActive(false);


        }

        if (enemyHeadHP == 0)
        {
            injuryPenalty = enemyAttack / 2;
            this.transform.GetChild(3).GetChild(4).gameObject.SetActive(false);

        }

        CombatEvents.UpdateEnemyHP.Invoke(totalDamage);

    }

    void UpdateEnemyHP(int value)
    {
        enemyHP = Mathf.Clamp(enemyHP - value, 0, 9999);
    }

    public void CheckForEnemyDefeated()
    {
        if (enemyHP <= 0)
        { CombatEvents.EnemyIsDefeated.Invoke();
        }
    }

    public void SendEnemyRawAttackPowerIS() 
    
    {
        enemyAttackTotal = enemyAttack - injuryPenalty;
        CombatEvents.EnemyAttackPower.Invoke(enemyAttackTotal);
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
