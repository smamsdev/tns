using UnityEngine;
using System.Collections;

public static class Lerper
{
    public static IEnumerator LerpFloat(float startValue, float endValue, float duration, System.Action<float> onUpdate)
    {
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            float t = timeElapsed / duration;
            float lerpedValue = Mathf.Lerp(startValue, endValue, t);

            onUpdate(lerpedValue);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        onUpdate(endValue); // Ensure the end value is reached accurately
    }
}