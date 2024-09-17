using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertsDialogueContainer : DialogueContainer
{
    DialogueManager dialogueManager;

    private void Awake()
    {
        dialogueManager = GameObject.FindObjectOfType<DialogueManager>();
    }

    public override IEnumerator DoAction()

    {
        CombatEvents.LockPlayerMovement?.Invoke();

        FieldEvents.isDialogueActive = true;

        yield return new WaitForSeconds(0.3f);

        dialogueManager.OpenDialogue(dialogue);
        dialogueLaunched = true;
    }
}

