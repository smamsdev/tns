using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DialogueScript : MonoBehaviour
{
    [SerializeField] DialogueManager dialogueManager;

    [SerializeField] Dialogue[] dialogue;
    public bool dialogueCompleted;

    public void StartDialogue()

    {
        dialogueManager.OpenDialogue(dialogue);
        dialogueCompleted = true;

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
}

