using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectEnemyMenuScript : MonoBehaviour
{
    [SerializeField] CombatManager combatManager;
    public GameObject[] buttonSlotGOs;
    public Combatant enemyhighlighted;

    public void InitializeButtonSlots()
    {
        foreach (GameObject buttonGO in buttonSlotGOs)
        {
            buttonGO.SetActive(false);
        };

        for (int i = 0; i < combatManager.enemies.Count; i++)
        {
            buttonSlotGOs[i].SetActive(true);

            Button button = buttonSlotGOs[i].GetComponent<Button>();
            EnemySelectButtonScript enemySelectButtonScript = button.GetComponent<EnemySelectButtonScript>();
            enemySelectButtonScript.combatant = combatManager.enemies[i];
            enemySelectButtonScript.buttonText.text = enemySelectButtonScript.combatant.name;
        }
    }

    public void HighlightEnemy(Combatant combatant)
    {
        combatManager.cameraFollow.transformToFollow = combatant.transform;
        enemyhighlighted = combatant;

        var enemyUI = combatant.combatantUI as EnemyUI;
        enemyUI.selectedAnimator.SetBool("flash", true);
    }

    public void DeselectEnemy(Combatant combatant)
    {
        var enemyUI = combatant.combatantUI as EnemyUI;
        enemyUI.selectedAnimator.SetBool("flash", false);
    }

    public void SelectedEnemyToRevertToOnBack(Button button)
    { 
        combatManager.combatMenuManager.thirdMenuFirstButton = button;
    }
}
