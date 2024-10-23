using UnityEngine;

public class SceneSetup : MonoBehaviour
{
    public Transform transformToFollow;
    public GameObject mainCamera;
    public bool useEntryCoordinates;
    public Vector2 forceLook;
    public Vector3 forcedEntryCoordinates;
    public bool forceEntryCoorinates;

    private void OnEnable()
    {
        var playerGO = GameObject.Find("Player");

        if (useEntryCoordinates)
        {
            if (forceEntryCoorinates)
            {
                FieldEvents.entryCoordinates = forcedEntryCoordinates;
            }

            transformToFollow.transform.position = FieldEvents.entryCoordinates;
        }

        if (forceLook != Vector2.zero) 
        {

            var playerMovementScript = playerGO.GetComponent<PlayerMovementScript>();
            playerMovementScript.lookDirection = forceLook;
        }

        mainCamera.transform.position = new Vector3(transformToFollow.position.x, transformToFollow.transform.position.y, transformToFollow.transform.position.z - 10);
    }
}