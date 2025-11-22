using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{

    Dialogue[] dialogue;

    public string dialogueCurrentlyInPlay;

    [SerializeField] GameObject dialogueBoxPrefab;
    [SerializeField] GameObject alertBoxPrefab;

    public GameObject prefabToUse;

    DialogueBox dialogueBox;

    GameObject dialogueBoxGameObject;

    public int dialogueElement;

    public bool dialogueIsActive;
    public int dialogueLength;

    private void Start()
    {
        FieldEvents.isDialogueActive = false;
    }

    public void OpenDialogue(Dialogue[] _dialogue)

    {
        prefabToUse = dialogueBoxPrefab;
        dialogue = _dialogue;
        dialogueElement = 0;
        dialogueLength = dialogue.Length;
        SpawnDialogueBox();
        dialogueIsActive = true;
        dialogueBox.DisplayMessage(dialogue[dialogueElement]);
        StartCoroutine(FieldEvents.CoolDown(0.3f));
    }

    public void OpenAlert(Dialogue[] _dialogue)

    {
        prefabToUse = alertBoxPrefab;

        dialogue = _dialogue;
        dialogueElement = 0;
        dialogueLength = dialogue.Length;
        SpawnDialogueBox();
        dialogueIsActive = true;
        dialogueBox.DisplayMessage(dialogue[dialogueElement]);
        StartCoroutine(FieldEvents.CoolDown(0.3f));
    }

    public void SpawnDialogueBox()

    {
        dialogueBoxGameObject = Instantiate(prefabToUse, this.transform);
        dialogueBoxGameObject.name = dialogue[0].dialogueGameObject.name + "Dialogue#" + dialogueElement;
        dialogueCurrentlyInPlay = dialogueBoxGameObject.name;
        dialogueBox = dialogueBoxGameObject.GetComponent<DialogueBox>();
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
            dialogueBox.animator.SetTrigger("CloseDialogue");
            dialogueElement = -1; //just set it to something negative so we know it's cooked
            dialogueIsActive = false;

            yield return new WaitForSeconds(0.3f);

            FieldEvents.isDialogueActive = false;

            CombatEvents.UnlockPlayerMovement?.Invoke();
            FieldEvents.HasCompleted.Invoke(dialogue[0].dialogueGameObject);
        }

        if (dialogueElement < dialogue.Length && dialogueElement != -1)
         {
            dialogueBox.animator.SetTrigger("CloseDialogue");

            yield return new WaitForSeconds(0.3f);

            dialogueElement++;
            SpawnDialogueBox();
            dialogueBox.DisplayMessage(dialogue[dialogueElement]);
        }
    }
}

