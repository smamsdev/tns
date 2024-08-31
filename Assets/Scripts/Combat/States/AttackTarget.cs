using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTarget : State
{
    [SerializeField] CombatManager combatManager;

    public override IEnumerator StartState()
    {
        CombatEvents.SendMove += SelectBodyPart;
        yield return new WaitForSeconds(0.1f);

        combatManager.combatUIScript.ShowBodyPartTargetMenu(true);
        combatManager.combatUIScript.ShowEnemySelectMenu(false);

        yield break;
    }

    void SelectBodyPart(int moveValue)

    {
        combatManager.enemy[combatManager.selectedEnemy].SetEnemyBodyPartTarget(moveValue);
        combatManager.SetState(combatManager.applyMove);

        CombatEvents.SendMove -= SelectBodyPart;
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))

        {
            combatManager.SetState(combatManager.secondMove);
            CombatEvents.InputCoolDown?.Invoke(0.2f);
        }
    }

    private void OnDisable()
    {
        CombatEvents.SendMove -= SelectBodyPart;
    }

    public void SelectBody()
    {
        combatManager.enemy[combatManager.selectedEnemy].SetEnemyBodyPartTarget(1);
        combatManager.SetState(combatManager.applyMove);
    }
}

