using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DialogueBox : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI nameFieldBackground;
    [SerializeField] TextMeshProUGUI dialogueFieldBackground;
    public Animator animator;

    public Vector2 startPosition;
    public Vector2 dialogueFinalPosition;

    public Dialogue dialogueElement;

    public void DisplayMessage(Dialogue _dialogueElement)

    {
        dialogueElement = _dialogueElement;

        SetFinalPosition();
        animator.SetTrigger("OpenDialogue");
        SetTextBackgroundAndName();
        StartCoroutine(AnimateDialoguePositionCoRoutine(new Vector2((dialogueFinalPosition.x), (dialogueFinalPosition.y)), 0.5f)); ;
        StartCoroutine(AnimateLetters(dialogueElement.dialoguetext, dialogueText, 0.02f));
    }

    void SetFinalPosition()

    {
        // if there is no Optional end position set in the inspector...
        if (dialogueElement.optionalDialogueFinalPosition.x == 0 && dialogueElement.optionalDialogueFinalPosition.y == 0)

        {
            //place it a little above and center the ActorGO 
            dialogueFinalPosition.y = 0.8f;
            dialogueFinalPosition.x = -0.3f;

            //tweak it a bit based on look direction
            if (dialogueElement.actorGameObject != GameObject.Find("Player"))

            {
                if (FieldEvents.lookDirection == Vector2.right)
                {
                    dialogueFinalPosition.x = 0.1f;
                    dialogueFinalPosition.y = 1.0f;

                    startPosition = new Vector2(-0.1f, 0.4f);

                }

                if (FieldEvents.lookDirection == Vector2.left)
                {
                    dialogueFinalPosition.x = -0.4f;

                    startPosition = new Vector2(-0.3f, 0.4f);
                }

                if (FieldEvents.lookDirection == Vector2.up)
                {
                    dialogueFinalPosition.x = 0.3f;
                    dialogueFinalPosition.y = 0.7f;

                    startPosition = new Vector2(-0.4f, 0.3f);
                }

                if (FieldEvents.lookDirection == Vector2.down)
                {
                    dialogueFinalPosition.x = 0.3f;
                    dialogueFinalPosition.y = 0.5f;

                    startPosition = new Vector2(-0.3f, 0.3f);
                }
            }

            if (dialogueElement.actorGameObject == GameObject.Find("Player"))

            {
                if (FieldEvents.lookDirection == Vector2.right)
                {
                    dialogueFinalPosition.x = -0.7f;
                    dialogueFinalPosition.y = 1.0f;

                    startPosition = new Vector2(-0.5f, 0.45f);
                }

                if (FieldEvents.lookDirection == Vector2.left)
                {
                    dialogueFinalPosition.x = 0.8f;

                    startPosition = new Vector2(0.1f, 0.45f);
                }

                if (FieldEvents.lookDirection == Vector2.up)
                {
                    dialogueFinalPosition.x = -0.5f;
                    dialogueFinalPosition.y = -0.45f;

                    startPosition = new Vector2(-0.53f, 0f);
                }

                if (FieldEvents.lookDirection == Vector2.down)
                {
                    dialogueFinalPosition.x = -0.4f;
                    dialogueFinalPosition.y = 1.1f;

                    startPosition = new Vector2(-0.0f, 0.55f);
                }
            }
        }

        else 
        {
            dialogueFinalPosition = dialogueElement.optionalDialogueFinalPosition;
        }
    }


    void SetTextBackgroundAndName()
        //set the name instantly, and instantiate the background of the dialog box to be the correct size BEFORE the text appears
    {

        //if there is no Gameobject set in inspector, then hide the name
        if (dialogueElement.actorGameObject == null)
        {
            nameText.text = "";
        }

        nameText.text = dialogueElement.actorGameObject.name;

        // if the dialog is too short, throw in some extra spaces, so it doesn't look all jacked up.

        int minimumLength = 12;

        if (dialogueElement.dialoguetext.Length < minimumLength)

        {
            string extraspace = "";

            extraspace.PadRight(minimumLength - dialogueElement.dialoguetext.Length);

            nameFieldBackground.text = dialogueElement.dialoguetext + extraspace.PadRight(minimumLength - dialogueElement.dialoguetext.Length) + ".";
            dialogueFieldBackground.text = dialogueElement.dialoguetext + extraspace.PadRight(minimumLength - dialogueElement.dialoguetext.Length) + ".";

            Debug.Log(nameFieldBackground.text);
        }

        else
        {
            nameFieldBackground.text = dialogueElement.dialoguetext;
            dialogueFieldBackground.text = dialogueElement.dialoguetext;
        }

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

