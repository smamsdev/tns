using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Setup : State
{
    public Setup(CombatManagerV3 _combatManagerV3) :base(_combatManagerV3)

    {
    }

    public override IEnumerator Start()

    {
        yield return new WaitForSeconds(0.01f);
        CombatEvents.CameraBattleMode?.Invoke(true);

        CombatEvents.UpdatePlayerPosition?.Invoke(combatManagerV3.playerFightingPosition.transform.position, 1f);

        //enemy
        combatManagerV3.enemyGameObjectDefaulPosition = combatManagerV3.enemyGameObject.transform.position;
        combatManagerV3.enemyGameObject.transform.GetChild(0).GetComponent<Enemy>().enabled = true; //turn on Enemy script, you don't want this on for every 'enemy' in the scnene
        combatManagerV3.enemyGameObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(true); //combat menu container
        combatManagerV3.enemyGameObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(true); //player stats container
        combatManagerV3.UpdateEnemyPosition(combatManagerV3.enemyGameObject.transform.position, 1);
        combatManagerV3.enemyGameObject.GetComponent<SortingGroup>().enabled = false;

        combatManagerV3.enemyRawAttackPower = 0;

        CombatEvents.InitializeEnemyPartsHP?.Invoke();
        CombatEvents.InitializeenemyHP?.Invoke(combatManagerV3.enemyGameObject.transform.GetChild(0).GetComponent<Enemy>().enemyHP);

        //player stats

        CombatEvents.InitializePlayerHP?.Invoke(combatManagerV3.playerStats.playerMaxHP);
        combatManagerV3.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
        combatManagerV3.playerStats.InitalisePlayerStats();


        combatManagerV3.SetState(new FirstMove(combatManagerV3));

        yield break;
    }

    public override void Update()
    {

    }

}

