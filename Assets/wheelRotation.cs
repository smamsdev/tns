using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class wheelRotation : MonoBehaviour
{
    float x = 0;
    float angle;

    [SerializeField] GameObject lWheel;
    [SerializeField] GameObject rWheel;

    void FixedUpdate()
    {
        angle = (x - this.transform.position.x)*80;

        lWheel.transform.rotation = Quaternion.Euler(0, 0, angle);
        rWheel.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
