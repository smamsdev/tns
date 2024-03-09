using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BattleTrigger : ToTrigger
{
    [SerializeField] Battle battleToTrigger;
    public CombatManager combatManager;
    public GameObject combatManagerGO;


    public override IEnumerator DoAction()
    {
        combatManager = FindObjectOfType<CombatManager>(true);
        combatManagerGO = combatManager.gameObject;
        combatManagerGO.SetActive(true);



        combatManager.battleScheme = battleToTrigger;

        combatManager.StartBattle();

        yield return null;
    }
}

