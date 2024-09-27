using UnityEngine;

public class SceneSetup : MonoBehaviour
{
    public Transform transformToFollow;
    public GameObject mainCamera;
    public bool useEntryCoordinates;
    public bool forceLookRight;
    public Vector3 forcedEntryCoordinates;
    public bool forceEntryCoorinates;

    private void Awake()
    {
        transformToFollow = mainCamera.GetComponent<CameraFollow>().transformToFollow.transform;

        if (useEntryCoordinates)
        {
            if (forceEntryCoorinates)
            {
                FieldEvents.entryCoordinates = forcedEntryCoordinates;
            }

            transformToFollow.transform.position = FieldEvents.entryCoordinates;
        }

        if (forceLookRight) 
        {
            FieldEvents.lookDirection = Vector2.right;
        }

        mainCamera.transform.position = new Vector3(transformToFollow.position.x, transformToFollow.transform.position.y, transformToFollow.transform.position.z - 10);
    }
}