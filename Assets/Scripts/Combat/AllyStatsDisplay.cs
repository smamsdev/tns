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
        allyNameTextMeshPro.text = ally.allyName;
        allyHP = ally.currentHP;
        allyHPTextMeshPro.text = "HP: " + allyHP;
    }

    public void UpdateAllyHPDisplay(int newHPValue)
    {
        if (ally.currentHP <= 0)
        {
            allyHPTextMeshPro.text = "DEAD";
        }

        else
        {
            StartCoroutine(UpdateEnemyHPDisplayCoroutine(allyHP, newHPValue));
        }
    }

    IEnumerator UpdateEnemyHPDisplayCoroutine(int _allyHP, int newHPValue)

    {
        animator.SetTrigger("bump");

        float elapsedTime = 0f;
        float lerpDuration = 1f;
        int valueToOutput;

        while (elapsedTime < lerpDuration)
        {
            float t = Mathf.Clamp01(elapsedTime / lerpDuration);

            valueToOutput = Mathf.RoundToInt(Mathf.Lerp(_allyHP, newHPValue, t));
            allyHPTextMeshPro.text = "HP: " + valueToOutput.ToString();
            allyHP = valueToOutput;

            elapsedTime += Time.deltaTime;

            yield return null;
        }
    }
}
