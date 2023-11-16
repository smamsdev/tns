using UnityEngine;

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

    [Header("")]
    public int enemyXP;

    [Header("Parts")]
    public int enemyBodyHP;
    public int enemyArmsHP;
    public int enemyHeadHP;

    [Header("Misc")]
    public int damageReceivedInjuryBonus;
    public int totalDamage;
    public Target targetIs;
    public int injuryPenalty;

    SpriteRenderer spriteRenderer;
    public GameObject battleSprites;

    private void OnEnable()
    {
        CombatEvents.CalculateEnemyDamageTaken += CalculateEnemyDamageTaken;
        CombatEvents.GetEnemyAttackPower += SendEnemyRawAttackPowerIS;
        CombatEvents.SetEnemyBodyPartTarget += SetEnemyBodyPartTarget;
    }

    private void OnDisable()
    {
        CombatEvents.CalculateEnemyDamageTaken -= CalculateEnemyDamageTaken;
        CombatEvents.GetEnemyAttackPower += SendEnemyRawAttackPowerIS;
        CombatEvents.SetEnemyBodyPartTarget += SetEnemyBodyPartTarget;
    }

    private void Start()
    {
        injuryPenalty = 0;
        damageReceivedInjuryBonus = 0;

        battleSprites = this.transform.parent.gameObject.transform.GetChild(1).gameObject;
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
            battleSprites.transform.GetChild(0).gameObject.SetActive(false);
        }

        if (enemyArmsHP == 0)
        {
            injuryPenalty = enemyAttack / 2;
            battleSprites.transform.GetChild(2).gameObject.SetActive(false);
        }

        if (enemyHeadHP == 0)
        {
            injuryPenalty = enemyAttack / 2;
            battleSprites.transform.GetChild(4).gameObject.SetActive(false);
        }

        UpdateenemyHP(totalDamage);
    }

    void UpdateenemyHP(int value)
    {
        enemyHP = enemyHP - value;

        CombatEvents.UpdateenemyHPUI.Invoke(totalDamage);

        if (enemyHP <= 0)
        {
            CombatEvents.EnemyIsDead.Invoke(true);

            var foundAnimators = battleSprites.GetComponentsInChildren<Animator>();
            foreach (Animator animator in foundAnimators)
            { 
                animator.enabled = false;
            }
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
