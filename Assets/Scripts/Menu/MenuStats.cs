using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuStats : Menu
{
    [SerializeField] PlayerPermanentStats playerPermanentStats;

    [Header("Combat Stats UI Elements")]
    [SerializeField] private TextMeshProUGUI hpValue;
    [SerializeField] private TextMeshProUGUI potentialValue;
    [SerializeField] private TextMeshProUGUI strengthValue;
    [SerializeField] private TextMeshProUGUI defenceValue;
    [SerializeField] private TextMeshProUGUI focusValue;

    [Header("XP Stats UI Elements")]
    [SerializeField] private TextMeshProUGUI levelValue;
    [SerializeField] private TextMeshProUGUI experienceValue;
    [SerializeField] private TextMeshProUGUI nextLevelValue;

    public override void DisplayMenu(bool on)

    {
        InitializeStats();
        displayContainer.SetActive(on);
    }

    public override void EnterMenu()

    {

    }

    public override void ExitMenu()

    {

    }

    public override void StateUpdate()
    {
        //chill
    }

    public void InitializeStats()
    {
        hpValue.text = $"{playerPermanentStats.currentHP} / {playerPermanentStats.maxHP}";
        potentialValue.text = $"{playerPermanentStats.currentPotential} / {playerPermanentStats.maxPotential}";
        strengthValue.text = $"{playerPermanentStats.attackBase}";
        defenceValue.text = $"{playerPermanentStats.fendBase}";
        focusValue.text = $"{playerPermanentStats.focusBase}";

        levelValue.text = $"{playerPermanentStats.level}";
        experienceValue.text = $"{playerPermanentStats.XP}";
        nextLevelValue.text = $"{playerPermanentStats.XPThreshold}";
    }
}
