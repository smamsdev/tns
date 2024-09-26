using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;

public static class FieldEvents
{
    public static Action<GameObject> HasCompleted;

    public static Action<GameObject> UpdateXP;
    public static Action<RaycastHit2D> PlayerRayCastHit;

    public static bool isCameraFollow = true;
    public static bool isCoolDownBool;
    public static bool isDialogueActive;

    public static bool isSequenceRunning;

    public static Vector2 lookDirection;

    public static bool freshScene;
    public static Vector2 entryCoordinates;

    public static float movementSpeedMultiplier;
    public static bool isMovementSpeedMultiplier;

    public static float horizontalInputToSave;

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
