using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[CreateAssetMenu]

public class EnemyManagerSO : ScriptableObject
{
    [SerializeField] GameObject enemyPf;

    GameObject enemy;

    public int baseHP;
    public int baseAttackPower;
    public Color color;

    public void SetHardEnemy()

    {
        baseHP = 55;
        baseAttackPower = 12;
        color = Color.red;

    }

    public void SetMediumEnemy()

    {
        baseHP = 25;
        baseAttackPower = 7;
        color = Color.yellow;

    }

    public void SetEasyEnemy()

    {
        baseHP = 12;
        baseAttackPower = 4;
        color = Color.white;

    }


    public void SpawnEnemy()
    {
        GameObject enemy = Instantiate(enemyPf);



    }





}
