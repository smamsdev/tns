using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUI : MonoBehaviour
{
    public FendScript fendScript;
    public EnemyDamageTakenDisplay enemyDamageTakenDisplay;
    public EnemyStatsDisplay enemyStatsDisplay;
    public EnemyAttackDisplay enemyAttackDisplay;
    public BodyPartsDamageTakenDisplay bodyPartsDamageTakenDisplay;
    public TargetDisplay partsTargetDisplay;

    //GOs to flip based on look direction
    public GameObject attackDisplayContainer;
    public GameObject partsDisplayContainer;
}
