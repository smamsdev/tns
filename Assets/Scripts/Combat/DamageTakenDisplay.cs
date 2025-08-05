using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageTakenDisplay : MonoBehaviour
{
    [SerializeField] Animator animator;
    public TextMeshProUGUI damageTakenTextMeshProUGUI, backStabText;

    void Start()
    {
        damageTakenTextMeshProUGUI.enabled = false;
    }

    public IEnumerator ShowDamageDisplayCoro(int damage)
    {
        float elapsedTime = 0f;
        float lerpDuration = .75f;
        int startNumber = 1;
        int valueToDisplay;

        animator.SetInteger("animState", 1);
        damageTakenTextMeshProUGUI.enabled = true;

        while (elapsedTime < lerpDuration)
        {
            float t = Mathf.Clamp01(elapsedTime / lerpDuration);
            valueToDisplay = Mathf.RoundToInt(Mathf.Lerp(startNumber, damage, t));
            damageTakenTextMeshProUGUI.text = valueToDisplay.ToString();
            elapsedTime += Time.deltaTime;

            yield return null; 
        }

        yield return new WaitForSeconds(0.5f);
        animator.SetInteger("animState", 0);
    }
}
