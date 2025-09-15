using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class PlayerCombat : Ally
{
    public PlayerMoveManager playerMoveManager;
    public PlayerPermanentStats playerPermanentStats;
    public PartySO party;
    public PlayerInventory playerInventory;

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
    public int focusBase;
    public float fendPotMod;
    public float attackPowerPotMod;

    private void OnEnable()
    {
        CombatEvents.UpdatePlayerPot += UpdatePlayerPot;
        movementScript = GetComponent<MovementScript>();
    }

    private void OnDisable()
    {
        CombatEvents.UpdatePlayerPot -= UpdatePlayerPot;
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
        int potentialAttackPower = Mathf.RoundToInt(playerPermanentStats.attackBase * moveMod * CalculatePotentialMod());

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

    public override void SelectMove()
    {
        throw new System.NotImplementedException();
    }
}
