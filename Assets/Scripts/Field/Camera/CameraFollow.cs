using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform transformToFollow;
    public float smoothTime = 0.15f; // how quickly camera catches up
    public float xOffset;
    public float yOffset;

    private Vector3 velocity = Vector3.zero;

    private void Awake()
    {
        if (transformToFollow == null)
        {
            transformToFollow = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    private void LateUpdate()
    {
        Vector3 targetPos = new Vector3(transformToFollow.position.x + xOffset, transformToFollow.position.y + yOffset, -10f);
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
    }
}
