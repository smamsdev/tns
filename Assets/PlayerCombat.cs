using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class PlayerCombat : Combatant
{
    public PlayerPermanentStats playerPermanentStats;
    public PlayerMoveManager playerMoveManager;

    public int CurrentPotential
    {
        get { return currentPotential; }
        set
        {
            currentPotential = Mathf.Clamp(value, 0, 9999);
        }
    }

    [Header("Potential")]
    public int currentPotential;
    [SerializeField] float fendPotMod;
    [SerializeField] float attackPowerPotMod;

    [Header("Base Mods")]  
    [SerializeField] int playerFocusbaseMod;
    [SerializeField] int attackPowerGearMod;
    [SerializeField] int fendPowerGearMod;

    [SerializeField] int playerFocusbaseChange;
    [SerializeField] int attackPowerBaseChange;
    [SerializeField] int fendBaseChange;

    private void OnEnable()
    {
        CombatEvents.UpdatePlayerPot += UpdatePlayerPot;
    }

    private void OnDisable()
    {
        CombatEvents.UpdatePlayerPot -= UpdatePlayerPot;
    }

    public override void InitialiseCombatantStats()
    {
        PlayerStatsDisplay playerStatsDisplay = combatantUI.statsDisplay as PlayerStatsDisplay;

        CurrentHP = playerPermanentStats.currentHP;
        playerStatsDisplay.combatantHP = CurrentHP;
        playerStatsDisplay.combatantHPTextMeshPro.text = "HP: " + CurrentHP.ToString();

        CurrentPotential = (playerPermanentStats.maxPotential / 2);
        playerStatsDisplay.currentPotential = CurrentPotential;
        playerStatsDisplay.potentialTMP.text = "Potential: " + CurrentPotential.ToString();
    }

    public float CalculatePotentialMod()
    {
        float potentialMod = 0;

        if (currentPotential <= 0)
        {
            potentialMod = -0.1f;
        }

        if (currentPotential == playerPermanentStats.maxPotential)
        {
            potentialMod = 2;
        }

        if (currentPotential > 0 && currentPotential < ( (float) (playerPermanentStats.maxPotential /2) ))
        {   
            potentialMod = ((float)currentPotential / playerPermanentStats.maxPotential) * 2.5f;
        }

        if (currentPotential< playerPermanentStats.maxPotential && currentPotential >= ((float)(playerPermanentStats.maxPotential / 2)))
        {
            potentialMod = 1;
        }

        return potentialMod;
    }

    public void TotalPlayerAttackPower(float moveMod)
    {
        // Calculate the potential attack power based on various modifiers
        int potentialAttackPower = Mathf.RoundToInt(playerPermanentStats.attackPowerBase * moveMod * CalculatePotentialMod());

        // Ensure the attack power is not less than zero
        attackTotal = Mathf.Max(0, potentialAttackPower);
    }

    public int TotalPlayerFendPower(float moveMod)
    {
        CalculatePotentialMod();

        fendTotal = Mathf.Clamp(Mathf.RoundToInt(playerPermanentStats.fendBase * moveMod), 0, 9999);
        return fendTotal;
    }

    public void UpdatePlayerPot(int value)
    {
        currentPotential += value;
        PlayerStatsDisplay playerStatsDisplay = combatantUI.statsDisplay as PlayerStatsDisplay;
        StartCoroutine(playerStatsDisplay.UpdatePlayerPotentialUI(currentPotential));
    }

    public override void UpdateHP(int value)
    {
        CurrentHP += value;
        PlayerStatsDisplay playerStatsDisplay = combatantUI.statsDisplay as PlayerStatsDisplay;
        playerStatsDisplay.UpdateHPDisplay(CurrentHP);
    }

    public override void SelectMove()
    {
        throw new System.NotImplementedException();
    }
}
