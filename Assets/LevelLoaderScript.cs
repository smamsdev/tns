using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoaderScript : ToTrigger
{
    public string sceneID;
    public string sceneName;

    public Vector3 entryCoordinates;
    static string PendingPreviousScene;
    public Animator animator;
    bool isCollision;

    public TextMeshProUGUI textMeshProUGUI;

    private void Start()
    {
        textMeshProUGUI.text = sceneName;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")

        {
            isCollision = true;
            animator.SetTrigger("OpenDestination");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")

        {
            isCollision = false;
            animator.SetTrigger("CloseDestination");
        }
    }

    public override IEnumerator DoAction()

    {
        FieldEvents.entryCoordinates = entryCoordinates;
        LoadScene(sceneID);
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

    private void Update()
    {
        if (isCollision)

        {
            if (Input.GetKeyDown(KeyCode.Space))

            {
                StartCoroutine(DoAction());
            }
        }
    }
}
