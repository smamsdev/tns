using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyMove : MonoBehaviour
{
    public float attackMoveModPercent;
    public float fendMoveModPercent;
    public int moveWeighting;
    public float animtionIntTriggerToUse = 0;
    public float distanceToCoverPercent = 80f;
    public float attackPushStrength = 0.2f;
    public string moveName;
    [HideInInspector] public CombatManager combatManager;
    [HideInInspector] public Enemy enemy;

    public abstract IEnumerator EnemyAttack(CombatManager _combatManager);

    public virtual IEnumerator EnemyReturn()

    {
        yield return new WaitForSeconds(0.5f);

        var combatMovementInstanceGO = Instantiate(combatManager.combatMovementPrefab, this.transform);
        var combatMovementInstance = combatMovementInstanceGO.GetComponent<CombatMovement>();
        yield return (combatMovementInstance.MoveCombatant(enemy.gameObject, enemy.fightingPosition.transform.position));
        Destroy(combatMovementInstanceGO);
    }

    public abstract void LoadMove(Enemy enemy);
}
