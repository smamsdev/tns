using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageTakenDisplay : MonoBehaviour
{
    [SerializeField] Animator animator;
    public TextMeshProUGUI damageTakenTextMeshProUGUI, backStabText;

    public IEnumerator ShowDamageDisplayCoro(int damage, bool isHeal = false)
    {

        float elapsedTime = 0f;
        float lerpDuration = .75f;
        int startNumber = 1;
        int valueToDisplay;

        if (isHeal)
        {
            animator.Play("HealingTaken");
        }
        else
        {
            animator.Play("DamageTaken");
        }

        while (elapsedTime < lerpDuration)
        {
            float t = Mathf.Clamp01(elapsedTime / lerpDuration);
            valueToDisplay = Mathf.RoundToInt(Mathf.Lerp(startNumber, damage, t));
            damageTakenTextMeshProUGUI.text = valueToDisplay.ToString();
            elapsedTime += Time.deltaTime;

            yield return null; 
        }

        yield return new WaitForSeconds(0.5f);
    }
}
