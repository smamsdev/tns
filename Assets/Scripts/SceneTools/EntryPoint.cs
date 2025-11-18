using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]

public class EntryPoint : ToTrigger
{
    public SceneEntry sceneEntrySO;
    public Vector3 entryCoordinates;
    public Animator animator;
    private Collider2D playerInTrigger;
    bool isCollision;
    public TextMeshProUGUI signPostTMP;

    public SceneCombination sceneCombination;

    void Reset()
    {
        animator = GetComponent<Animator>();
        playerInTrigger = GetComponent<Collider2D>();
        if (playerInTrigger != true) playerInTrigger.isTrigger = true;
    }

    private void Start()
    {
        signPostTMP.text = sceneEntrySO.sceneNameToDisplay;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInTrigger = collision;
            animator.Play("Open");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInTrigger = null;
            animator.Play("Close");
        }
    }

    public override IEnumerator DoAction()
    {
        var sceneCombo = sceneEntrySO.sceneCombinationToEnter;

        yield return LoadScene(sceneCombo.baseScene.name, sceneCombo.additiveScenes[sceneCombo.activeAdditiveIteration].name);
        FieldEvents.HasCompleted.Invoke(this.gameObject);

    }

    public IEnumerator LoadScene(String newBaseSceneName, String newAdditiveSceneName)
    {
        FadeOut();
        SceneManager.LoadScene(sceneCombination.baseScene.name, LoadSceneMode.Single);
        SceneManager.LoadScene(newAdditiveSceneName, LoadSceneMode.Additive);
        yield return null;
    }

    private void FadeOut()
    {
        FieldEvents.SceneChanging();
    }

    private void Update()
    {
        if (playerInTrigger && Input.GetKeyDown(KeyCode.Space))
        {
            //StartCoroutine(DoAction());
            Debug.Log("test");
        }
    }
}
