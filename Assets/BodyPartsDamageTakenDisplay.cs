using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BodyPartsDamageTakenDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI bodyPartDamageTakenTextMeshProUGUI;

    public void BodyPartDamageTakenDisplay(string partName, int startValue, int endValue, int maxHP)

    {   
        StartCoroutine(BodyPartDamageTakenDisplayCoroutine(partName, startValue, endValue, maxHP));
    }

    private void Start()
    {
        if (bodyPartDamageTakenTextMeshProUGUI.enabled)
        {
            bodyPartDamageTakenTextMeshProUGUI.enabled = false;
        }
    }

    IEnumerator BodyPartDamageTakenDisplayCoroutine(string partName, int startValue, int endValue, int maxHP)

    {
        //animator.SetTrigger("bump");
        bodyPartDamageTakenTextMeshProUGUI.enabled = true;

        float elapsedTime = 0f;
        float lerpDuration = 0.5f;
        int valueToOutput;

        while (elapsedTime < lerpDuration)
        {


            float t = Mathf.Clamp01(elapsedTime / lerpDuration);

            valueToOutput = Mathf.RoundToInt(Mathf.Lerp(startValue, endValue, t));

            bodyPartDamageTakenTextMeshProUGUI.text = partName + "<br>" + valueToOutput.ToString() + "/" + maxHP;

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        yield return new WaitForSeconds(1f);
        bodyPartDamageTakenTextMeshProUGUI.enabled = false;
    }
}
