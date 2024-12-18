using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerDamageTakenDisplay : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] TextMeshProUGUI textMeshProUGUI;

    private void OnEnable()
    {
        CombatEvents.PlayerDamageDisplay += ShowPlayerDamageDisplay;
    }

    private void OnDisable()
    {
        CombatEvents.PlayerDamageDisplay -= ShowPlayerDamageDisplay;
    }

    public void ShowPlayerDamageDisplay(int value)

    {
        animator.SetBool("damageTaken", true);
        textMeshProUGUI.enabled = true;
        StartCoroutine(ShowPlayerDamageDisplayCoRo(value));
    }

    IEnumerator ShowPlayerDamageDisplayCoRo(int damage)

    {
        float elapsedTime = 0f;
        float lerpDuration = 0.5f;
        int startNumber = 1;
        int endValue = damage;
        int valueToDisplay;

        while (elapsedTime < lerpDuration)
        {
            // Calculate the interpolation factor between 0 and 1 based on the elapsed time and duration
            float t = Mathf.Clamp01(elapsedTime / lerpDuration);

            // Lerp between the start and end values
            valueToDisplay = Mathf.RoundToInt(Mathf.Lerp(startNumber, endValue, t));
            textMeshProUGUI.text = valueToDisplay.ToString();

            // Increment the elapsed time
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        animator.SetBool("damageTaken", false);
    }

}
