using UnityEngine;

[System.Serializable]

public class Dialogue

{
    public GameObject actorGameObject;
    [HideInInspector] public string actorName;
    [TextArea(2, 5)] public string dialoguetext;
    public Vector3 optionalDialogueFinalPosition;
    [HideInInspector] public GameObject dialogueGameObject;
}