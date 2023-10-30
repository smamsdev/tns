using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DialogueContainer : MonoBehaviour
{
    [SerializeField] DialogueManager dialogueManager;

    public Dialogue[] dialogue;
    public bool dialogueLaunched;

    public void StartDialogue()

    {
        dialogue[0].dialogueGameObjectID = this.gameObject.name;
        dialogueManager.OpenDialogue(dialogue);
        dialogueLaunched = true;
    }
}

[System.Serializable]

public class Dialogue

{
    public GameObject actorGameObject;
    [HideInInspector] public string actorName;
    [TextArea(2, 5)] public string dialoguetext;
    public Vector2 dialogueFinalPosition;
    [HideInInspector] public Transform actorPosition;
    [HideInInspector] public GameObject dialogueBoxGameObject;
    [HideInInspector] public string dialogueGameObjectID;
}

