using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneScript : MonoBehaviour
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

    private void Start()
    {
         Act[0].StartAct();

      // Battles[0].GetComponentInChildren<CombatManagerV3>().SetBattleSetupBattle();

    }

    void HasBeenDefeated(GameObject enemyGameObject)

    {
        StartCoroutine(HasBeenDefeatedCoRoutine(enemyGameObject));
    }

    IEnumerator HasBeenDefeatedCoRoutine(GameObject enemyGameObject)

    {
        if (enemyGameObject == Battles[0].GetComponent<CombatManagerV3>().enemyGameObject)

        {
            Act[1].StartAct();
            yield return new WaitForSeconds(3);

            StartCoroutine(ChainNewDialogue(Dialogue[0]));

        }
    }


    public void DialogueEvent(string ID, int dialogueElement, bool isDialogueComplete)

    {
          if (ID == "1stDialogue" && isDialogueComplete)

        {

            StartCoroutine(ChainNewDialogue(Dialogue[2]));
        }

        if (ID == "2ndDialogue" && isDialogueComplete) 
        
        {
            DialogueReload(DialogueTriggerReload[1]);
            DialogueReload(DialogueTriggerReload[2]);
        }

        if (ID == "DefeatedDialogue" && isDialogueComplete)

        {
            DialogueReload(DialogueTriggerReload[0]);
        }

        if (ID == "Larmy1stDialogue" && isDialogueComplete)

        {
            FieldEvents.objectFetched = false;
        }
    }

    void ActorActionHasStarted(string ID, bool isStarted)
    {
       
    }


    void ActorActionHasCompleted(string ID, bool isCompleted)
    {
      //  if (ID == "PlayerEnter"  && isCompleted) 
      //  
      //  { Dialogue[1].DoAction(); }
    }

    void DialogueReload (DialogueContainer dialogueToReload) 
    
    {
        var actorDialogueTrigger = dialogueToReload.transform.parent.gameObject.GetComponent<DialogueTrigger>();
        actorDialogueTrigger.dialogueToPlay = dialogueToReload;
    }

    IEnumerator ChainNewDialogue(DialogueContainer dialogue)

    {
        var actorDialogue = dialogue.transform.parent.gameObject.GetComponent<DialogueTrigger>();
        actorDialogue.dialogueToPlay = dialogue;

        yield return new WaitForSeconds(0.01f); //i forget why but it breaks if you dont put this
//actorDialogue.dialogueToPlay.DoAction();

    }
}

