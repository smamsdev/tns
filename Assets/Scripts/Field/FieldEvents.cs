using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;

public static class FieldEvents
{
    public static Action<GameObject> HasCompleted;

    public static Action SceneChanging;

    public static Action<Enemy> UpdateXP;
    public static Action<RaycastHit2D> PlayerRayCastHit;

    public static bool isCameraFollow = true;
    public static bool isCoolDownBool;
    public static bool isDialogueActive;

    public static bool isSequenceRunning;
    public static bool movementLocked;

    public static bool freshScene;
    public static Vector3 entryCoordinates;

    public static float movementSpeedMultiplier;
    public static bool isMovementSpeedMultiplier;

    public static float horizontalInputToSave;

    public static string duration;
    public static string sceneName;

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

    public static void UpdateTime()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(Time.time);
        FieldEvents.duration = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
    }

}
