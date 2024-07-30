using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoaderScript : ToTrigger
{
    public string sceneName;
    public Vector2 entryCoordinates;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Player")

        {
            StartCoroutine(DoAction());
        }
    }

    public override IEnumerator DoAction()

    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        FieldEvents.entryCoordinates = entryCoordinates;

        Debug.Log(this.gameObject);

        yield return null;
    }

}
