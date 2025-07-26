using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectEnemyMenuScript : MonoBehaviour
{
    [SerializeField] CombatManager combatManager;
    public GameObject[] buttonSlotGOs;
    public Enemy enemyhighlighted;

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
            enemySelectButtonScript.enemy = combatManager.enemies[i];
            enemySelectButtonScript.buttonText.text = enemySelectButtonScript.enemy.name;
        }
    }

    public void HighlightEnemy(Enemy enemy)
    {
        combatManager.cameraFollow.transformToFollow = enemy.transform;
        enemyhighlighted = enemy;

        var enemyUI = enemy.combatantUI as EnemyUI;
        enemyUI.selectedAnimator.SetBool("flash", true);
    }

    public void DeselectEnemy(Enemy enemy)
    {
        var enemyUI = enemy.combatantUI as EnemyUI;
        enemyUI.selectedAnimator.SetBool("flash", false);
    }

    public void SelectedEnemyToRevertToOnBack(Button button)
    { 
        combatManager.combatMenuManager.thirdMenuFirstButton = button;
    }
}
