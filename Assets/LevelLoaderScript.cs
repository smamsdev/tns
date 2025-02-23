using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class LevelLoaderScript : ToTrigger
{
    public string sceneID;
    public string sceneName;
    public Vector3 entryCoordinates;
    private string pendingPreviousScene;
    public Animator animator;
    bool isCollision;
    public TextMeshProUGUI textMeshProUGUI;

    private void Start()
    {
        if (textMeshProUGUI != null)
        { 
            textMeshProUGUI.text = sceneName;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isCollision = true;
            animator.SetTrigger("OpenDestination");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isCollision = false;
            animator.SetTrigger("CloseDestination");
        }
    }

    public override IEnumerator DoAction()
    {
        FieldEvents.entryCoordinates = entryCoordinates;
        FieldEvents.SceneChanging.Invoke();
        yield return new WaitForSeconds(1f);
        LoadScene(sceneID);
        FieldEvents.HasCompleted.Invoke(this.gameObject);
        yield return null;
    }

    public void LoadScene(string SceneNameToLoad)
    {
        FadeOut();
        pendingPreviousScene = SceneManager.GetActiveScene().name;
        SceneManager.sceneLoaded += ActivatorAndUnloader;
        SceneManager.LoadScene(SceneNameToLoad, LoadSceneMode.Additive);
    }

    private void ActivatorAndUnloader(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= ActivatorAndUnloader;
        SceneManager.SetActiveScene(scene);

        SceneManager.UnloadSceneAsync(pendingPreviousScene);

    }

    private void FadeOut()
    {
        FieldEvents.SceneChanging();
    }

    private void Update()
    {
        if (isCollision && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(DoAction());
        }
    }
}