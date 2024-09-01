using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Battle : ToTrigger
{
    public GameObject[] enemyGameObject;
    public GameObject playerFightingPosition;

    public GameObject combatManager;

    public override IEnumerator DoAction()
    {
        combatManager = GameObject.Find("CombatManager");
        var combatManagerscript = combatManager.GetComponent<CombatManager>();

        combatManagerscript.battleScheme = this;
        combatManagerscript.StartBattle();

        FieldEvents.HasCompleted.Invoke(this.gameObject);

        yield return null;
    }
}

