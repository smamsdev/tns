using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public override void UpdateHPDisplay(int value)
    {
        combatantHPTextMeshPro.text = "HP: " + value.ToString();

        if (combatant.CurrentHP <= 0)
        {
            Debug.LogError("player just died do someth");
        }
    }

    public IEnumerator UpdatePlayerPotentialUI(int newValue)
    {
        potentialTMPAnimator.SetTrigger("bump");

        float elapsedTime = 0f;
        float lerpDuration = 0.5f;
        int valueToOutput;

        while (elapsedTime < lerpDuration)
        {
            float t = Mathf.Clamp01(elapsedTime / lerpDuration);

            valueToOutput = Mathf.RoundToInt(Mathf.Lerp(currentPotential, newValue, t));
            potentialTMP.text = "Potential: " + valueToOutput.ToString();

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        currentPotential = newValue;
    }

    public override void InitialiseCombatStatsDisplay(Combatant combatant)
    {
        combatantHP = combatant.CurrentHP;
        combatantHPTextMeshPro.text = "HP: " + combatant.CurrentHP.ToString();

        var playerCombatant = combatant as PlayerCombat;

        currentPotential = playerCombatant.CurrentPotential;
        potentialTMP.text = "Potential: " + playerCombatant.CurrentPotential.ToString();
    }
}
