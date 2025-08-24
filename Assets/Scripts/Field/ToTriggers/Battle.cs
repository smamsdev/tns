using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Battle : ToTrigger
{
    public List<Combatant> allies;
    public List<Combatant> enemies;
    public Transform battleCenterPosition;
    public GameObject playerFightingPosition;
    public CombatManager combatManager;
    public Vector2 playerDefaultLookDirection;
    public bool isEnemyFlanked =false;
    public bool isAllyFlanked = false;


    private void OnEnable()
    {
        combatManager = GameObject.FindGameObjectWithTag("CombatManager").GetComponent<CombatManager>();
    }

    public override IEnumerator DoAction()
    {
        combatManager.battleScheme = this;
        combatManager.StartBattle();

        FieldEvents.HasCompleted.Invoke(this.gameObject);

        yield return null;
    }
}

