using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTarget : MonoBehaviour
{
    [SerializeField] CombatManagerV3 combatManagerV3;

    public IEnumerator StartState()
    {
        yield return new WaitForSeconds(0.1f);

        combatManagerV3.combatUIScript.secondMoveMenu.SetActive(false);

        if (combatManagerV3.playerMoveManager.firstMoveIs == 1 || combatManagerV3.playerMoveManager.secondMoveIs == 1)

        {
            combatManagerV3.attackTargetMenuScript.DisplayAttackTargetMenu();
        }

        else { combatManagerV3.SetBattleStateApplyPlayerMove(); }

        yield break;
    }

    public void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))

        {
            combatManagerV3.SetBattleStateSecondMove();
            CombatEvents.InputCoolDown?.Invoke(0.2f);
        }
    }

}

