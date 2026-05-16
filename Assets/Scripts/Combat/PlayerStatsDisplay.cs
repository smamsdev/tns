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
    public int currentPotential;

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

    public IEnumerator UpdatePlayerPotentialUI(int change)
    {
        if (change >= 0)
            potentialTMPAnimator.Play("CombatUIStatPlus");

        else
            potentialTMPAnimator.Play("CombatUIStatMinus");

        float current = currentPotential;
        float finalValue = currentPotential += change;
        float elapsedTime = 0f;
        float lerpDuration = 0.5f;
        int valueToOutput;
        var playerCombatant = combatant as PlayerCombat;

        while (elapsedTime < lerpDuration)
        {
            float t = Mathf.Clamp01(elapsedTime / lerpDuration);

            valueToOutput = Mathf.RoundToInt(Mathf.Lerp(current, finalValue, t));
            potentialTMP.text = valueToOutput.ToString() + " / " + playerCombatant.MaxPotential;

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        currentPotential = Mathf.RoundToInt(finalValue);
    }

    public override void InitialiseCombatStatsDisplay(Combatant combatant)
    {
        combatantHPTextMeshPro.text = combatant.CurrentHP.ToString() + " / " + combatant.MaxHP;

        var playerCombatant = combatant as PlayerCombat;

        currentPotential = playerCombatant.CurrentPotential;
        potentialTMP.text = playerCombatant.CurrentPotential.ToString() + " / " + playerCombatant.MaxPotential; 
    }
}
