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
    public bool isEnemyFlanked =false;
    public bool isAllyFlanked = false;
    public bool isRandomEnounter;


    private void OnEnable()
    {
        combatManager = GameObject.FindGameObjectWithTag("CombatManager").GetComponent<CombatManager>();
    }

    public override IEnumerator TriggerFunction()
    {
        combatManager.battleScheme = this;
        combatManager.StartBattle();
        yield return null;
    }
}

