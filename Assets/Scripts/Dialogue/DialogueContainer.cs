using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DialogueContainer : ToTrigger
{
    DialogueManager dialogueManager;

    public Dialogue[] dialogue;
    public bool dialogueLaunched;
    public bool isLoopedDialogue;

    private void Awake()
    {
        for (int i = 0; i < dialogue.Length; i++)
        {
            dialogue[i].dialogueGameObject = gameObject; //wtf is this
        }

        dialogueManager = GameObject.FindObjectOfType<DialogueManager>();
    }

    public void OpenDialogue()

    {
        FieldEvents.isDialogueActive = true;
        dialogueManager.OpenDialogue(dialogue);
        dialogueLaunched = true;
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

[System.Serializable]

public class Dialogue

{
    public GameObject actorGameObject;
    [HideInInspector] public string actorName;
    [TextArea(2, 5)] public string dialoguetext;
    public Vector2 optionalDialogueFinalPosition;
    [HideInInspector] public GameObject dialogueGameObject;
}


//if you leave gameobject null it will show no Actor name and default to GO localposition, useful for items, buildings
//if you leave final position blank it will default to parent GO pos + a little bit, to appear above its pivot point

