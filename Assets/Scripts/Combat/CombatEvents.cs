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
    public static Action<State> PassState;

    //Combat UI
    public static Action<string> UpdateNarrator;
    public static Action<bool, bool, bool> HighlightBodypartTarget;
    public static Action<string, int, int, int> BodyPartDamageTakenDisplay;
    public static Action<int> UpdateFendDisplay;
    public static Action<int> UpdatePlayerPotOnUI;
    public static Action<int> UpdatePlayerHPDisplay;
    public static Action<string> UpdateFirstMoveDisplay;
    public static Action<string> UpdateSecondMoveDisplay;
    public static Action<float> InputCoolDown;
    public static Action <GameObject> ButtonHighlighted;

    //Enemy
    public static Action InitializeEnemyHP;
    public static Action<int> UpdateEnemyHPUI;
    public static Action InitializeEnemyPartsHP;
    public static Action<int> ShowEnemyDamageTakenDisplay;
    public static Action <int> SetEnemyBodyPartTarget;
    public static Action<int> UpdateEnemyAttackDisplay;
    public static Action<int> UpdateEnemyFendDisplay;

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
