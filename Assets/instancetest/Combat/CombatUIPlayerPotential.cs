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
        CombatEvents.UpdatePlayerPot += PlayerPotChange;
        CombatEvents.InitializePlayerPotDisplay += InitializePlayerPotDisplay;
    }

    private void OnDisable()
    {
        CombatEvents.UpdatePlayerPot -= PlayerPotChange;
        CombatEvents.InitializePlayerPotDisplay -= InitializePlayerPotDisplay;
    }

    public void InitializePlayerPotDisplay(int value)
    {
        playerCurrentPotential = value;
        textMeshProUGUI.text = "Potential: " + playerCurrentPotential.ToString();
    }


    public void PlayerPotChange(int value)
    {
        playerCurrentPotential = Mathf.Clamp(playerCurrentPotential + value, 0, 100);
        textMeshProUGUI.text = "Potential: " + playerCurrentPotential.ToString();
    }

}
