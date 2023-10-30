using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueContainer dialogueToPlay;

    private void OnEnable()
    {
        FieldEvents.PlayerRayCastHit += PlayerRayCastHit;
    }

    private void OnDisable()
    {
        FieldEvents.PlayerRayCastHit -= PlayerRayCastHit;
    }

    private void Start()
    {
        dialogueToPlay = this.transform.GetChild(0).gameObject.GetComponent<DialogueContainer>();
    }

    void PlayerRayCastHit(RaycastHit2D raycastHit2D)

    {
        if (raycastHit2D.collider.tag == this.transform.parent.gameObject.tag && !dialogueToPlay.dialogueLaunched && !FieldEvents.isCooldown())    
        {
            StartCoroutine(FieldEvents.CoolDown(0.3f));
            dialogueToPlay.StartDialogue(); 
        }
    }

}
