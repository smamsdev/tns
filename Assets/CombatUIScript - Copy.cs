using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatUIScript2 : MonoBehaviour
{
    [SerializeField] GameObject firstMoveMenu;
    [SerializeField] Display1stMoveScript displayFirstMoveText;

    [SerializeField] Display2ndMoveScript displaySecondMoveText;
    public GameObject secondMoveMenu;

    [SerializeField] GameObject targetmenu;
    [SerializeField] AttackTargetMenuScript attackTargetMenuScript;

    [SerializeField] Button firstAttack; //to auto select attack on 2nd menu
    [SerializeField] Button secondAttack; //to auto select attack on 2nd menu

    public int firstMoveIs;
    public int secondMoveIs;




    private void Start()
    {
        secondAttack.GetComponent<Button>();  //to auto select attack on 2nd menu


    }

    public void ShowFirstMoveMenu()

    {
        firstMoveMenu.SetActive(true);
        secondMoveMenu.SetActive(false);
        firstAttack.Select();

        displayFirstMoveText.UpdateFirstDisplayText("First Move");
        displaySecondMoveText.UpdateSecondDisplayText("Second Move");




    }





    public void HideFirstMove ()

    {
        StartCoroutine(HideFirstMoveAfterDelay());     //makes nice colored text highlight linger
              
    }

    private IEnumerator HideFirstMoveAfterDelay()

    { yield return new WaitForSeconds(0.1f);
        firstMoveMenu.SetActive(false);
        secondMoveMenu.SetActive(true);
        secondAttack.Select();
    }




    public void HideTargetMenu()

    {
     targetmenu.SetActive(false);
     ShowFirstMoveMenu();
    }

    public void HideSecondMenu()

    {
        secondMoveMenu.SetActive(false);

    }



    public void FirstAttack()

    {

        displayFirstMoveText.UpdateFirstDisplayText("Attack");
        firstMoveIs = 1;

    }


    public void FirstDefend()

    {

        displayFirstMoveText.UpdateFirstDisplayText("Defend");
        firstMoveIs = 2;
    }

    public void FirstFocus()

    {

        displayFirstMoveText.UpdateFirstDisplayText("Focus");
        firstMoveIs = 3;
    }

    //second move
    public void SecondAttack()

    {
        displaySecondMoveText.UpdateSecondDisplayText("Attack");
        secondMoveIs = 1;
    }


    public void SecondDefend()

    {
        displaySecondMoveText.UpdateSecondDisplayText("Defend");
        secondMoveIs = 2;
    }

    public void SecondFocus()

    {
        displaySecondMoveText.UpdateSecondDisplayText("Focus");
        secondMoveIs = 3;
    }


}
