using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DialogueBox : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI nameText;
    public GridLayoutGroup gridLayoutSizer;
    public Animator animator;
    SpriteRenderer speakerSpriteRenderer;

    Vector2 actorPos;
    public Vector2 dialogueFinalPosition;

    public Dialogue dialogueElement;

    public void DisplayMessage(Dialogue dialogueElement)
    {
        this.dialogueElement = dialogueElement;
        speakerSpriteRenderer = dialogueElement.actorGameObject.GetComponent<SpriteRenderer>();

        SetupTextBox();

        SetFinalPosition();
        animator.SetTrigger("OpenDialogue");

        StartCoroutine(AnimateDialoguePositionCoRoutine(dialogueFinalPosition)); ;
        StartCoroutine(AnimateLetters(dialogueElement.dialoguetext, dialogueText, 0.02f));
    }

    void SetFinalPosition()
    {
        actorPos = dialogueElement.actorGameObject.transform.position;

        dialogueFinalPosition.x = actorPos.x - 0.00f;
        //0.3 above the speakers highest sprite point
        dialogueFinalPosition.y = actorPos.y + speakerSpriteRenderer.bounds.size.y + 0.3f;
    }

    void SetupTextBox()
    //set the name instantly, and instantiate the background of the dialog box to be the correct size BEFORE the text appears
    {
        string actorname = dialogueElement.actorGameObject.name;

        if (actorname == "Player") 
        { 
            actorname = "Liam"; 
        }

        nameText.text = actorname;

        dialogueText.text = dialogueElement.dialoguetext;

        var tmps = new List<TextMeshProUGUI> { dialogueText, nameText };
        var longestTMP = FieldEvents.FindLongestText(tmps);

        Vector2 newCellSize = gridLayoutSizer.cellSize;
        newCellSize = new Vector2(longestTMP.preferredWidth, dialogueText.preferredHeight + nameText.preferredHeight);
        gridLayoutSizer.cellSize = newCellSize;
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
        Vector2 startingPos = new Vector2 (actorPos.x, actorPos.y + speakerSpriteRenderer.bounds.size.y - 0.1f);
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