using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CombatUIScript : MonoBehaviour
{
    [SerializeField] CombatManager combatManager;
    public FendScript playerFendScript;
    public PlayerDamageTakenDisplay playerDamageTakenDisplay;

    [Header("Menu GameObjects")]
    [SerializeField] GameObject firstMoveMenu;
    [SerializeField] GameObject secondMoveMenu;
    [SerializeField] GameObject enemySelectMenu;
    [SerializeField] GameObject attackTargetMenu;
    [SerializeField] CombatInventoryMenu combatInventoryMenu;

    [Header("Menu Scripts")]
    public AttackTargetMenuScript attackTargetMenuScript;
    public SelectEnemyMenuScript selectEnemyMenuScript;

    [SerializeField] TextMeshProUGUI StyleDisplayText;
    [SerializeField] TextMeshProUGUI MoveDisplayText;

    public Button firstMenuFirstButton; 
    public Button secondMenuFirstButton;
    public Button thirdMMenuFirstButton;
    public Button targetMenuFirstButton;

    public void ShowFirstMoveMenu(bool on)

    {
        if (on)
        {
            CombatEvents.InputCoolDown?.Invoke(0.2f);
            firstMoveMenu.SetActive(true);
            firstMenuFirstButton.Select();

            UpdateFirstMoveDisplay("Style?");
            UpdateSecondMoveDisplay("Move?");
        }

        if (!on)
        {
            firstMoveMenu.SetActive(false);
        }
    }

    public void ShowSecondMoveMenu(bool on)

    {
        if (on)
        {
            secondMenuFirstButton.Select();
            CombatEvents.InputCoolDown?.Invoke(0.1f);

            ShowFirstMoveMenu(false);
            ShowEnemySelectMenu(false);
            secondMoveMenu.SetActive(true);

            UpdateSecondMoveDisplay("Move?");
        }

        if (!on)
        {
            secondMoveMenu.SetActive(false);
        }
    }

    public void ShowEnemySelectMenu(bool on)

    {
        if (on) 
        {
        enemySelectMenu.SetActive(true);
            thirdMMenuFirstButton.Select();
        }

        if (!on) 
        
        {
            enemySelectMenu.SetActive(false);
        }
    }

    public void ShowBodyPartTargetMenu(bool on)

    {
        CombatEvents.InputCoolDown?.Invoke(0.1f);

        if (on)
        {
            attackTargetMenu.SetActive(true);
            targetMenuFirstButton.Select();

            combatManager.enemy[combatManager.selectedEnemy].enemyUI.partsTargetDisplay.UpdateTargetDisplay(true, false, false);
        }

        if (!on)
        {
            attackTargetMenu.SetActive(false);
            combatManager.enemy[combatManager.selectedEnemy].enemyUI.partsTargetDisplay.UpdateTargetDisplay(false, false, false);
        }
    }

    public void UpdateFirstMoveDisplay(string value)

    {
        StyleDisplayText.text = value;
    }

    public void UpdateSecondMoveDisplay(string value)

    {
        MoveDisplayText.text = value;
    }

}