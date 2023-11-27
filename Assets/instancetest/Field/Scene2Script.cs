using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene2Script : MonoBehaviour
{
    public DialogueContainer[] Dialogue;
    public DialogueContainer[] DialogueTriggerReload;
    public Shift[] shift;

    public GameObject[] Battles;
    public bool isFreshScene;
    public bool resetStartPositionToDefault;


    private void Awake()
    {
    //   if (resetStartPositionToDefault)
    //   {
    //       FieldEvents.playerLastKnownPos = GameObject.Find("Player").transform.position;
    //       resetStartPositionToDefault = false;
    //
    //   }
    //
    //   if (!FieldEvents.freshScene)
    //   {
    //       var playerMovementScript = GameObject.Find("Player").GetComponent<PlayerMovementScript>();
    //
    //       playerMovementScript.transform.position = new Vector2(FieldEvents.playerLastKnownPos.x, FieldEvents.playerLastKnownPos.y - 0.02f);
    //   }


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

         if (ID == "Vegeta1stDialogue" && isDialogueComplete)
        
         {
             Battles[0].GetComponentInChildren<CombatManagerV3>().SetBattleSetupBattle();
         }
        
        // if (ID == "DefeatedDialogue" && isDialogueComplete)
        //
        // {
        //     DialogueReload(DialogueTriggerReload[0]);
        // }
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

