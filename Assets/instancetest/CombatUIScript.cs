using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CombatUIScript : MonoBehaviour
{
    [SerializeField] GameObject firstMoveMenu;

    public GameObject secondMoveMenu;

    [SerializeField] GameObject targetmenu;
    [SerializeField] AttackTargetMenuScript attackTargetMenuScript;

    [SerializeField] TextMeshProUGUI textMeshProUGUIFirstMoveDisplay;
    [SerializeField] TextMeshProUGUI textMeshProUGUISecondMoveDisplay;

    public Button secondAttackButton; //to auto select attack on 2nd menu

    public Button firstAttackButton; //to auto select attack on 1st menu
    public Button firstFocusButton; // to autphighlight when other 2 options are disabled

    public int firstMoveIs;
    public int secondMoveIs;


    public bool firstdMoveIsBeingDecided;
    public bool secondMoveIsBeingDecided;

    public bool firstAttackButtonIsHighlighted;
    public bool secondAttackButtonIsHighlighted;

    private void OnEnable()
    {
        CombatEvents.UpdateFirstMoveDisplay += UpdateFirstMoveDisplay;
        CombatEvents.UpdateSecondMoveDisplay += UpdateSecondMoveDisplay;
    }

    private void OnDisable()
    {
        CombatEvents.UpdateFirstMoveDisplay -= UpdateFirstMoveDisplay;
        CombatEvents.UpdateSecondMoveDisplay -= UpdateSecondMoveDisplay;
    }


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

            while (!firstAttackButtonIsHighlighted)

            {
                firstAttackButton.Select();
                firstAttackButtonIsHighlighted = true;

            }

            firstMoveMenu.SetActive(true);
            secondMoveMenu.SetActive(false);

            UpdateFirstMoveDisplay("First Move");
            UpdateSecondMoveDisplay("Second Move");

            firstdMoveIsBeingDecided = true;
        }

    }

    public void ShowSecondMoveMenu()

    {
        while (!secondAttackButtonIsHighlighted)

        {
            secondAttackButton.Select();
            secondAttackButtonIsHighlighted = true;
        }

        if (secondMoveIsBeingDecided == false)
        {
            StartCoroutine(HideFirstMoveAfterDelay());     //makes nice colored text highlight linger
        }

        IEnumerator HideFirstMoveAfterDelay()

        {
            attackTargetMenuScript.EnableSecondMoveButtonsAgainForNextTurn();

            yield return new WaitForSeconds(0.2f);
            firstMoveMenu.SetActive(false);
            targetmenu.SetActive(false);
            secondMoveMenu.SetActive(true);
            secondMoveIsBeingDecided = true;
            attackTargetMenuScript.targetSelected = false;
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

    void UpdateFirstMoveDisplay(string value)

    {
        textMeshProUGUIFirstMoveDisplay.text = value;
    }

    void UpdateSecondMoveDisplay(string value)

    {
        textMeshProUGUISecondMoveDisplay.text = value;
    }

    void HighlightFirstAttack()

    {
        firstAttackButton.Select();
    }


}