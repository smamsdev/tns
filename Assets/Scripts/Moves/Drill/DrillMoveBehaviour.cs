using System.Collections;
using UnityEditor.VersionControl;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class DrillMoveBehaviour : MoveBehaviour
{
    public StatModifier reduceMaxHP;
    public StatModifier increaseAttack;
    public StatModifier increaseFend;

    public override IEnumerator ApplyCustomMoveFunction()
    {
        int previousHP = combatantToAct.CurrentHP;
        int newMaxHP = Mathf.RoundToInt(combatantToAct.MaxHP + (combatantToAct.MaxHP * reduceMaxHP.amount));
        int delta = Mathf.Max(0, previousHP - newMaxHP);

        combatantToAct.ChangeStat(increaseAttack);
        combatantToAct.ChangeStat(increaseFend);

        StartCoroutine(combatantToAct.CombatHPChanged(-delta, combatManager));
        combatantToAct.ChangeStat(reduceMaxHP);
        combatantToAct.combatantUI.statsDisplay.UpdateHPDisplay(combatantToAct.CurrentHP);

        yield return null;
    }
}
