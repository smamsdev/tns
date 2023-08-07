using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CombatUIScript : MonoBehaviour
{
    [SerializeField] GameObject firstMoveMenu;
    [SerializeField] GameObject targetDisplayContainer;

    [SerializeField] Display1stMoveScript displayFirstMoveText;

    [SerializeField] Display2ndMoveScript displaySecondMoveText;
    public GameObject secondMoveMenu;

    [SerializeField] GameObject targetmenu;
    [SerializeField] AttackTargetMenuScript attackTargetMenuScript;

    public Button secondAttackButton; //to auto select attack on 2nd menu

    public Button firstAttackButton; //to auto select attack on 1st menu
    public Button firstFocusButton; // to autphighlight when other 2 options are disabled

    public int firstMoveIs;
    public int secondMoveIs;


    public bool firstdMoveIsBeingDecided;
    public bool secondMoveIsBeingDecided;

    private void Start()
    {
        secondAttackButton.GetComponent<Button>();  //to auto select attack on 2nd menu
        firstAttackButton.GetComponent<Button>();

        firstdMoveIsBeingDecided = false;
        secondMoveIsBeingDecided = false;

    }

    public void ShowFirstMoveMenu()

    {
  

        if (firstdMoveIsBeingDecided == false)
        {
            firstMoveMenu.SetActive(true);

            //CheckPotIsHighEnoughToInput();

            firstAttackButton.Select();

            displayFirstMoveText.UpdateFirstDisplayText("First Move");
            displaySecondMoveText.UpdateSecondDisplayText("Second Move");

            firstdMoveIsBeingDecided = true;
        }

    }

    public void ShowSecondMoveMenu()

    {
        if (secondMoveIsBeingDecided == false)
        {
            StartCoroutine(HideFirstMoveAfterDelay());     //makes nice colored text highlight linger
        }

        IEnumerator HideFirstMoveAfterDelay()

        {
            yield return new WaitForSeconds(0.2f);
            firstMoveMenu.SetActive(false);
            secondMoveMenu.SetActive(true);
            secondAttackButton.Select();
            secondMoveIsBeingDecided = true;
        }
    }

   




    public void HideTargetMenu()

    {
        targetmenu.SetActive(false);
        attackTargetMenuScript.EnableSecondMoveButtonsAgainForNextTurn();

    }

    public void HideSecondMenu()

    {
        secondMoveMenu.SetActive(false);

    }



    public void CheckPotIsHighEnoughToInput()
    {

//       if (playerStats.playerCurrentPotential <= 0)
//       {
//           firstAttackButton.interactable = false;
//           firstDefendButton.interactable = false;
//
//           firstFocusButton.Select();
//
//       }
//
//       else
//       {
//           firstAttackButton.interactable = true;
//           firstDefendButton.interactable = true;
//           firstAttackButton.Select();
//       }
    }


}