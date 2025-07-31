using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Mathematics;
using UnityEngine.UI;
using TMPro;

public static class CombatEvents
{
    //Combat Setup
    public static Action<bool> BattleMode;

    public static bool isBattleMode;
    public static Action LockPlayerMovement;
    public static Action UnlockPlayerMovement;
    public static Action<State> SendState;

    //Combat UI
    public static Action<string> UpdateNarrator;
    public static Action<int> UpdateFendDisplay;

    public static Action<float> InputCoolDown;
    public static Action<int> InventoryButtonHighlighted;
    public static Action<int> GearSlotButtonHighlighted;
    
    //Enemy

    public static Action<Combatant> CombatantisDefeated;

    //Player
    public static Action<float, bool> UpdatePlayerAttackMoveMod;
    public static Action<float, bool> UpdatePlayerFendMoveMod;
    public static Action<int> UpdatePlayerPot;
    public static Action<int> PlayerDamageDisplay;
    public static Action DisablePlayerDamageDisplay;
    public static Action PlayerDefeated;
    public static Action<int> SendMove;

}
