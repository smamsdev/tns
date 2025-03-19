using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CombatMenuManager : MonoBehaviour
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

    [Header("First Buttons to Highlight")]

    public Button firstMenuFirstButton; 
    public Button secondMenuFirstButton;
    public Button thirdMenuFirstButton;
    public Button targetMenuFirstButton;

    private void Start()
    {
        DisableMenuState();
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

    public void DisableMenuState()
    {
        firstMoveMenu.SetActive(false);
        secondMoveMenu.SetActive(false);
        enemySelectMenu.SetActive(false);
        attackTargetMenu.SetActive(false);
        GearSelectMenu.SetActive(false);
    }
}