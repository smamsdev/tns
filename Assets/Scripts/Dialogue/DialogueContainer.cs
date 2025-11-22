using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueContainer : ToTrigger
{
    [HideInInspector] public DialogueManager dialogueManager;

    public Dialogue[] dialogue;
    public bool dialogueLaunched;
    public bool isLoopedDialogue;

    private void Start()
    {
        for (int i = 0; i < dialogue.Length; i++)
        {
            dialogue[i].dialogueGameObject = gameObject; //wtf is this
        }

        dialogueManager = GameObject.FindGameObjectWithTag("DialogManager").GetComponent<DialogueManager>();
    }

   public override IEnumerator TriggerFunction()

   {
        CombatEvents.LockPlayerMovement?.Invoke();

        FieldEvents.isDialogueActive = true;

        yield return new WaitForSeconds(0.3f);

        dialogueManager.OpenDialogue(dialogue);
       dialogueLaunched = true;
   }

    protected override void TriggerComplete()
    {
        //
    }

}

