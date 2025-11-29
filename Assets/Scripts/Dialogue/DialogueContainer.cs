using System.Collections;
using UnityEngine;

public class DialogueContainer : ToTrigger
{
    private static DialogueManager _cachedDialogueManager;

    private DialogueManager DialogueManager
    {
        get
        {
            if (_cachedDialogueManager == null)
            {
                _cachedDialogueManager = GameObject.FindGameObjectWithTag("DialogManager").GetComponent<DialogueManager>();
            }
            return _cachedDialogueManager;
        }
    }

    public Dialogue[] dialogue;
    public bool dialogueLaunched;
    public bool isLoopedDialogue;

    private void Start()
    {
        dialogue[0].dialogueGameObject = gameObject;
    }

    public override IEnumerator TriggerFunction()
    {
        CombatEvents.LockPlayerMovement?.Invoke();
        FieldEvents.isDialogueActive = true;

        yield return new WaitForSeconds(0.3f);

        DialogueManager.OpenDialogue(dialogue);
        dialogueLaunched = true;
    }

    protected override void TriggerComplete()
    {
        //Not in use
        //FieldEvents.HasCompleted invoked elsewhere
    }
}
