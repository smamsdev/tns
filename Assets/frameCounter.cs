using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class frameCounter : MonoBehaviour
{
    public TextMeshProUGUI frameRateText;

    void Update()
    {
        if (frameRateText != null)
        {
            float fps = 1.0f / Time.deltaTime;
            frameRateText.text = $"FPS: {Mathf.Ceil(fps)}";
        }
    }
}
