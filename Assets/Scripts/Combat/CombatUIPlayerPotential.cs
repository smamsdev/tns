using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CombatUIPlayerPotential : MonoBehaviour
{
    TextMeshProUGUI textMeshProUGUI;
    int playerCurrentPotential;

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


    public void UpdatePlayerPotOnUI(int value)
    {
        playerCurrentPotential = value;
        textMeshProUGUI.text = "Potential: " + playerCurrentPotential.ToString();
    }

}
