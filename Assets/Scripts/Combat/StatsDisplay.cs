using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatsDisplay : MonoBehaviour
{
    public TextMeshProUGUI combatantNameTextMeshPro;
    public TextMeshProUGUI combatantHPTextMeshPro;
    public GameObject statsDisplayGameObject;
    [SerializeField] Animator HPTMPanimator;
    public Combatant combatant;
    public int combatantHP;

    private void OnDisable()
    {
        ShowStatsDisplay(false);
    }

    public void ShowStatsDisplay(bool on)

    {
        statsDisplayGameObject.SetActive(on);
    }

    public void UpdateHPDisplay(int value)
    {
        if (!statsDisplayGameObject.activeSelf)
        {
            ShowStatsDisplay(true);
        }

        StartCoroutine(UpdateHPDisplayCoroutine(combatantHP, value));

        if (combatant.CurrentHP <= 0)
        {
            combatantHPTextMeshPro.text = "DEFEATED";
        }
    }

    IEnumerator UpdateHPDisplayCoroutine(int enemyHP, int value)
    {
        HPTMPanimator.SetTrigger("bump");
        var newHPValue = enemyHP + value;

        float elapsedTime = 0f;
        float lerpDuration = 1f;
        int valueToOutput;

        while (elapsedTime < lerpDuration)
        {
            float t = Mathf.Clamp01(elapsedTime / lerpDuration);

            valueToOutput = Mathf.RoundToInt(Mathf.Lerp(enemyHP, newHPValue, t));
            combatantHPTextMeshPro.text = "HP: " + valueToOutput.ToString();
            enemyHP = valueToOutput;

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
    }
}
