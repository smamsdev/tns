using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class wheelRotation : MonoBehaviour
{
    public float horizontalSpin;

    public float verticalSpinFrame;

    [SerializeField] GameObject lWheel;
    [SerializeField] GameObject rWheel;
    [SerializeField] Animator lAnim, rAnim;
    public Action rotationMethod;
    public bool isVertical;

    float lastY;
    float lastX;

    void Awake()
    {
        lastY = transform.position.y;
        lastX = transform.position.x;

        if ( !isVertical )
        rotationMethod = HorizontalRotation;

        else
            rotationMethod = VerticalRotation;
    }

    void FixedUpdate()
    {
        rotationMethod.Invoke();
       // Debug.Log(this.gameObject.name);
    }

    void HorizontalRotation()
    {
        float deltaX = transform.position.x - lastX;
        lastX = transform.position.x;

        horizontalSpin = Mathf.Repeat(horizontalSpin - (deltaX * 400), 360f);

        lWheel.transform.rotation = Quaternion.Euler(0, 0, horizontalSpin);
        rWheel.transform.rotation = Quaternion.Euler(0, 0, horizontalSpin);
    }

    void VerticalRotation()
    {
        float deltaY = transform.position.y - lastY;
        lastY = transform.position.y;

        verticalSpinFrame = Mathf.Repeat(verticalSpinFrame + (deltaY * 8), 1f);

        lAnim.speed = 0f;
        rAnim.speed = 0f;

        lAnim.Play("WheelVerticalFwd_Clip", 0, verticalSpinFrame);
        rAnim.Play("WheelVerticalFwd_Clip", 0, verticalSpinFrame);
    }
}
