using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Victory : State
{
    //rework this whole goddam thing pls. yucl

    public override IEnumerator StartState()
    {
        //combatManager.battleScheme.enemies[combatManager.selectedEnemy].transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        //combatManager.battleScheme.enemies[combatManager.selectedEnemy].transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        //combatManager.battleScheme.enemies[combatManager.selectedEnemy].GetComponent<SortingGroup>().enabled = true;


        Debug.Log("fix this bullshit");
       
        combatManager.transform.GetChild(0).GetChild(0).gameObject.SetActive(false); //combat menu container
        combatManager.transform.GetChild(0).GetChild(1).gameObject.SetActive(false); //player stats container

        yield return new WaitForSeconds(0);

        //FieldEvents.UpdateXP(combatManager.battleScheme.enemies[combatManager.selectedEnemy]);
        CombatEvents.BattleMode?.Invoke(false);
        CombatEvents.UnlockPlayerMovement?.Invoke();
        FieldEvents.HasCompleted(combatManager.gameObject);

    }
}
