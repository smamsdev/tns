using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractionPoint : ColliderInteractableAbstract
{
    public bool disableColliderAfterTrigger;

    public override IEnumerator TriggerFunction()
    {
        animator.Play("Close");
        playerInTrigger = null;

        if (disableColliderAfterTrigger)
        {
            GetComponent<Collider2D>().enabled = false;
        }

        yield return null;
    }

    private void Update()
    {
        if (playerInTrigger && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Triggered());
        }
    }
}
