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
    public static Action LockPlayerMovement;
    public static Action UnlockPlayerMovement;
    public static Action<State> SendState;

    //Combat UI
    public static Action<string> UpdateNarrator;
    public static Action<int> UpdateFendDisplay;

    public static Action<int> UpdatePlayerHPDisplay;
    public static Action<string> UpdateFirstMoveDisplay;
    public static Action<string> UpdateSecondMoveDisplay;
    public static Action<float> InputCoolDown;
    public static Action <GameObject> ButtonHighlighted;

    //Enemy
    public static Action <int> SetEnemyBodyPartTarget;
    public static Action<int> UpdateEnemyAttackDisplay;

    public static Action<bool> EnemyIsDead;

    //Player
    public static Action<float, bool> UpdatePlayerAttackMoveMod;
    public static Action<float, bool> UpdatePlayerFendMoveMod;
    public static Action<float, int, bool> UpdatePlayerPotentialMoveCost;
    public static Action<int> UpdatePlayerPot;
    public static Action<int> InitializePlayerHP;
    public static Action<int> UpdatePlayerPotOnUI;
    public static Action<int> InitializePlayerPotUI;
    public static Action<int> UpdatePlayerHP;
    public static Action<int> PlayerDamageDisplay;
    public static Action DisablePlayerDamageDisplay;
    public static Action<int> SendMove;

    //ApplyMove events

    public static Action MeleeAttack;
    public static Action CounterAttack;
    public static Action EndMove;

}
