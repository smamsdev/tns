using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackTargetMenuScript : MonoBehaviour
{
    [SerializeField] CombatManager combatManager;
    
    //buttons
    [SerializeField] GameObject secondAttackObj;
    [SerializeField] GameObject secondDefObj;
    [SerializeField] GameObject secondFocusObj;
    [SerializeField] GameObject secondEquipObj;

    Button secondAttackButton;
    Button secondDefButton;
    Button secondFocButton;
    Button secondEquipButton;

    //other stuff
    public GameObject attackTargetMenu;

    [SerializeField] GameObject aimBodyObj;
    [SerializeField] GameObject aimArmsObj;
    [SerializeField] GameObject aimHeadObj;

    [SerializeField] GameObject targetDisplayContainerObj;


   [SerializeField] Button aimBodyButton;
   [SerializeField] Button aimArmsButton;
   [SerializeField] Button aimHeadButton;

    public bool targetSelected;
    public int targetIsSet;


    private void Start()
    {
        targetSelected = false;

        secondAttackButton = secondAttackObj.GetComponent<Button>();
        secondDefButton = secondDefObj.GetComponent<Button>();
        secondFocButton = secondFocusObj.GetComponent<Button>();
        secondEquipButton = secondEquipObj.GetComponent<Button>();

        targetIsSet = 0;
    }

    public void DisplayAttackTargetMenu() 

    {
        CombatEvents.InputCoolDown?.Invoke(0.1f);

        if (targetSelected == false)
        {
            attackTargetMenu.SetActive(true);
            targetDisplayContainerObj.SetActive(true);
            aimBodyButton.Select();

            CombatEvents.HighlightBodypartTarget.Invoke(true, false, false);

            //disable the buttons from 2nd move  menu
            secondAttackButton.interactable = false;
            secondDefButton.interactable = false;
            secondFocButton.interactable = false;
            secondEquipButton.interactable = false;
            targetSelected = true;
        }
    }


    public void TargetBody()

    {
        combatManager.enemy[combatManager.selectedEnemy].SetEnemyBodyPartTarget(1);
    }

    public void TargetArms()

    {
        combatManager.enemy[combatManager.selectedEnemy].SetEnemyBodyPartTarget(2);
    }

    public void TargetHead()

    {
        combatManager.enemy[combatManager.selectedEnemy].SetEnemyBodyPartTarget(3);
    }

    public void EnableSecondMoveButtonsAgainForNextTurn()
    {
                secondAttackButton.interactable = true;
                secondDefButton.interactable = true;
                secondFocButton.interactable = true;
                secondEquipButton.interactable = true;
    }

}
