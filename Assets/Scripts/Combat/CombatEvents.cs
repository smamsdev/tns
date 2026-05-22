using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Mathematics;
using UnityEngine.UI;
using TMPro;

public static class CombatEvents
{
    public static Action LockPlayerMovement;
    public static Action UnlockPlayerMovement;
    public static Action<State> SendState;

    //Combat UI
    public static Action<int> UpdateFendDisplay;

    public static Action<float> InputCoolDown;
    public static Action<int> InventoryButtonHighlighted;
    public static Action<int> GearSlotButtonHighlighted;

    //Player
    public static Action<float, bool> UpdatePlayerAttackMoveMod;
    public static Action<float, bool> UpdatePlayerFendMoveMod;
    public static Action<int> PlayerDamageDisplay;
    public static Action DisablePlayerDamageDisplay;
    public static Action PlayerDefeated;
    public static Action<int> SendMove;
}
