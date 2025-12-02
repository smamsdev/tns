using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractionPoint : ColliderInteractableAbstract
{
    public bool disableColliderAfterAction;
    public ToTrigger optionalTriggerOnAction;

    public override IEnumerator TriggerFunction()
    {
        animator.Play("Close");
        playerInTrigger = null;

        if (disableColliderAfterAction)
        {
            GetComponent<Collider2D>().enabled = false;
        }

        yield return null;
    }

    private void Update()
    {
        if (playerInTrigger && Input.GetKeyDown(KeyCode.Space))
        {
            if (optionalTriggerOnAction != null)
            {
                StartCoroutine(TriggerFunction());
                StartCoroutine(optionalTriggerOnAction.Triggered());
                return;
            }

            StartCoroutine(this.Triggered());
        }
    }
}
