using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatsDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI combatantNameTextMeshPro;
    [SerializeField] TextMeshProUGUI combatantHPTextMeshPro;
    public GameObject statsDisplayGameObject;
    [SerializeField] Animator animator;
    Combatant combatant;
    int combatantHP;

    public void ShowStatsDisplay(bool on)

    {
        if (on) {statsDisplayGameObject.SetActive(true); }
        if (!on) {statsDisplayGameObject.SetActive(false); }
    }

    public void InitializeStatsUI(Combatant combatant)
    {
        this.combatant = combatant;
        combatantNameTextMeshPro.text = combatant.combatantName;
        combatantHP = combatant.currentHP;
        combatantHPTextMeshPro.text = "HP: " + combatantHP;
    }

    public void UpdateHPDisplay(int value)
    {
        ShowStatsDisplay(true);

        if (combatant.currentHP <= 0)
        {
            combatantHPTextMeshPro.text = "DEAD";
        }

        else
        {
            StartCoroutine(UpdateHPDisplayCoroutine(combatantHP, value));
        }
    }

    IEnumerator UpdateHPDisplayCoroutine(int enemyHP, int value)

    {
        animator.SetTrigger("bump");
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
        ShowStatsDisplay(false);
    }
}
