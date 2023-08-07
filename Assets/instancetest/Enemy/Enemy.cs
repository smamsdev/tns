using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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

    public Target targetIs;

    public int injuryPenalty;

    [SerializeField] TextMeshPro textMeshPro;

    [SerializeField] TargetDisplay targetDisplay;

    SpriteRenderer spriteRenderer;
    Transform myTransform;

    private void OnEnable()
    {
        CombatEvents.UpdateEnemyHP += UpdateEnemyHP;
        CombatEvents.IsEnemyDefeated += CheckForEnemyDefeated;
        CombatEvents.GetEnemyAttackPower += SendEnemyRawAttackPower;
        CombatEvents.SetEnemyBodyPartTarget += SetEnemyBodyPartTarget;
    }

    private void OnDisable()
    {
        CombatEvents.UpdateEnemyHP -= UpdateEnemyHP;
        CombatEvents.IsEnemyDefeated -= CheckForEnemyDefeated;
        CombatEvents.GetEnemyAttackPower += SendEnemyRawAttackPower;
        CombatEvents.SetEnemyBodyPartTarget += SetEnemyBodyPartTarget;
    }

    private void Start()
    {
        injuryPenalty = 0;
    }

    public void UpdateEnemyHP(int value)
    {
        if (targetIs == Target.body) 
        
        {
            enemyHP = Mathf.Clamp(enemyHP-value, 0,9999);
            enemyBodyHP = Mathf.Clamp(enemyBodyHP - value, 0, 9999);
        }

        if (targetIs == Target.arms)

        {
            enemyHP = Mathf.CeilToInt(enemyHP-value * 0.6f);
            enemyArmsHP = Mathf.Clamp(enemyArmsHP - value, 0, 9999);
            injuryPenalty += Mathf.CeilToInt(value * 0.1f);
        }

        if (targetIs == Target.head)

        {
            enemyHP = Mathf.CeilToInt(enemyHP-value * 0.4f);
            enemyHeadHP = Mathf.Clamp(enemyHeadHP - value, 0,9999);
        }

        if (enemyBodyHP == 0)
        { 
            injuryPenalty = enemyAttack / 2;

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

    public void CheckForEnemyDefeated()
    {
        if (enemyHP <= 0)
        { //CombatEvents.EnemyIsDefeated.Invoke();
        }
    }

    public void SendEnemyRawAttackPower() 
    
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
