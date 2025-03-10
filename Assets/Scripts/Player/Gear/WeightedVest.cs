using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class WeightedVest : Gear
{
    Enemy enemy;

    private void OnEnable()
    {
        CombatEvents.BattleMode += LoadGear;
    }

    private void OnDisable()
    {
        CombatEvents.BattleMode -= LoadGear;
    }

    private void Awake()
    {
        gearID = this.name;
    }

    private void LoadGear(bool on)

    {
        StartCoroutine(GetEnemyReference(0.1f));
    }

    IEnumerator GetEnemyReference(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
      //  enemy = GameObject.Find("CombatManager").GetComponent<CombatManager>().battle.enemyGameObject.GetComponentInChildren<Enemy>();
    }

    public override void ApplyFendGear() //asasd

    {
        enemy.attackTotal += Mathf.RoundToInt(enemy.attackTotal * 0.3f);
    }

    public override void ResetFendGear()

    {
        enemy.enemyXP += Mathf.RoundToInt(enemy.enemyXP * 0.1f);
    }
}
