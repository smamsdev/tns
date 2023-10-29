using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneScript : MonoBehaviour
{
    public DialogueScript[] Dialogue;
    public Act[] Act;
    public GameObject[] Battles;

    private void OnEnable()
    {
        FieldEvents.DialogueEvent += DialogueEvent;
        FieldEvents.HasBeenDefeated += HasBeenDefeated;
        FieldEvents.ActorActionHasStarted += ActorActionHasStarted;
        FieldEvents.ActorActionHasCompleted += ActorActionHasCompleted;
    }

    private void OnDisable()
    {
        FieldEvents.DialogueEvent -= DialogueEvent;
        FieldEvents.HasBeenDefeated -= HasBeenDefeated;
        FieldEvents.ActorActionHasStarted -= ActorActionHasStarted;
        FieldEvents.ActorActionHasCompleted -= ActorActionHasCompleted;
    }

    private void Start()
    {
       Act[0].StartAct();
    }

    void HasBeenDefeated(GameObject enemyGameObject)

    {
        StartCoroutine(HasBeenDefeatedCoRoutine(enemyGameObject));
    }

    IEnumerator HasBeenDefeatedCoRoutine(GameObject enemyGameObject)

    {

        if (enemyGameObject == Battles[0].GetComponent<CombatManagerV3>().enemyGameObject)

        {
            Act[2].StartAct();
            yield return new WaitForSeconds(3);
            Dialogue[0].StartDialogue();
        }
    }


    public void DialogueEvent(string ID, int dialogueElement, bool isDialogueComplete)

    {
        if (ID == "2ndDialogue" && isDialogueComplete) 
        
        {
            var battle = Battles[0].GetComponentInChildren<CombatManagerV3>();
            battle.SetBattleSetupBattle(); 
        }
    }

    void ActorActionHasStarted(string ID, bool isStarted)
    {
       
    }


    void ActorActionHasCompleted(string ID, bool isCompleted)
    {
        if (ID == "PlayerEnter" && isCompleted) 
        
        { 
            Act[1].StartAct();
        }
    }
}

