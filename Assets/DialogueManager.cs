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
        StartCoroutine(CoolDown());
    }

    public void SpawnDialogueBox()

    {
        dialogue[dialogueElement].dialogueBoxGameObject = Instantiate(dialogueBoxPrefab, transform);

        dialogue[dialogueElement].dialogueBoxGameObject.name = dialogueGameObjectID + "Dialogue#" + dialogueElement;
        dialogueBox = dialogue[dialogueElement].dialogueBoxGameObject.GetComponent<DialogueBox>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && dialogueIsActive == true &! isCoolDown)

        {
            StartCoroutine(NextMessage());
            StartCoroutine(CoolDown());
        }
    }

    IEnumerator CoolDown()
    {
        isCoolDown = true;
        yield return new WaitForSeconds(0.3f);
        isCoolDown = false;
    }

    public IEnumerator NextMessage()

    {

        if (dialogueElement == (dialogue.Length-1))
        {
            StartCoroutine(CoolDown());
            CombatEvents.UnlockPlayerMovement?.Invoke();
            dialogueIsActive = false;
            dialogueBox.animator.SetTrigger("CloseDialogue");

            FieldEvents.DialogueEvent?.Invoke(dialogueGameObjectID, dialogueElement+1, true);
            yield return new WaitForSeconds(0.3f);


            DestroyDialogueBox();

            dialogueElement++;
        }

        if (dialogueElement < dialogue.Length)
         {
            dialogueBox.animator.SetTrigger("CloseDialogue");

            yield return new WaitForSeconds(0.3f);

            DestroyDialogueBox();
            dialogueElement++;

            SpawnDialogueBox();

            FieldEvents.DialogueEvent?.Invoke(dialogueGameObjectID, dialogueElement, false);
            dialogueBox.DisplayMessage(dialogue[dialogueElement]);
        }


    }

    public void DestroyDialogueBox()

    {
        Destroy(dialogue[dialogueElement].dialogueBoxGameObject);
    }

}

