using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class EnemyBaseStats : ScriptableObject
{
    public int enemyHP;
    public int enemyAttack;
    public string enemyName;
    public Color enemyColor;
    SpriteRenderer enemyRenderer;

    public int enemyBodyHP;
    public int enemyArmsHP;
    public int enemyHeadHP;

}
