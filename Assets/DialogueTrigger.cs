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


    //auto load the first Dialogue Child
    private void Start()
    {
        dialogueToPlay = this.transform.GetChild(0).gameObject.GetComponent<DialogueContainer>();
    }

    void PlayerRayCastHit(RaycastHit2D raycastHit2D)

    {
        if (raycastHit2D.collider.gameObject == this.transform.parent.gameObject && !dialogueToPlay.dialogueLaunched && !FieldEvents.isCooldown() && !FieldEvents.isDialogueActive)    
        {
            StartCoroutine(FieldEvents.CoolDown(0.3f));
            dialogueToPlay.OpenDialogue();
        }
    }

    public void ReloadDialogue(GameObject gameObject)

    {
        if (gameObject == dialogueToPlay.dialogue[0].dialogueGameObject)
     
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

