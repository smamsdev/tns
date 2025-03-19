using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AllyStatsDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI allyNameTextMeshPro;
    [SerializeField] TextMeshProUGUI allyHPTextMeshPro;
    public GameObject allyStatsDisplayGameObject;
    [SerializeField] Animator animator;
    Ally ally;
    int allyHP;

    public void ShowAllyStatsDisplay(bool on)

    {
        if (on) { allyStatsDisplayGameObject.SetActive(true); }
        if (!on) { allyStatsDisplayGameObject.SetActive(false); }
    }

    public void InitializeAllyStatsUI(Ally _ally)
    {
        ally = _ally;
        allyNameTextMeshPro.text = ally.combatantName;
        allyHP = ally.currentHP;
        allyHPTextMeshPro.text = "HP: " + allyHP;
    }

    public void UpdateAllyHPDisplay(int value)
    {
        ShowAllyStatsDisplay(true);

        if (ally.currentHP <= 0)
        {
            allyHPTextMeshPro.text = "DEAD";
        }

        else
        {
            StartCoroutine(UpdateAllyHPDisplayCoroutine(allyHP, value));
        }
    }

    IEnumerator UpdateAllyHPDisplayCoroutine(int enemyHP, int value)

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
            allyHPTextMeshPro.text = "HP: " + valueToOutput.ToString();
            enemyHP = valueToOutput;

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        ShowAllyStatsDisplay(false);
    }
}
