using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Victory : State
{
    [SerializeField] CombatManager combatManager;

    public override IEnumerator StartState()
    {
        combatManager.battleScheme.enemyGameObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        combatManager.battleScheme.enemyGameObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        combatManager.battleScheme.enemyGameObject.GetComponent<SortingGroup>().enabled = true;

        combatManager.transform.GetChild(0).GetChild(0).gameObject.SetActive(false); //combat menu container
        combatManager.transform.GetChild(0).GetChild(1).gameObject.SetActive(false); //player stats container

        yield return new WaitForSeconds(0);

        FieldEvents.UpdateXP(combatManager.battleScheme.enemyGameObject);
        CombatEvents.BattleMode?.Invoke(false);
        CombatEvents.UnlockPlayerMovement?.Invoke();
        FieldEvents.HasCompleted(combatManager.gameObject);

    }
}
