using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CombatUIPlayerHP : MonoBehaviour
{

    TextMeshProUGUI textMeshProUGUI;
    int playerCurrentHP;

    private void Awake()
    {
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        CombatEvents.UpdatePlayerHPDisplay += UpdatePlayerHP;
        CombatEvents.InitializePlayerHP += InitializePlayerHP;
    }

    private void OnDisable()
    {
        CombatEvents.UpdatePlayerHPDisplay -= UpdatePlayerHP;
        CombatEvents.InitializePlayerHP -= InitializePlayerHP;
    }

    void UpdatePlayerHP(int value)
    {

        playerCurrentHP = value;
        textMeshProUGUI.text = "HP: " + playerCurrentHP.ToString();
    }

    void InitializePlayerHP(int value)
    {
        playerCurrentHP = value;
        textMeshProUGUI.text = "HP: " + value.ToString();
    }
}
