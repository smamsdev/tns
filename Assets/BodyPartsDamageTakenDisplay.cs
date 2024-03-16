using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BodyPartsDamageTakenDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMeshProUGUI;

    private void OnEnable()
    {
        CombatEvents.BodyPartDamageTakenDisplay += BodyPartDamageTakenDisplay;
    }

    private void OnDisable()
    {
        CombatEvents.BodyPartDamageTakenDisplay -= BodyPartDamageTakenDisplay;
    }

    public void BodyPartDamageTakenDisplay(string partName, int startValue, int endValue, int maxHP)

    {   
        StartCoroutine(BodyPartDamageTakenDisplayCoroutine(partName, startValue, endValue, maxHP));
    }

    IEnumerator BodyPartDamageTakenDisplayCoroutine(string partName, int startValue, int endValue, int maxHP)

    {
        //animator.SetTrigger("bump");

        float elapsedTime = 0f;
        float lerpDuration = 0.5f;
        int valueToOutput;

        while (elapsedTime < lerpDuration)
        {
            textMeshProUGUI.enabled = true;

            float t = Mathf.Clamp01(elapsedTime / lerpDuration);

            valueToOutput = Mathf.RoundToInt(Mathf.Lerp(startValue, endValue, t));

            textMeshProUGUI.text = partName + "<br>" + valueToOutput.ToString() + "/" + maxHP;

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        yield return new WaitForSeconds(1f);
        textMeshProUGUI.enabled = false;
    }
}
