using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Victory : State
{
    public Victory(CombatManagerV3 _combatManagerV3) : base(_combatManagerV3)

    {
    }

    public override IEnumerator Start()
    {
        combatManagerV3.battleScheme.enemyGameObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        combatManagerV3.battleScheme.enemyGameObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        combatManagerV3.battleScheme.enemyGameObject.GetComponent<SortingGroup>().enabled = true;

        combatManagerV3.transform.GetChild(0).GetChild(0).gameObject.SetActive(false); //combat menu container
        combatManagerV3.transform.GetChild(0).GetChild(1).gameObject.SetActive(false); //player stats container

        yield return new WaitForSeconds(0);

        FieldEvents.UpdateXP(combatManagerV3.battleScheme.enemyGameObject);
        CombatEvents.BattleMode?.Invoke(false);
        CombatEvents.UnlockPlayerMovement?.Invoke();
        FieldEvents.HasCompleted(combatManagerV3.gameObject);

    }
}
