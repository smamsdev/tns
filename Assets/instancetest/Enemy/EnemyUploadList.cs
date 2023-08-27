using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUploadList : MonoBehaviour
{

    [SerializeField] EnemyBaseStats enemyBaseStats;


    private void Start()
    {
        //EnemyEasy();
    }

    public void EnemyEasy()
 
    {
        enemyBaseStats.enemyHP = 100;
        enemyBaseStats.enemyAttack = 7;
        enemyBaseStats.enemyName = "Smams";
        enemyBaseStats.enemyColor = Color.white;

        enemyBaseStats.enemyBodyHP = 50;
        enemyBaseStats.enemyArmsHP = 50;
        enemyBaseStats.enemyHeadHP = 50;
    }

    public void EnemyMid()

    {
        enemyBaseStats.enemyHP = 62;
        enemyBaseStats.enemyAttack = 9;
        enemyBaseStats.enemyName = "Gohan";
        enemyBaseStats.enemyColor = Color.yellow;

        enemyBaseStats.enemyBodyHP = 50;
        enemyBaseStats.enemyArmsHP = 50;
        enemyBaseStats.enemyHeadHP = 50;

    }

    public void EnemyHard()

    {
        enemyBaseStats.enemyHP = 82;
        enemyBaseStats.enemyAttack = 15;
        enemyBaseStats.enemyName = "Vegeta";
        enemyBaseStats.enemyColor = new Color(1, 0.4f, 1f);

        enemyBaseStats.enemyBodyHP = 50;
        enemyBaseStats.enemyArmsHP = 50;
        enemyBaseStats.enemyHeadHP = 50;
    }
}
