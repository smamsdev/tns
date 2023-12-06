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
    public Animator animator;

    public Vector2 startPosition;
    public Vector2 dialogueFinalPosition;

    public Dialogue dialogueElement;

    public void DisplayMessage(Dialogue _dialogueElement)

    {
        dialogueElement = _dialogueElement;

        SetFinalPosition();
        animator.SetTrigger("OpenDialogue");
        SetTextAndName();
        StartCoroutine(AnimateDialoguePositionCoRoutine(new Vector2((dialogueFinalPosition.x), (dialogueFinalPosition.y)), 0.4f)); ;
        StartCoroutine(AnimateLetters(dialogueElement.dialoguetext, dialogueText, 0.02f));
    }

    void SetFinalPosition()

    {
        // if there is no Optional end position set in the inspector...
        if (dialogueElement.optionalDialogueFinalPosition.x == 0 && dialogueElement.optionalDialogueFinalPosition.y == 0)

        {
            //place it a little above and center the ActorGO 
            dialogueFinalPosition.y = 0.8f;
            dialogueFinalPosition.x = 0;

            //tweak it a bit based on look direction
            if (dialogueElement.actorGameObject != GameObject.Find("Player"))

            {
                if (FieldEvents.lookDirection == Vector2.right)
                {
                    dialogueFinalPosition.x = 0.4f;
                }

                if (FieldEvents.lookDirection == Vector2.left)
                {
                    dialogueFinalPosition.x = -0.4f;
                }

                if (FieldEvents.lookDirection == Vector2.up)
                {
                    dialogueFinalPosition.x = 0.7f;
                    dialogueFinalPosition.y = -0.2f;
                }

                if (FieldEvents.lookDirection == Vector2.down)
                {
                    dialogueFinalPosition.x = 0.4f;
                    dialogueFinalPosition.y = 0.9f;
                }
            }

            if (dialogueElement.actorGameObject == GameObject.Find("Player"))

            {
                if (FieldEvents.lookDirection == Vector2.right)
                {
                    dialogueFinalPosition.x = -0.4f;
                }

                if (FieldEvents.lookDirection == Vector2.left)
                {
                    dialogueFinalPosition.x = 0.8f;
                }

                if (FieldEvents.lookDirection == Vector2.up)
                {
                    dialogueFinalPosition.x = -0.4f;
                    dialogueFinalPosition.y = -0.3f;
                }

                if (FieldEvents.lookDirection == Vector2.down)
                {
                    dialogueFinalPosition.x = -0.3f;
                    dialogueFinalPosition.y = 1.0f;
                }
            }
        }

        else 
        {
            dialogueFinalPosition = dialogueElement.optionalDialogueFinalPosition;
        }
    }

    void SetTextAndName()

    {
        //if there is no Gameobject set in inspector, then hide the name
        if (dialogueElement.actorGameObject == null)
        {
            backgroundActorNameText.text = "";
        }

        else
        {
            backgroundActorNameText.text = dialogueElement.actorGameObject.name;
        }

        backgroundDialogueText.text = dialogueElement.dialoguetext;
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
            this.transform.localPosition = Vector2.Lerp(startingPos, finalPosition, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        this.transform.localPosition = finalPosition;
    }

    public void DestoryDialogueBox() { Destroy(this.gameObject); }
}

