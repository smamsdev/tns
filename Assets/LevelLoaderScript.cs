using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoaderScript : MonoBehaviour
{
    public int sceneNumber;
    public bool rememberPosition;
    public bool isFreshScene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isFreshScene)

        { FieldEvents.freshScene = true; }

        if (!isFreshScene)

        { FieldEvents.freshScene = false; }



        if (collision.tag == "Player")

        {
            if (rememberPosition) 
            { 
            var playerMovementScript = GameObject.Find("player").GetComponent<PlayerMovementScript>();
            FieldEvents.playerLastKnownPos = playerMovementScript.transform.position;
            }

            SceneManager.LoadScene(sceneNumber, LoadSceneMode.Single);
        }
    }


}
