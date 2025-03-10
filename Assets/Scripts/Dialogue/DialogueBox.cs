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
    GameObject playerObj;

    Vector2 actorPos;
    public Vector2 dialogueFinalPosition;

    public Dialogue dialogueElement;

    public void DisplayMessage(Dialogue _dialogueElement)

    {
        playerObj = GameObject.Find("Player");
        dialogueElement = _dialogueElement;
       // this.transform.position = actorPos;

        SetFinalPosition();
        animator.SetTrigger("OpenDialogue");
        SetTextBackgroundAndName();
        StartCoroutine(AnimateDialoguePositionCoRoutine(dialogueFinalPosition)); ;
        StartCoroutine(AnimateLetters(dialogueElement.dialoguetext, dialogueText, 0.02f));
    }

    void SetFinalPosition()

    {
        actorPos = dialogueElement.actorGameObject.transform.position;
        // if there is no Optional end position set in the inspector...
        if (dialogueElement.optionalDialogueFinalPosition == Vector3.zero)

        {
            dialogueFinalPosition.x = actorPos.x - 0.00f;
            dialogueFinalPosition.y = actorPos.y + 0.8f;
        }

        else
        {
            dialogueFinalPosition = dialogueElement.optionalDialogueFinalPosition;
        }
    }

    void SetTextBackgroundAndName()
    //set the name instantly, and instantiate the background of the dialog box to be the correct size BEFORE the text appears
    {
        string actorname = dialogueElement.actorGameObject.name;

        if (actorname == "Player") 
        { 
            actorname = "Liam"; 
        }

        //if there is no Gameobject set in inspector, then hide the name
        if (dialogueElement.actorGameObject == null)
        {
            nameText.text = "";
        }

        else 
        {
            nameText.text = actorname;
        }

        // pray that this works

        int paddingLength = 3;
        string padding = new string(' ', paddingLength);

        if (dialogueElement.dialoguetext.Length < (dialogueElement.actorGameObject.name.Length))

        {
            nameFieldBackground.text = actorname + "X";
            dialogueFieldBackground.text = actorname + "X";
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

    public IEnumerator AnimateDialoguePositionCoRoutine(Vector2 finalPosition)
    {
        float elapsedTime = 0;
        float seconds = 0.4f;
        Vector2 startingPos = new Vector2 (actorPos.x, actorPos.y + 0.35f);
        while (elapsedTime < seconds)
        {
            this.transform.position = Vector2.Lerp(startingPos, finalPosition, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        this.transform.position = finalPosition;
    }

    public void DestoryDialogueBox() { Destroy(this.gameObject); }
}