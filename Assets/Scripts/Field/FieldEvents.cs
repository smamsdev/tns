using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using TMPro;

public static class FieldEvents
{
    public static Action<GameObject> HasCompleted;

    public static Action<RaycastHit2D> PlayerRayCastHit;
    public static bool fromEntryPoint;
    public static Vector3 positionOnEntry;

    public static bool isCoolDownBool;
    public static bool isDialogueActive;
    public static bool isSequenceRunning;

    public static bool movementLocked;

    public static string duration;

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

    public static TextMeshProUGUI FindLongestText(List<TextMeshProUGUI> textElementsToSort)
    {
        return textElementsToSort.OrderByDescending(text => text.preferredWidth).First();
    }

    public static void LerpValues(float initialValue, float finalValue, float lerpDuration, Action<float> callback)
    {
        var go = new GameObject("Runner");
        var coRoutineRunner = go.AddComponent<FieldEvents.CoroutineRunner>();
        coRoutineRunner.StartCoroutine(LerpValuesCoro(initialValue, finalValue, lerpDuration, callback, go));
    }

    public static IEnumerator LerpValuesCoro(float initialValue, float finalValue, float lerpDuration, Action<float> callback, GameObject coRoutineRunnerGO)
    {
        float elapsedTime = 0f;

        while (elapsedTime < lerpDuration)
        {
            float t = Mathf.Clamp01(elapsedTime / lerpDuration);
            float valueToOutput = Mathf.Lerp(initialValue, finalValue, t);

            callback(valueToOutput);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        callback(finalValue);
        GameObject.Destroy(coRoutineRunnerGO);
    }

    public class CoroutineRunner : MonoBehaviour { }

    public static void SetGridNavigationWrapAround(List<Button> buttons, int maxRows)
    {
        int buttonCount = buttons.Count;

        int rows = Mathf.Min(buttonCount, maxRows);
        int columns = Mathf.CeilToInt((float)buttonCount / rows);

        for (int i = 0; i < buttonCount; i++)
        {
            Button button = buttons[i];
            Navigation nav = button.navigation;
            nav.mode = Navigation.Mode.Explicit;

            int col = i / rows;
            int row = i % rows;

            int WrapIndex(int r, int c)
            {
                // wrap columns circularly
                if (c < 0) c = columns - 1;
                if (c >= columns) c = 0;

                int colStart = c * rows;
                int colEnd = Mathf.Min(colStart + rows, buttonCount); // last item in this column +1
                int index = colStart + r;

                // wrap within column
                if (index >= colEnd) index = colStart;
                if (index < colStart) index = colEnd - 1;

                return index;
            }

            nav.selectOnUp = buttons[WrapIndex(row - 1, col)];
            nav.selectOnDown = buttons[WrapIndex(row + 1, col)];
            nav.selectOnLeft = buttons[WrapIndex(row, col - 1)];
            nav.selectOnRight = buttons[WrapIndex(row, col + 1)];

            button.navigation = nav;
        }
    }
}
