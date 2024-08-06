using UnityEngine;

public class SceneSetup : MonoBehaviour
{
    public GameObject player;
    public GameObject mainCamera;
    public bool rememberEntryCoordinates;

    private void Start()
    {
        if (rememberEntryCoordinates)
        {
            player.transform.position = FieldEvents.entryCoordinates;
        }

        mainCamera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - 10);
    }
}