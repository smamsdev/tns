using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoaderScript : ToTrigger
{
    public string sceneName;
    public Vector3 entryCoordinates;
    static string PendingPreviousScene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")

        {
            StartCoroutine(DoAction());
        }
    }

    public override IEnumerator DoAction()

    {
        FieldEvents.entryCoordinates = entryCoordinates;
        LoadScene(sceneName);
        yield return null;
    }

    public static void LoadScene(string SceneNameToLoad)
    {
        PendingPreviousScene = SceneManager.GetActiveScene().name;
        SceneManager.sceneLoaded += ActivatorAndUnloader;
        SceneManager.LoadScene(SceneNameToLoad, LoadSceneMode.Additive);
    }

    static void ActivatorAndUnloader(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= ActivatorAndUnloader;
        SceneManager.SetActiveScene(scene);
        SceneManager.UnloadSceneAsync(PendingPreviousScene);
    }
}
