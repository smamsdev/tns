using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]

//can be used as a trigger or collider interactive
public class EntryPoint : ColliderInteractableAbstract
{
    public SceneEntry sceneEntrySO;

    void Reset()
    {
        animator = GetComponent<Animator>();
        playerInTrigger = GetComponent<Collider2D>();
        if (playerInTrigger != true) playerInTrigger.isTrigger = true;
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

    public override IEnumerator TriggerFunction()
    {
        var sceneCombo = sceneEntrySO.sceneCombinationToEnter;
        var defaultFaderAnimator = GameObject.FindGameObjectWithTag("Fader").GetComponent<Animator>();
        defaultFaderAnimator.Play("DissolveToBlack");

        yield return new WaitForSeconds(0.5f);

        yield return LoadScene(sceneCombo.baseScene.name, sceneCombo.additiveScenes[sceneCombo.activeAdditiveIteration].name);
        var playerGO = GameObject.FindGameObjectWithTag("Player");
    }

    public IEnumerator LoadScene(String newBaseSceneName, String newAdditiveSceneName)
    {
        SceneManager.LoadScene(newBaseSceneName, LoadSceneMode.Single);
        SceneManager.LoadScene(newAdditiveSceneName, LoadSceneMode.Additive);
        FieldEvents.sceneName = sceneEntrySO.sceneCombinationToEnter.sceneName;
        yield return null;
    }

    private void Update()
    {
        if (playerInTrigger && Input.GetKeyDown(KeyCode.Space))
        {
            CombatEvents.LockPlayerMovement();
            playerInTrigger.enabled = false;
            FieldEvents.positionOnEntry = sceneEntrySO.positionOnEntry;
            FieldEvents.fromEntryPoint = true;
            StartCoroutine(Triggered());
        }
    }
}
