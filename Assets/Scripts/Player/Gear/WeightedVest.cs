using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class WeightedVest : Gear
{

    Enemy enemy;

    [TextArea(2, 5)] public string description;

    private void Start()

    {
        StartCoroutine(LateStart(0.1f));
    }

    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        enemy = GameObject.Find("CombatManager").GetComponent<CombatManagerV3>().battleScheme.enemyGameObject.GetComponentInChildren<Enemy>();
    }

    public override void ApplyFendGear()

    {
        enemy.enemyAttack += Mathf.RoundToInt(enemy.enemyAttack*0.3f);
    }

    public override void ResetFendGear()

    {
        enemy.enemyXP += Mathf.RoundToInt(enemy.enemyXP * 0.1f);
    }

}
