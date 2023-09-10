using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackTargetMenuScript : MonoBehaviour
{
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

    public void DisplayAttackTargetMenuCoRoutine() //coroutine to stop the menu select glitching out if you pressed too fast

    { StartCoroutine(DisplayAttackTargetMenu()); }

    private IEnumerator DisplayAttackTargetMenu()
      
    {


        if (targetSelected == false) 
   
        {
            attackTargetMenu.SetActive(true);
            targetDisplayContainerObj.SetActive(true);
            aimBodyButton.Select();

            yield return new WaitForSeconds(0.3f);

            CombatEvents.HighlightBodypartTarget.Invoke(true, false, false);



            CombatEvents.UpdateNarrator.Invoke("Select Target");

            //disable the buttons from 2nd move  menu
            secondAttackButton.interactable = false;
            secondDefButton.interactable = false;
            secondFocButton.interactable = false;
            secondEquipButton.interactable = false;

            targetSelected = true;   
        } 
    }

    public void TargetBody()

    { CombatEvents.SetEnemyBodyPartTarget.Invoke(1);}

    public void TargetArms()

    { CombatEvents.SetEnemyBodyPartTarget.Invoke(2);}

    public void TargetHead()

    { CombatEvents.SetEnemyBodyPartTarget.Invoke(3);}

    public void EnableSecondMoveButtonsAgainForNextTurn()
    {
                secondAttackButton.interactable = true;
                secondDefButton.interactable = true;
                secondFocButton.interactable = true;
                secondEquipButton.interactable = true;
    }

}
