using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaSceneScript : MonoBehaviour
{
    public DialogueContainer[] Dialogue;
    public DialogueContainer[] DialogueTriggerReload;
    public Act[] Act;
    public GameObject[] Battles;

    private void OnEnable()
    {
        FieldEvents.HasBeenDefeated += HasBeenDefeated;
        FieldEvents.ActorActionHasStarted += ActorActionHasStarted;
        FieldEvents.ActorActionHasCompleted += ActorActionHasCompleted;
    }

    private void OnDisable()
    {
        FieldEvents.HasBeenDefeated -= HasBeenDefeated;
        FieldEvents.ActorActionHasStarted -= ActorActionHasStarted;
        FieldEvents.ActorActionHasCompleted -= ActorActionHasCompleted;
    }



    void HasBeenDefeated(GameObject enemyGameObject)

    {
        // StartCoroutine(HasBeenDefeatedCoRoutine(enemyGameObject));
    }

    IEnumerator HasBeenDefeatedCoRoutine(GameObject enemyGameObject)

    {
        // if (enemyGameObject == Battles[0].GetComponent<CombatManagerV3>().enemyGameObject)
        //
        // {
        //     Act[1].StartAct();
        yield return new WaitForSeconds(3);
        //
        //     StartCoroutine(ChainNewDialogue(Dialogue[0]));
        //
        // }
    }


    public void DialogueEvent(string ID, int dialogueElement, bool isDialogueComplete)

    {

        if (ID == "Larmy1stDialogue" && isDialogueComplete)
        
        {
            FieldEvents.objectFetched = true;
            Debug.Log("isthison");
        }

    }

    void ActorActionHasStarted(string ID, bool isStarted)
    {

    }


    void ActorActionHasCompleted(string ID, bool isCompleted)
    {
        // if (ID == "PlayerEnter" && isCompleted)
        // 
        // { Dialogue[1].StartDialogue(); }
    }

    void DialogueReload(DialogueContainer dialogueToReload)

    {
        dialogueToReload.transform.parent.gameObject.GetComponent<DialogueTrigger>().dialogueToPlay = dialogueToReload;
    }

    IEnumerator ChainNewDialogue(DialogueContainer dialogue)

    {
        // var actorDialogue = dialogue.transform.parent.gameObject.GetComponent<DialogueTrigger>();
        // actorDialogue.dialogueToPlay = dialogue;
        // 
        yield return new WaitForSeconds(0.01f); //i forget why but it breaks if you dont put this
                                                // actorDialogue.dialogueToPlay.StartDialogue();

    }
}

