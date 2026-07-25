using System.Collections;
using UnityEngine;

public class BasicMoveWithStatChange : BasicMove
{
    public StatModifier statChange;
    public float randomnessPer;

    public override IEnumerator ApplyCustomMoveFunction()
    {
        float valueMax = statChange.amount + (Mathf.Abs(randomnessPer) * statChange.amount);
        float valueMin = statChange.amount - (Mathf.Abs(randomnessPer) * statChange.amount);
        float valueWithRandomFactor = Random.Range(valueMin, valueMax);

        statChange.amount = valueWithRandomFactor;

        combatantToAct.targetCombatant.ChangeStat(statChange);
        yield return null;
    }
}
