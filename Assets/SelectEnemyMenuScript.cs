using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectEnemyMenuScript : MonoBehaviour
{
    [SerializeField] CombatManager combatManager;
    public GameObject[] buttonSlot;

    public void InitializeButtonSlots()
    {
        for (int i = 0; i < combatManager.enemies.Count; i++)

        {
            Button button = buttonSlot[i].GetComponent<Button>();
            button.interactable = true;

            var enemySelectButtonScript = button.GetComponent<EnemySelectButtonScript>();
            enemySelectButtonScript.enemy = combatManager.enemies[i];
            enemySelectButtonScript.buttonText.text = enemySelectButtonScript.enemy.name;
        }
    }

    public void HighlightEnemy(Enemy enemy)
    {
        enemy.combatantUI.statsDisplay.statsDisplayGameObject.SetActive(true);
        combatManager.cameraFollow.transformToFollow = enemy.transform;
    }

    public void DeselectEnemy(Enemy enemy)
    {
        enemy.combatantUI.statsDisplay.statsDisplayGameObject.SetActive(false);
    }

    public void SelectedEnemyToRevertToOnBack(Button button)
    { 
        combatManager.combatMenuManager.thirdMenuFirstButton = button;
    }
}
