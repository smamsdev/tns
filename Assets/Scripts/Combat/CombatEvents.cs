using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Mathematics;

public static class CombatEvents
{
    //Combat Setup
    public static Action BeginBattle;
    public static Action<bool> CameraBattleMode;
    public static Action LockPlayerMovement;
    public static Action UnlockPlayerMovement;
    public static Action<GameObject, Vector2, float>UpdateFighterPosition;

    //Combat UI
    public static Action<string> UpdateNarrator;
    public static Action<bool, bool, bool> HighlightBodypartTarget;
    public static Action<int> UpdateFendDisplay;
    public static Action<int> UpdatePlayerPotOnUI;
    public static Action<bool> ShowHideFendDisplay;
    public static Action<int> UpdatePlayerHPDisplay;
    public static Action<string> UpdateFirstMoveDisplay;
    public static Action<string> UpdateSecondMoveDisplay;
    public static Action<float> InputCoolDown;

    //Enemy
    public static Action<int> EnemyAttackPower;
    public static Action GetEnemyAttackPower;
    public static Action<int> InitializeenemyHP;
    public static Action<int> CalculateEnemyDamageTaken;
    public static Action<int> UpdateenemyHPUI;
    public static Action InitializeEnemyPartsHP;
    public static Action <int> SetEnemyBodyPartTarget;

    public static Action<string> UpdateTargetDisplayBodyDescription;
    public static Action<string> UpdateTargetDisplayArmsDescription;
    public static Action<string> UpdateTargetDisplayHeadDescription;

    public static Action<bool> EnemyIsDead;

    //Player
    public static Action<float, bool> UpdatePlayerAttackMoveMod;
    public static Action<float, bool> UpdatePlayerFendMoveMod;
    public static Action<float, int, bool> UpdatePlayerPotentialMoveCost;
    public static Action<int> UpdatePlayerPot;
    public static Action<int> InitializePlayerHP;
    public static Action<int> InitializePlayerPotUI;
    public static Action<int> UpdatePlayerHP;


}
