using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using static UnityEngine.Rendering.DebugUI;

public class CombatUIPlayerHP : MonoBehaviour
{
    TextMeshProUGUI textMeshProUGUI;
    [SerializeField] Animator animator;
    public int playerCurrentHP;

    private void Awake()
    {
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        CombatEvents.UpdatePlayerHPDisplay += UpdatePlayerHP;
        CombatEvents.InitializePlayerHPUI += InitializePlayerHP;
    }

    private void OnDisable()
    {
        CombatEvents.UpdatePlayerHPDisplay -= UpdatePlayerHP;
        CombatEvents.InitializePlayerHPUI -= InitializePlayerHP;
    }

    void UpdatePlayerHP(int finalValue)
    {

        StartCoroutine(UpdatePlayerHPDisplayCoroutine(playerCurrentHP,finalValue));
    }

    void InitializePlayerHP(int value)
    {
        playerCurrentHP = value;
        textMeshProUGUI.text = "HP: " + value.ToString();
    }

    IEnumerator UpdatePlayerHPDisplayCoroutine(int currentHP, int finalValue)

    {
        //Debug.Log(currentHP);
        //Debug.Log(finalValue);

        animator.SetTrigger("bump");

        float elapsedTime = 0f;
        float lerpDuration = 1f;
        int valueToOutput;

        while (elapsedTime < lerpDuration)
        {
            float t = Mathf.Clamp01(elapsedTime / lerpDuration);

            valueToOutput = Mathf.RoundToInt(Mathf.Lerp(currentHP, finalValue, t));
            textMeshProUGUI.text = "HP: " + valueToOutput.ToString();
            playerCurrentHP = valueToOutput;

            elapsedTime += Time.deltaTime;

            yield return null;
        }
    }
}
