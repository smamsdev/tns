using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUI : MonoBehaviour
{
    public EnemyFendScript enemyFendScript;
    public EnemyDamageTakenDisplay enemyDamageTakenDisplay;
    public EnemyStatsDisplay enemyStatsDisplay;
    public EnemyAttackDisplay enemyAttackDisplay;
    public BodyPartsDamageTakenDisplay bodyPartsDamageTakenDisplay;
    public TargetDisplay partsTargetDisplay;

    public void AnchorEnemyUIToEnemyGameObject(GameObject fightingPosition)

    {
        this.transform.position = fightingPosition.transform.position;
    }
}
