using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class frameLimiter : MonoBehaviour
{

    public int targetFrameRate;

    private void Start()
    {
        Application.targetFrameRate = targetFrameRate;
    }
}
