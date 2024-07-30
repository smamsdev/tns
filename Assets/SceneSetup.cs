using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSetup : MonoBehaviour
{
    public GameObject player;
    public GameObject mainCamera;

    void Start()
    {
        player.transform.position = FieldEvents.entryCoordinates;
        mainCamera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, (player.transform.position.z - 10));
    }
}
