using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{

    public DialogueContainer dialogueToPlay;

    public DialogueReload[] dialogueReloads;

    private void OnEnable()
    {
        FieldEvents.PlayerRayCastHit += PlayerRayCastHit;
        FieldEvents.HasCompleted += ReloadDialogue;
    }

    private void OnDisable()
    {
        FieldEvents.PlayerRayCastHit -= PlayerRayCastHit;
        FieldEvents.HasCompleted -= ReloadDialogue;
    }

    void PlayerRayCastHit(RaycastHit2D raycastHit2D)
    {
        if (raycastHit2D.collider.gameObject == this.gameObject && !dialogueToPlay.dialogueLaunched && !FieldEvents.isCooldown() && !FieldEvents.isDialogueActive)    
        {
            StartCoroutine(FieldEvents.CoolDown(0.3f));
            StartCoroutine(dialogueToPlay.DoAction());
        }
    }

    public void ReloadDialogue(GameObject gameObject)

    {
            if (dialogueToPlay.dialogue[0].dialogueGameObject != null && gameObject == dialogueToPlay.dialogue[0].dialogueGameObject)
     
           if (dialogueToPlay.isLoopedDialogue)
           {
               dialogueToPlay.dialogueLaunched = false;
           }
     
       if (dialogueReloads.Length > 0)
    
              for (int i = 0; i < dialogueReloads.Length; i++)
              {
                  if (gameObject == dialogueReloads[i].GOToTrigger)
    
                  {
                      dialogueToPlay = dialogueReloads[i].dialogueToReplace;
                  }
              }
    }

}

[System.Serializable]

public class DialogueReload

{
    public GameObject GOToTrigger;
    public DialogueContainer dialogueToReplace;
}

