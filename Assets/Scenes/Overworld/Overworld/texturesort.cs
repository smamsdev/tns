using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardDistanceSort : MonoBehaviour
{
    public Camera cam;
    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (!cam) cam = Camera.main;
    }
    //
    void LateUpdate()
    {
        // Rotate to face camera (billboard)
        Vector3 lookDir = cam.transform.forward;
        lookDir.y = 0f; // flatten
        lookDir.Normalize();
        transform.forward = lookDir;

        // Sort by distance
        float distance = Vector3.Distance(cam.transform.position, transform.position);

        // Push further objects slightly back using render queue
        rend.material.renderQueue = 3000 + Mathf.RoundToInt(distance * 10f);
    }
}

