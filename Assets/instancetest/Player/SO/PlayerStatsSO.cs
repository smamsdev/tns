using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Events;
using System;

[CreateAssetMenu]

public class PlayerStatsSO : ScriptableObject
{

    private void OnEnable()
    {
        CombatEvents.UpdatePlayerFendMoveMod += UpdatePlayerFendMoveMod;
        CombatEvents.UpdatePlayerAttackMoveMod += UpdatePlayerAttackMoveMod;
        CombatEvents.UpdatePlayerHP += UpdatePlayerHP;
        CombatEvents.UpdatePlayerPotMoveMod += UpdatePlayerFocusMoveMod;
        CombatEvents.UpdatePlayerPot += PlayerPotChange;

    }

    private void OnDisable()
    {
        CombatEvents.UpdatePlayerFendMoveMod -= UpdatePlayerFendMoveMod;
        CombatEvents.UpdatePlayerAttackMoveMod += UpdatePlayerAttackMoveMod;
        CombatEvents.UpdatePlayerHP -= UpdatePlayerHP;
        CombatEvents.UpdatePlayerPotMoveMod -= UpdatePlayerFocusMoveMod;
        CombatEvents.UpdatePlayerPot -= PlayerPotChange;
    }

    [Header("HP")]
    public int playerCurrentHP;
    public int playerMaxHP;

    [Header("Fend")]
    public int playerFend;
    [SerializeField] float fendMoveMod;
    [SerializeField] int fendBase;
    [SerializeField] float fendPotMod;

    [Header("Power")]
    public int attackPower;
    [SerializeField] float attackPowerMoveMod;
    [SerializeField] int attackPowerBase;
    [SerializeField] float attackPowerPotMod;

    [Header("Potential")]
    [SerializeField] int playerCurrentPotential;
    [SerializeField] int playerMaxPotential;
    [SerializeField] int playerFocusbase;
    public float playerFocusMoveMod;

    [Header("Other")]
    [SerializeField] Vector2 position;
    public int enemyAttackPower;

    int basePlayerCurrentHP = 100;

    public int basePlayerAttackPowerBase = 8;
    int basePlayerAttackPowerMoveMod = 0;

    int  basePlayerMaxHP = 100;
    int basePlayerFendBase = 8;

    int basePlayerCurrentPotential = 50;
    int basePlayerMaxPotential = 100;
    int basePlayerFocusbase = 10;
    int basePlayerFocusMoveMod = 0;

    public void InitalisePlayerStats()
    {
        playerCurrentHP = basePlayerCurrentHP;


        attackPowerBase = basePlayerAttackPowerBase;
        attackPowerMoveMod = basePlayerAttackPowerMoveMod;

        playerMaxHP = basePlayerMaxHP;
        fendBase = basePlayerFendBase;

        playerCurrentPotential = basePlayerCurrentPotential;
        playerMaxPotential = basePlayerMaxPotential;

        playerFocusbase = basePlayerFocusbase;
        playerFocusMoveMod = basePlayerFocusMoveMod;

        enemyAttackPower = 0;

        UpdatePlayerAttackPower();
        CombatEvents.InitializePlayerPotDisplay.Invoke(playerCurrentPotential);

    }

    public void CheckForPotPunishment()
    {
        if (playerCurrentPotential == 0)
        {
            attackPowerPotMod = Mathf.CeilToInt(attackPowerBase * -0.75f);
            fendPotMod = Mathf.CeilToInt(fendBase * -0.75f);

            Debug.Log("punishing");
        }

        else
        {
            attackPowerPotMod = (100 - playerCurrentPotential) * -0.025f;
            fendPotMod = (100 - playerCurrentPotential) * -0.05f;
        }
    }

    public void UpdatePlayerAttackPower()
    {
        CheckForPotPunishment();
        attackPower = Mathf.Clamp(Mathf.CeilToInt(attackPowerBase + attackPowerMoveMod + attackPowerPotMod), 0, 9999);
        playerFend = Mathf.Clamp(Mathf.CeilToInt(fendBase + fendMoveMod + fendPotMod), 0, 9999);
        CombatEvents.UpdateFendDisplay(playerFend);
    }

    public void UpdatePlayerAttackMoveMod(float moveModMultiplier, bool isAttack)
    { if (isAttack)
        { attackPowerMoveMod = attackPowerBase * moveModMultiplier; }
    else { attackPowerMoveMod -= attackPowerBase; }

        UpdatePlayerAttackPower();
    }

    public void UpdatePlayerFendMoveMod(float moveModMultiplier, bool isFend)
    {
        if (isFend)
        {
            fendMoveMod = fendBase * moveModMultiplier;

        
        }
        else { fendMoveMod -= fendBase; }

        UpdatePlayerAttackPower();
    }

    public void UpdatePlayerFocusMoveMod(float moveModMultiplier, int focusMoveCost, bool isFocus)
    {
        if (isFocus)
        { playerFocusMoveMod = playerFocusbase * moveModMultiplier;

            playerFocusbase++;
            attackPowerBase++;
            fendBase++;
        }
        else { playerFocusMoveMod = focusMoveCost + (playerFocusbase * moveModMultiplier);

            CombatEvents.UpdatePlayerPot.Invoke(Mathf.CeilToInt(playerFocusMoveMod));
        }
    }

    public void PlayerPotChange(int value)
    {
        playerCurrentPotential = Mathf.Clamp(playerCurrentPotential+value, 0, 100);
    }

     void UpdatePlayerHP(int value)
    {
        playerCurrentHP -= value;
        CombatEvents.UpdatePlayerHPDisplay.Invoke(playerCurrentHP);
    }

    public void ResetAllMoveMods()

    {
        attackPowerMoveMod = 0;
        fendMoveMod = 0;
        playerFocusMoveMod = 0;
    }

}
      
