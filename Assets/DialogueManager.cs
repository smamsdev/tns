using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    Dialogue[] dialogue;
    [SerializeField] GameObject dialogueBoxPrefab;

    DialogueBox dialogueBox;
    public int dialogueElement;

    public bool dialogueIsActive;

    public void OpenDialogue(Dialogue[] _dialogue)

    {
        dialogue = _dialogue;

        dialogueElement = 0;

        SpawnDialogueBox();

        dialogueIsActive = true;
        CombatEvents.LockPlayerMovement?.Invoke();
        dialogueElement = 0;
        dialogueBox.DisplayMessage(dialogue[dialogueElement]);
    }

    public void SpawnDialogueBox()

    {
        dialogue[dialogueElement].dialogueBoxGameObject = Instantiate(dialogueBoxPrefab, transform);
        dialogue[dialogueElement].dialogueBoxGameObject.name = "dialogue" + dialogueElement;
        dialogueBox = dialogue[dialogueElement].dialogueBoxGameObject.GetComponent<DialogueBox>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && dialogueIsActive == true)

        {
            StartCoroutine(NextMessage());
        }
    }

    public IEnumerator NextMessage()

    {
        if (dialogueElement == (dialogue.Length-1))
        {
            dialogueBox.animator.SetTrigger("CloseDialogue");
            yield return new WaitForSeconds(0.3f);

            DestroyDialogueBox();
            dialogueElement++;

            Debug.Log("Convo ended");
            dialogueIsActive = false;

            CombatEvents.UnlockPlayerMovement?.Invoke();
            yield break;
        }



        if (dialogueElement < dialogue.Length)
         {
            dialogueBox.animator.SetTrigger("CloseDialogue");
            yield return new WaitForSeconds(0.3f);

            DestroyDialogueBox();
            dialogueElement++;

            SpawnDialogueBox();
            dialogueBox.DisplayMessage(dialogue[dialogueElement]);
        }


    }

    public void DestroyDialogueBox()

    {
        Destroy(dialogue[dialogueElement].dialogueBoxGameObject);
    }

}

