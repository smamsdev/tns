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
    public static string sceneName;

    public static bool isCoolDownBool;
    public static bool isDialogueActive;
    public static bool isSequenceRunning;

    public static bool menuAvailable = false;
    public static bool movementLocked = false;

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

    public static TextMeshProUGUI FindLongestText(List<TextMeshProUGUI> textElementsToSort)
    {
        return textElementsToSort.OrderByDescending(text => text.preferredWidth).First();
    }

    public static IEnumerator LerpValuesCoRo(float initialValue, float finalValue, float duration, Action<float> callback)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = Mathf.Clamp01(elapsedTime / duration);
            float value = Mathf.Lerp(initialValue, finalValue, t);
            callback(value);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        callback(finalValue);
    }

    //assumes a vertical major button axis
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

            nav.selectOnUp = buttons[WrapIndex(row - 1, col)];
            nav.selectOnDown = buttons[WrapIndex(row + 1, col)];
            nav.selectOnLeft = buttons[WrapIndex(row, col - 1)];
            nav.selectOnRight = buttons[WrapIndex(row, col + 1)];

            button.navigation = nav;

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
        }
    }

    // Assumes a horizontal major button axis (wraps left/right and up/down)
    public static void SetGridNavigationWrapAroundHorizontal(List<Button> buttons, int maxColumns)
    {
        int buttonCount = buttons.Count;
        int columns = Mathf.Min(buttonCount, maxColumns);
        int rows = Mathf.CeilToInt((float)buttonCount / columns);

        for (int i = 0; i < buttonCount; i++)
        {
            Button buttonToSet = buttons[i];
            Navigation nav = buttonToSet.navigation;
            nav.mode = Navigation.Mode.Explicit;

            int row = i / columns;
            int col = i % columns;

            nav.selectOnLeft = buttons[WrapHorizontal(row, (col - 1), buttonToSet)];
            nav.selectOnRight = buttons[WrapHorizontal(row, (col + 1), buttonToSet)];
            nav.selectOnUp = buttons[WrapVertical(row - 1, col)];
            nav.selectOnDown = buttons[WrapVertical(row + 1, col)];

            buttonToSet.navigation = nav;
        }

        //
        int WrapHorizontal(int row, int col, Button buttonToSet)
        {
            int rowStart = row * columns;
            int rowEnd = Mathf.Min(rowStart + columns, buttonCount);

            if (col < 0) col = rowEnd - rowStart - 1; 
            if (col >= rowEnd - rowStart) col = 0;


            return rowStart + col;
        }

        int WrapVertical(int row, int col)
        {
            if (row < 0) row = rows - 1;      
            if (row >= rows) row = 0;         

            int rowStart = row * columns;
            int rowEnd = Mathf.Min(rowStart + columns, buttonCount);

            if (col >= rowEnd - rowStart) col = rowEnd - rowStart - 1; // stay in bounds
            if (col < 0) col = 0;

            return rowStart + col;
        }
    }

    public static void SetTextAlpha(TextMeshProUGUI textMeshProUGUI, float alpha) //other classes to want to use this a bunch so put it here
    {
        Color color = textMeshProUGUI.color;
        color.a = alpha;
        textMeshProUGUI.color = color;
    }

}
