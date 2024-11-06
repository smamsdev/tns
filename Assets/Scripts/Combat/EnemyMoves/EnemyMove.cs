using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyMove : MonoBehaviour
{
    public float attackMoveModPercent;
    public float fendMoveModPercent;
    public int moveWeighting;

    public string moveName;
    public CombatManager combatManager;

    public abstract IEnumerator OnEnemyAttack();

    public abstract void LoadMove(Enemy enemy);



}
