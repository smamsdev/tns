using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondMove : MonoBehaviour
{
    [SerializeField] CombatManagerV3 combatManagerV3;

    public IEnumerator StartState()
    {
        yield return new WaitForSeconds(0.1f);

        combatManagerV3.combatUIScript.ShowSecondMoveMenu();
        combatManagerV3.playerMoveManager.secondMoveIs = 0;

        yield break;
    }

    public void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))

        {
            combatManagerV3.SetBattleStateFirstMove();
            CombatEvents.InputCoolDown?.Invoke(0.1f);
        }
    }
}
