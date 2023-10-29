using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{

    DialogueScript dialogueToPlay;
    [SerializeField] DialogueScript firstDialogueScript;
    [SerializeField] DialogueScript secondDialogueScript;
    bool isChainDialogueMode;

    [SerializeField] PlayerMovementScript playerMovementScript;

    private void OnEnable()
    {
        FieldEvents.DialogueEvent += DialogueEvent;
    }

    private void OnDisable()
    {
        FieldEvents.DialogueEvent -= DialogueEvent;
    }

    private void Start()
    {
        dialogueToPlay = firstDialogueScript;
    }

    private void Update()
    {

       if (playerMovementScript.hit.collider != null && playerMovementScript.hit.collider.tag == "Vegeta" && !dialogueToPlay.dialogueLaunched && !isChainDialogueMode)
    
       { dialogueToPlay.StartDialogue(); }
    }

    public void DialogueEvent(string ID, int dialogueElement, bool isDialogueComplete)

    {

        if (ID == "1stDialogue" && isDialogueComplete)

        {
            isChainDialogueMode = true;
            dialogueToPlay = secondDialogueScript;
            StartCoroutine(PlayDialogue(secondDialogueScript));
        }

        else { isChainDialogueMode = false; }
    }

    IEnumerator PlayDialogue(DialogueScript _dialogueToPlay)

    {
        yield return new WaitForSeconds(0.01f);
        CombatEvents.LockPlayerMovement?.Invoke();
        yield return new WaitForSeconds(0.5f);
        dialogueToPlay = _dialogueToPlay;
        dialogueToPlay.StartDialogue();
    }

}
