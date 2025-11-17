using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : ToTrigger
{
    public String newBaseSceneName;
    public String newAdditiveSceneName;

    public string destinationNameToDisplay;
    public Vector3 entryCoordinates;
    public Animator animator;
    bool isCollision;
    public TextMeshProUGUI textMeshProUGUI;

    public SceneCombination sceneCombination;
    public int additiveSceneIteration;

    private void Start()
    {
        if (textMeshProUGUI != null)
        {
            textMeshProUGUI.text = destinationNameToDisplay;
        }

        else 
        { 
            Debug.Log(this.name + " destination is blank");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isCollision = true;
            animator.Play("Open");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isCollision = false;
            animator.Play("Close");
        }
    }

    public override IEnumerator DoAction()
    {
        FieldEvents.entryCoordinates = entryCoordinates;
        yield return new WaitForSeconds(1f);
        LoadScene(newBaseSceneName, newAdditiveSceneName);
        FieldEvents.HasCompleted.Invoke(this.gameObject);
        yield return null;
    }

    public void LoadScene(String newBaseSceneName, String newAdditiveSceneName)
    {
        FadeOut();
        SceneManager.LoadScene(sceneCombination.baseScene.name, LoadSceneMode.Single);
        SceneManager.LoadScene(newAdditiveSceneName, LoadSceneMode.Additive);
    }

    void LoadTest()
    {
        FadeOut();
        SceneManager.LoadScene(newBaseSceneName, LoadSceneMode.Single);
        SceneManager.LoadScene(newAdditiveSceneName, LoadSceneMode.Additive);
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
