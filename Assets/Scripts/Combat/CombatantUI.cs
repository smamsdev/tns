using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatantUI : MonoBehaviour
{
    public StatsDisplay statsDisplay;
    public AttackDisplay attackDisplay;
    public FendScript fendScript;
    public DamageTakenDisplay damageTakenDisplay;
    public Animator selectedAnimator;
    public GameObject combatUIContainer;

    private void OnEnable()
    {
        combatUIContainer.SetActive(false);
    }

    public void DisplayCombatantMove(Combatant combatant)
    {
        combatant.combatantUI.attackDisplay.ShowAttackDisplay(combatant, true);
        combatant.combatantUI.fendScript.ShowFendDisplay(combatant, true);
    }

    //public void HideCombatantMove(Combatant combatant)
    //{
    //    combatant.combatantUI.attackDisplay.attackDisplayAnimator.Play("CombatantAttackDamageHidden");
    //    combatant.combatantUI.fendScript.fendAnimator.Play("FendDefault");
    //    Debug.Log("asdasd");
    //}
}