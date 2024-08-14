using UnityEngine;

public class SceneSetup : MonoBehaviour
{
    public GameObject player;
    public GameObject mainCamera;
    public bool rememberEntryCoordinates;
    public bool forceLookRight;

    private void Awake()
    {
        if (rememberEntryCoordinates)
        {
            player.transform.position = FieldEvents.entryCoordinates;
        }

        if (forceLookRight) 
        {
            FieldEvents.lookDirection = Vector2.right;
        }

        mainCamera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - 10);
    }
}