using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueBox : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI actorNameText;
    public TextMeshProUGUI dialogueText;
    [SerializeField] TextMeshProUGUI backgroundActorNameText;
    [SerializeField] TextMeshProUGUI backgroundDialogueText;
    public string dialogueId;
    public Animator animator;

    [SerializeField] Transform dialoguePosition;

    public Dialogue[] dialogue;

    public void DisplayMessage(Dialogue dialogue)

    {
        animator.SetTrigger("OpenDialogue");
        dialoguePosition.position = dialogue.actorGameObject.GetComponent<Transform>().position;

        StartCoroutine(AnimateDialoguePositionCoRoutine(new Vector2(dialogue.dialogueFinalPosition.x, dialogue.dialogueFinalPosition.y), 0.4f));;

        backgroundActorNameText.text = dialogue.actorGameObject.name;
        backgroundDialogueText.text = dialogue.dialoguetext;
        actorNameText.text = dialogue.actorGameObject.name;

        StartCoroutine(AnimateLetters(dialogue.dialoguetext, dialogueText, 0.02f));
    }

    IEnumerator AnimateLetters(string incomingDialogue, TextMeshProUGUI textToUpdate, float time)

    {
        textToUpdate.text = "";
        yield return new WaitForSeconds(0.5f);

        foreach (char letter in incomingDialogue.ToCharArray()) 
        {
            textToUpdate.text += letter;
            yield return new WaitForSeconds(time);
        }
    }

    public IEnumerator AnimateDialoguePositionCoRoutine(Vector2 finalPosition, float seconds)
    {
        float elapsedTime = 0;
        Vector2 startingPos = dialoguePosition.localPosition;
        while (elapsedTime < seconds)
        {
            dialoguePosition.localPosition = Vector2.Lerp(startingPos, finalPosition, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

}

