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
    public static Action StartScene;

    public static Action<RaycastHit2D> PlayerRayCastHit;

    public static bool isCameraFollow = true;
    public static bool isCoolDownBool;
    public static bool isDialogueActive;
    public static bool isSequenceRunning;

    public static bool movementLocked;

    public static Vector3 entryCoordinates;

    public static float movementSpeedMultiplier;
    public static bool isMovementSpeedMultiplier;

    //Stats for saving
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
    }

    public static void UpdateTime()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(Time.time);
        FieldEvents.duration = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
    }

    public static void LerpValues(int initialValue, int finalValue, float lerpDuration, Action<int> callback)
    {
        var go = new GameObject("Runner");
        var coRoutineRunner = go.AddComponent<FieldEvents.CoroutineRunner>();
        coRoutineRunner.StartCoroutine(LerpValuesCoro(initialValue, finalValue, lerpDuration, callback, go));
    }

    public static IEnumerator LerpValuesCoro(int initialValue, int finalValue, float lerpDuration, Action<int> callback, GameObject coRoutineRunnerGO)
    {
        float elapsedTime = 0f;

        while (elapsedTime < lerpDuration)
        {
            float t = Mathf.Clamp01(elapsedTime / lerpDuration);
            int valueToOutput = Mathf.RoundToInt(Mathf.Lerp(initialValue, finalValue, t));

            callback(valueToOutput);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        callback(finalValue);
        GameObject.Destroy(coRoutineRunnerGO);
    }

    public class CoroutineRunner : MonoBehaviour { }
}
