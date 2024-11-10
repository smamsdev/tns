using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleGame : MonoBehaviour
{
    public enum RotationMode { Increasing, Decreasing }
    private RotationMode rotationMode = RotationMode.Increasing;

    float angle;
    float angleMax = 9f;
    float angleMin = -11f;


    [SerializeField] Animator playerAnimator;
    [SerializeField] GameObject rifle;

    void FixedUpdate()
    {
        if (Random.value < 0.02f)
        {
            if (rotationMode == RotationMode.Increasing)
            {
                rotationMode = RotationMode.Decreasing;
            }
            else
            {
                rotationMode = RotationMode.Increasing;
            }
        }

        if (rotationMode == RotationMode.Increasing)
        {
            IncreaseAngle();
        }
        else if (rotationMode == RotationMode.Decreasing)
        {
            DecreaseAngle();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var animator = GetComponent<Animator>();

            animator.SetTrigger("Trigger1");

            playerAnimator.SetTrigger("Trigger1");

            if (angle < 1.5f && angle > -1.5f)
            {
                Debug.Log("hit");
            }

            else
            {
                Debug.Log("cringe");
            }
        }
    }

    void IncreaseAngle()
    {
        angle += .3f;
        rifle.transform.rotation = Quaternion.Euler(0, 0, angle);

        if (angle >= angleMax)
        {
            rotationMode = RotationMode.Decreasing;
        }
    }

    void DecreaseAngle()
    {
        angle -=.3f;
        rifle.transform.rotation = Quaternion.Euler(0, 0, angle);

        if (angle <= angleMin)
        {
            rotationMode = RotationMode.Increasing;
        }
    }
}