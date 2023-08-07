using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Mathematics;

public static class CombatEvents
{
    //Combat UI
    public static Action<string> UpdateNarrator;
    public static Action<bool, bool, bool> HighlightBodypartTarget;
    public static Action<int> UpdateFendDisplay;
    public static Action<bool> ShowHideFendDisplay;
    public static Action<int> UpdatePlayerHPDisplay;

    //Enemy
    public static Action IsEnemyDefeated;
    public static Action EnemyIsDefeated;
    public static Action<int> EnemyAttackPower;
    public static Action GetEnemyAttackPower;
    public static Action<int> InitializeEnemyHP;
    public static Action<int> UpdateEnemyHP;
    public static Action InitializePartsHP;
    public static Action <int> SetEnemyBodyPartTarget;

    public static Action<string> UpdateTargetDisplayBodyDescription;
    public static Action<string> UpdateTargetDisplayArmsDescription;
    public static Action<string> UpdateTargetDisplayHeadDescription;


    //Player
    public static Action<float, bool> UpdatePlayerAttackMoveMod;
    public static Action<float, bool> UpdatePlayerFendMoveMod;
    public static Action<float, int, bool> UpdatePlayerPotMoveMod;
    public static Action<int> UpdatePlayerPot;
    public static Action<int> InitializePlayerHP;
    public static Action<int> InitializePlayerPotDisplay;
    public static Action<int> UpdatePlayerHP;


}
