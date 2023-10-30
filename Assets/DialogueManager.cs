using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{

    Dialogue[] dialogue;
    [SerializeField] GameObject dialogueBoxPrefab;

    DialogueBox dialogueBox;
    public int dialogueElement;
    string dialogueGameObjectID;

    public bool dialogueIsActive;
    public int dialogueLength;
    bool isCoolDown;


    public void OpenDialogue(Dialogue[] _dialogue)

    {
        dialogue = _dialogue;

        dialogueElement = 0;
        dialogueLength = dialogue.Length;

        SpawnDialogueBox();

        dialogueIsActive = true;
        CombatEvents.LockPlayerMovement?.Invoke();
        dialogueGameObjectID = dialogue[0].dialogueGameObjectID;

        dialogueBox.DisplayMessage(dialogue[dialogueElement]);
        FieldEvents.DialogueEvent?.Invoke(dialogue[dialogueElement].dialogueGameObjectID, dialogueElement, false);
        StartCoroutine(FieldEvents.CoolDown(0.3f));
    }

    public void SpawnDialogueBox()

    {
        dialogue[dialogueElement].dialogueBoxGameObject = Instantiate(dialogueBoxPrefab, transform);

        dialogue[dialogueElement].dialogueBoxGameObject.name = dialogueGameObjectID + "Dialogue#" + dialogueElement;
        dialogueBox = dialogue[dialogueElement].dialogueBoxGameObject.GetComponent<DialogueBox>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && dialogueIsActive == true &! FieldEvents.isCooldown())

        {
            StartCoroutine(NextMessage());
            StartCoroutine(FieldEvents.CoolDown(0.3f));
        }
    }

    public IEnumerator NextMessage()

    {

        if (dialogueElement == (dialogue.Length-1))
        {
            dialogueElement++;
            StartCoroutine(FieldEvents.CoolDown(0.3f));
            CombatEvents.UnlockPlayerMovement?.Invoke();
            dialogueIsActive = false;
            dialogueBox.animator.SetTrigger("CloseDialogue");

            yield return new WaitForSeconds(0.3f);

            FieldEvents.DialogueEvent?.Invoke(dialogueGameObjectID, dialogueElement, true);

            dialogueElement = -1; //just set it to something negative so we know it's cooked

        }

        if (dialogueElement < dialogue.Length && dialogueElement != -1)
         {
            dialogueBox.animator.SetTrigger("CloseDialogue");

            yield return new WaitForSeconds(0.3f);

            dialogueElement++;
            SpawnDialogueBox();

            FieldEvents.DialogueEvent?.Invoke(dialogueGameObjectID, dialogueElement, false);
            dialogueBox.DisplayMessage(dialogue[dialogueElement]);
        }
    }

 

}

