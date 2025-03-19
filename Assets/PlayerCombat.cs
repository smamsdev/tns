using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class PlayerCombat : Combatant
{

    public PlayerPermanentStats playerPermanentStats;

    [Header("HP")]
    public int playerCurrentHP;
    public int playerMaxHP;

    [Header("Fend")]
    public int playerFend;
    [SerializeField] float fendPotMod;

    [Header("Power")]
    public int attackPower;

    [SerializeField] float attackPowerPotMod;

    [Header("Potential")]
    public int currentPotential;
    public int maxPotential;

    [Header("Base Mods")]  
    [SerializeField] int attackPowerBaseMod;
    [SerializeField] int fendBaseMod;
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

    public void InitialiseStats()
    {
        playerCurrentHP = playerPermanentStats.maxHP;
        playerMaxHP = playerPermanentStats.maxHP;

        maxPotential = playerPermanentStats.maxPotential;
        currentPotential = maxPotential / 2;

        CombatEvents.InitializePlayerHPUI.Invoke(playerMaxHP);
        CombatEvents.InitializePlayerPotUI.Invoke(currentPotential);
    }

    public float CalculatePotentialMod()
    {
        float potentialMod = 0;

        if (currentPotential <= 0)
        {
            potentialMod = -0.1f;
        }

        if (currentPotential == maxPotential)
        {
            potentialMod = 2;
        }

        if (currentPotential > 0 && currentPotential < ( (float) (maxPotential/2) ))
        {   
            potentialMod = ((float)currentPotential / maxPotential) * 2.5f;
        }

        if (currentPotential< maxPotential && currentPotential >= ((float)(maxPotential / 2)))
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
        attackPower = Mathf.Max(0, potentialAttackPower);
    }

    public int TotalPlayerFendPower(float moveMod)
    {
        CalculatePotentialMod();

        playerFend = Mathf.Clamp(Mathf.RoundToInt(playerPermanentStats.fendBase * moveMod), 0, 9999);
        return playerFend;
    }

    public void UpdatePlayerPot(int value)
    {
        CurrentPotential += value;
        CombatEvents.UpdatePlayerPotOnUI(currentPotential);
    }

    public override void UpdateHP(int value)
    {
        PlayerCurrentHP += value;
    }

    public override void SelectMove()
    {
        throw new System.NotImplementedException();
    }

    public int PlayerCurrentHP
    {
        get { return playerCurrentHP; }
        set
        {
            playerCurrentHP = Mathf.Clamp(value, 0, 9999);
        }
    }

    public int CurrentPotential
    {
        get { return currentPotential; }
        set
        {
            currentPotential = Mathf.Clamp(value, 0, 9999);
        }
    }
}
