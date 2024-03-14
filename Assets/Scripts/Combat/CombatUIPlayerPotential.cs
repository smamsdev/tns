using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class CombatUIPlayerPotential : MonoBehaviour
{
    TextMeshProUGUI textMeshProUGUI;
    int playerCurrentPotential;
    [SerializeField] Animator animator;

    private void Awake()
    {
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }


    private void OnEnable()
    {
        CombatEvents.UpdatePlayerPotOnUI += UpdatePlayerPotOnUI;
        CombatEvents.InitializePlayerPotUI += InitializePlayerPotUI;
    }

    private void OnDisable()
    {
        CombatEvents.UpdatePlayerPotOnUI -= UpdatePlayerPotOnUI;
        CombatEvents.InitializePlayerPotUI -= InitializePlayerPotUI;
    }

    public void InitializePlayerPotUI(int value)
    {
        playerCurrentPotential = value;
        textMeshProUGUI.text = "Potential: " + playerCurrentPotential.ToString();

    }

    public void UpdatePlayerPotOnUI(int newValue)
    {
        StartCoroutine(UpdatePlayerPotentialDisplayCoroutine(playerCurrentPotential, newValue));
    }


    IEnumerator UpdatePlayerPotentialDisplayCoroutine(int currentHP, int finalValue)

    {
        animator.SetTrigger("bump");

        float elapsedTime = 0f;
        float lerpDuration = 1f;
        int valueToOutput;

        while (elapsedTime < lerpDuration)
        {
            float t = Mathf.Clamp01(elapsedTime / lerpDuration);

            valueToOutput = Mathf.RoundToInt(Mathf.Lerp(currentHP, finalValue, t));
            textMeshProUGUI.text = "Potential: " + valueToOutput.ToString();
            playerCurrentPotential = valueToOutput;

            elapsedTime += Time.deltaTime;

            yield return null;
        }
    }

}
