using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Defeat : State
{
    [SerializeField] CombatManager combatManager;
    [SerializeField] GameObject combatUIContainer;

    public bool playerDefeated;

    public override IEnumerator StartState()
    {
        StopAllCoroutines();
        yield return new WaitForSeconds(1f);
        //defeated animation
        combatUIContainer.SetActive(false);
        yield return new WaitForSeconds(1f);
        FieldEvents.HasCompleted(this.gameObject);

        yield return null;
    }
}
