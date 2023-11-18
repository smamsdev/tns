using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;

public static class FieldEvents
{
    public static Action<GameObject> HasCompleted;
    public static Action<string, bool> ActorActionHasStarted;
    public static Action<string, bool> ActorActionHasCompleted;
    public static Action<bool, float> IsWalkwayBoost;

    public static Action<GameObject> HasBeenDefeated;
    public static Action<RaycastHit2D> PlayerRayCastHit;

    static bool isCoolDownBool;
    public static bool isDialogueActive;

    public static bool objectFetched;
    public static Vector2 playerLastKnownPos;
    public static Vector2 lookDirection;

    public static bool freshScene;



    public static bool isCooldown()

    {
        return isCoolDownBool; 
    }
    
    public static IEnumerator CoolDown(float seconds)
    {
        isCoolDownBool = true;
        yield return new WaitForSeconds(seconds);
        isCoolDownBool = false;
        isCooldown();
    }

        
}
