using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform transformToFollow;
    public float cameraSpeed;
    public float xOffset;
    public float yOffset;

    //dont forget about z -10, it's important!
    private void Awake()
    {
        if (transformToFollow == null)
        { 
            transformToFollow = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }
    }

    private void FixedUpdate()
    {
        var newPos = new Vector3(transformToFollow.position.x + xOffset, transformToFollow.position.y + yOffset, -10f);
        transform.position = Vector3.Slerp(transform.position, newPos, cameraSpeed * Time.deltaTime);
    }
}
