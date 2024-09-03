using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CombatUIManager : MonoBehaviour
{
    [SerializeField] CombatManager combatManager;

    [Header("Menu GameObjects")]
    public GameObject firstMoveMenu;
    public GameObject secondMoveMenu;
    public GameObject enemySelectMenu;
    public GameObject attackTargetMenu;
    public GameObject GearSelectMenu;

    [Header("Scripts")]
    public SelectEnemyMenuScript selectEnemyMenuScript;
    public CombatInventoryMenu combatInventoryMenuScript;
    public FendScript playerFendScript;
    public PlayerDamageTakenDisplay playerDamageTakenDisplay;

    [Header("Shared UI elements")]

    [SerializeField] TextMeshProUGUI StyleDisplayText;
    [SerializeField] TextMeshProUGUI MoveDisplayText;

    [Header("First Buttons to Highlight")]

    public Button firstMenuFirstButton; 
    public Button secondMenuFirstButton;
    public Button thirdMenuFirstButton;
    public Button targetMenuFirstButton;

    private void Start()
    {
        ChangeMenuState(false);
    }

    public void ChangeMenuState(GameObject menuToEnable)

    {
        firstMoveMenu.SetActive(false);
        secondMoveMenu.SetActive(false);
        enemySelectMenu.SetActive(false);
        attackTargetMenu.SetActive(false);
        GearSelectMenu.SetActive(false);

        menuToEnable.SetActive(true);
    }

    public void ChangeMenuState(bool off)

    {
        firstMoveMenu.SetActive(false);
        secondMoveMenu.SetActive(false);
        enemySelectMenu.SetActive(false);
        attackTargetMenu.SetActive(false);
        GearSelectMenu.SetActive(false);
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