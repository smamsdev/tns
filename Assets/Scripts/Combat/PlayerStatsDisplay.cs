using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;

public class PlayerStatsDisplay : StatsDisplay
{
    public TextMeshProUGUI potentialTMP;
    [SerializeField] Animator potentialTMPAnimator;
    public float currentPotential;

    public override void ShowStatsDisplay(bool on)
    {
        return;
    }

    public override void ShowCombatantName(bool on)
    {
        return;
    }

    public override void UpdateHPDisplay(int value)
    {
        combatantHPTextMeshPro.text = combatant.CurrentHP.ToString() + " / " + combatant.MaxHP;

        if (combatant.CurrentHP <= 0)
            Debug.LogError("player just died do someth");
    }

    public IEnumerator UpdatePlayerPotentialUI(int change, int maxPot)
    {
        if (change >= 0)
            potentialTMPAnimator.Play("CombatUIStatPlus");

        else
            potentialTMPAnimator.Play("CombatUIStatMinus");

        float current = currentPotential;
        float finalValue = currentPotential += change;
        currentPotential = finalValue;
        float lerpDuration = 0.5f;
        var playerCombatant = combatant as PlayerCombat;

        StartCoroutine(FieldEvents.LerpValuesCoRo(current, finalValue, lerpDuration, (output) =>
        {
            int newValue = Mathf.Clamp((Mathf.RoundToInt(output)), 0, maxPot);
            potentialTMP.text = newValue + " / " + maxPot;
        }
        ));

        yield return null;
    }

    public override void InitialiseCombatStatsDisplay(Combatant combatant)
    {
        combatantHPTextMeshPro.text = combatant.CurrentHP.ToString() + " / " + combatant.MaxHP;

        var playerCombatant = combatant as PlayerCombat;

        currentPotential = playerCombatant.CurrentPotential;
        potentialTMP.text = playerCombatant.CurrentPotential.ToString() + " / " + playerCombatant.MaxPotential; 
    }
}
