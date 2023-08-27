using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyLoader : MonoBehaviour
{
    [SerializeField] GameObject enemyPf;
    [SerializeField] EnemyBaseStats enemyBaseStats;
    
    Enemy enemy;
    SpriteRenderer enemySprite;

    GameObject enemyInstance;
    bool enemyIsInstantiated = false;

    private void OnEnable()
    {
        CombatEvents.EnemyIsDefeated += DestroyEnemy;
    }

    private void OnDisable()
    {
        CombatEvents.EnemyIsDefeated -= DestroyEnemy;
    }

        private void Start()
    {
       // SpawnEnemy();
    }


    void SpawnEnemy()
    {
        enemyInstance = Instantiate(enemyPf, transform);
        enemyIsInstantiated = true;

        enemy = enemyInstance.GetComponent<Enemy>();
        enemySprite = enemyInstance.GetComponent<SpriteRenderer>();
        enemy.enemyHP = enemyBaseStats.enemyHP;
        enemy.enemyAttack = enemyBaseStats.enemyAttack;
        enemy.enemyName = enemyBaseStats.enemyName;
        enemySprite.color = enemyBaseStats.enemyColor;
        enemy.enemyBodyHP = enemyBaseStats.enemyBodyHP;
        enemy.enemyArmsHP = enemyBaseStats.enemyArmsHP;
        enemy.enemyHeadHP = enemyBaseStats.enemyHeadHP;

        CombatEvents.InitializePartsHP.Invoke();

        CombatEvents.InitializeEnemyHP.Invoke(enemy.enemyHP);

    }

    public void DestroyEnemy()
    {
        if (enemyIsInstantiated)
        {
            Destroy(enemyInstance);
            enemyIsInstantiated = false;
        }
    }



}
