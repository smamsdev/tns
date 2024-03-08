using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class lerpaNumber : MonoBehaviour
{
    public int number1;
    public int numberToSubtract;
    public int finalNumber;
    public float timer;

    private void Start()
    {
      StartCoroutine(UpdateNumber(number1, numberToSubtract, 0.5f));
    }


    IEnumerator UpdateNumber(int number, int _numberToSubtract, float lerpDuration)

    {
        float elapsedTime = 0f;

        int endValue = number - _numberToSubtract;

        while (elapsedTime < lerpDuration)
        {
            // Calculate the interpolation factor between 0 and 1 based on the elapsed time and duration
            float t = Mathf.Clamp01(elapsedTime / lerpDuration);

            // Lerp between the start and end values
            finalNumber = Mathf.RoundToInt(Mathf.Lerp(number, endValue, t));

            // Increment the elapsed time
            elapsedTime += Time.deltaTime;

            yield return null;
        }

    }
}
