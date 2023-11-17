using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DialogueBox : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    [SerializeField] TextMeshProUGUI backgroundActorNameText;
    [SerializeField] TextMeshProUGUI backgroundDialogueText;
    public string dialogueId;
    public Animator animator;

    Vector2 startPosition;

    public Dialogue[] dialogue;

    public void DisplayMessage(Dialogue dialogue)

    {
        // if there is no end position set in the inspector, default the position to slightly above center of its GameObject
        if (dialogue.dialogueFinalPosition.x == 0 && dialogue.dialogueFinalPosition.y == 0)

        {
            dialogue.dialogueFinalPosition.x = 0;
            dialogue.dialogueFinalPosition.y = 0.8f;

        }

            animator.SetTrigger("OpenDialogue");

        if (dialogue.actorGameObject == null)

        {
            startPosition = this.transform.position;
        }

        else

        {
            startPosition = dialogue.actorGameObject.transform.position;
        }
            StartCoroutine(AnimateDialoguePositionCoRoutine(new Vector2((dialogue.actorGameObject.transform.position.x + dialogue.dialogueFinalPosition.x), (dialogue.actorGameObject.transform.position.y + dialogue.dialogueFinalPosition.y)), 0.4f)); ;

        //if there is no Gameobject set in inspector, then hide the name
        if (dialogue.actorGameObject == null)
        {
            backgroundActorNameText.text = "";
        }

        else
        {
            backgroundActorNameText.text = dialogue.actorGameObject.name;
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
        Vector2 startingPos = startPosition;
        while (elapsedTime < seconds)
        {
            this.transform.position = Vector2.Lerp(startingPos, finalPosition, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    public void DestoryDialogueBox() { Destroy(this.gameObject); }
}

