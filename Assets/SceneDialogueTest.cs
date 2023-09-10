using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneDialogueTest : MonoBehaviour
{
    [SerializeField] DialogueManager Vegetadialogue;
    [SerializeField] CombatManagerV2 combatManagerV2;


    private void Update()
    {

        if (Vegetadialogue.dialogueElement == 3 ) 
        
        {
            combatManagerV2.SetupBattle();
            Debug.Log("starting fight");
            Vegetadialogue.dialogueElement = -1;
        }
    }
}

