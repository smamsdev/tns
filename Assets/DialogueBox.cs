using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DialogueBox : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI actorNameText;
    public TextMeshProUGUI dialogueText;
    [SerializeField] TextMeshProUGUI backgroundActorNameText;
    [SerializeField] TextMeshProUGUI backgroundDialogueText;
    public string dialogueId;
    public Animator animator;

    public Dialogue[] dialogue;

 
    public void DisplayMessage(Dialogue dialogue)


    {
        animator.SetTrigger("OpenDialogue");

        // Set the starting position for the dialogue box
        // if there is no Actor GameObject just default to roughly the dialogue GO postion

        if (dialogue.actorGameObject == null)

        {
            this.transform.localPosition = new Vector2(dialogue.dialogueFinalPosition.x, dialogue.dialogueFinalPosition.y);
        }


        else
        {
            this.transform.position = dialogue.actorGameObject.transform.position;
        }

        if (dialogue.dialogueFinalPosition == Vector2.zero)

        {
            this.transform.position = dialogue.dialogueDefaultTransform.position;
            dialogue.dialogueFinalPosition = new Vector2((GetComponentInParent<Transform>().localPosition.x- dialogue.dialoguetext.Length*5), (GetComponentInParent<Transform>().localPosition.y + 325));
        }

        StartCoroutine(AnimateDialoguePositionCoRoutine(new Vector2(dialogue.dialogueFinalPosition.x, dialogue.dialogueFinalPosition.y), 0.4f)); ;

        if (dialogue.actorGameObject == null)
        {
            backgroundActorNameText.text = "";
            actorNameText.text = "";
        }

        else
        {
            backgroundActorNameText.text = dialogue.actorGameObject.name;
            actorNameText.text = dialogue.actorGameObject.name;
        }

        backgroundDialogueText.text = dialogue.dialoguetext;


        StartCoroutine(AnimateLetters(dialogue.dialoguetext, dialogueText, 0.02f));
    }

    IEnumerator AnimateLetters(string dialoguetext, TextMeshProUGUI textToUpdate, float time)

    {
        textToUpdate.text = "";
        yield return new WaitForSeconds(0.5f);

        foreach (char letter in dialoguetext.ToCharArray())
        {
            textToUpdate.text += letter;
            yield return new WaitForSeconds(time);
        }
    }

    public IEnumerator AnimateDialoguePositionCoRoutine(Vector2 finalPosition, float seconds)
    {
        float elapsedTime = 0;
        Vector2 startingPos = this.transform.localPosition;
        while (elapsedTime < seconds)
        {
            this.transform.localPosition = Vector2.Lerp(startingPos, finalPosition, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    public void DestoryDialogueBox() { Destroy(this.gameObject); }
}

