using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Battle : ToTrigger
{
    public List<Ally> allies;
    public List<Enemy> enemies;

    public GameObject playerFightingPosition;
    public GameObject combatManager;
    public Vector2 playerDefaultLookDirection;

    private void Start()
    {
        combatManager = GameObject.FindGameObjectWithTag("CombatManager");
    }

    public override IEnumerator DoAction()
    {
        var combatManagerscript = combatManager.GetComponent<CombatManager>();

        combatManagerscript.battleScheme = this;
        combatManagerscript.StartBattle();

        FieldEvents.HasCompleted.Invoke(this.gameObject);

        yield return null;
    }
}

