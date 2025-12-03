using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class PlayerCombat : PartyMemberCombat
{
    public PlayerMoveManager playerMoveManager;
    public PlayerPermanentStats playerPermanentStats;
    public PartySO party;
    public PlayerInventory playerInventory;

    public int MaxPotential
    {
        get { return maxPotential; }
        set
        {
            maxPotential = Mathf.Clamp(value, 0, 9999);
        }
    }

    [SerializeField] private int maxPotential;

    public int CurrentPotential
    {
        get { return currentPotential; }
        set
        {
            currentPotential = Mathf.Clamp(value, 0, 9999);
        }
    }

    [SerializeField] private int currentPotential;
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

        if (currentPotential == playerPermanentStats.MaxPotential)
        {
            potentialMod = 2;
        }

        if (currentPotential > 0 && currentPotential < ( (float) (playerPermanentStats.MaxPotential /2) ))
        {   
            potentialMod = ((float)currentPotential / playerPermanentStats.MaxPotential) * 2.5f;
        }

        if (currentPotential< playerPermanentStats.MaxPotential && currentPotential >= ((float)(playerPermanentStats.MaxPotential / 2)))
        {
            potentialMod = 1;
        }

        return potentialMod;
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
