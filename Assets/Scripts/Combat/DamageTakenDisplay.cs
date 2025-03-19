using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageTakenDisplay : MonoBehaviour
{
    [SerializeField] Animator animator;
    public TextMeshProUGUI damageTakenTextMeshProUGUI;

    void Start()
    {
        damageTakenTextMeshProUGUI.enabled = false;
    }

    public void ShowDamageDisplay(int remainder)
    {
        animator.SetInteger("animState", 1);
        damageTakenTextMeshProUGUI.enabled = true;
        StartCoroutine(ShowDamageDisplayCoroutine(remainder));
    }

    IEnumerator ShowDamageDisplayCoroutine(int damage)
    {
        float elapsedTime = 0f;
        float lerpDuration = 0.5f;
        int startNumber = 1;
        int endValue = damage;
        int valueToDisplay;

        while (elapsedTime < lerpDuration)
        {
            float t = Mathf.Clamp01(elapsedTime / lerpDuration);

            valueToDisplay = Mathf.RoundToInt(Mathf.Lerp(startNumber, endValue, t));
            damageTakenTextMeshProUGUI.text = valueToDisplay.ToString();

            elapsedTime += Time.deltaTime;

            yield return null;
        }
    }

    public void DisableDamageDisplay()
    {
        animator.SetInteger("animState", 0);
    }
}
