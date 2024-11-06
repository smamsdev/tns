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
        for (int i = 0; i < combatManager.enemy.Length; i++)

        {
            Button button = buttonSlot[i].GetComponent<Button>();
            button.interactable = true;
            TextMeshProUGUI text = buttonSlot[i].GetComponentInChildren<TextMeshProUGUI>();
            text.text = combatManager.enemy[i].name;
        }
    }

    public void HighlightEnemy(int enemyHighlighted)
    {
        var enemy = combatManager.enemy[enemyHighlighted];
        enemy.enemyUI.enemyStatsDisplay.enemyStatsDisplayGameObject.SetActive(true);
        combatManager.cameraFollow.transformToFollow = enemy.transform;
    }

    public void DeselectEnemy(int enemyHighlighted)
    {
        var enemy = combatManager.enemy[enemyHighlighted];
        enemy.enemyUI.enemyStatsDisplay.enemyStatsDisplayGameObject.SetActive(false);
    }

    public void SelectedEnemyToRevertToOnBack(Button button)
    { 
        combatManager.CombatUIManager.thirdMenuFirstButton = button;
    }
}
