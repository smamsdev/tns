using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clock : MonoBehaviour
{

    [SerializeField] GameObject hours;
    [SerializeField] GameObject minutes;
    [SerializeField] GameObject seconds;

    void Start()
    {
        InvokeRepeating("Clock", 0, 1);
    }

    void Clock()

    {
        DateTime currentTime = System.DateTime.Now;

        Debug.Log(MinuteHangAngle(currentTime));

        hours.transform.rotation = Quaternion.Euler(0, 0, -(currentTime.Hour * 30));
        minutes.transform.rotation = Quaternion.Euler(0, 0, -MinuteHangAngle(currentTime));
        seconds.transform.rotation = Quaternion.Euler(0, 0, -SecondHangAngle(currentTime));

    }

    int MinuteHangAngle(DateTime dateTime)

    {
        int result = dateTime.Minute * 6;

        return result;
    }

    int SecondHangAngle(DateTime dateTime)

    {
        int result = dateTime.Second * 6;

        return result;
    }
}
