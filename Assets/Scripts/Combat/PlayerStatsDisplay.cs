using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStatsDisplay : StatsDisplay
{
    public TextMeshProUGUI potentialTMP;
    [SerializeField] Animator potentialTMPAnimator;
    public int currentPotential;

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
}
