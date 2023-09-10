using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VegetaDialogueTrigger : MonoBehaviour
{
    [SerializeField] DialogueScript dialogueScript;
    [SerializeField] PlayerMovementScript playerMovementScript;

    private void Update()
    {
        if (playerMovementScript.hit.collider != null && playerMovementScript.hit.collider.tag == "Vegeta" && !dialogueScript.dialogueCompleted)

        { dialogueScript.StartDialogue(); }

    }


}
