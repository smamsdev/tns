using UnityEngine;

public class SceneSetup : MonoBehaviour
{
    public Transform transformToFollow;
    public GameObject mainCamera;
    public bool rememberEntryCoordinates;
    public bool forceLookRight;

    private void Awake()

    {
        transformToFollow = mainCamera.GetComponent<CameraFollow>().transformToFollow.transform;

        if (rememberEntryCoordinates)
        {
            transformToFollow.transform.position = FieldEvents.entryCoordinates;
        }

        if (forceLookRight) 
        {
            FieldEvents.lookDirection = Vector2.right;
        }

        mainCamera.transform.position = new Vector3(transformToFollow.position.x, transformToFollow.transform.position.y, transformToFollow.transform.position.z - 10);
    }
}