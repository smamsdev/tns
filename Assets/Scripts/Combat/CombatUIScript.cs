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
    [SerializeField] CombatInventoryMenu combatInventoryMenu;
    public SelectEnemyMenuScript selectEnemyMenuScript;

    public PlayerDamageTakenDisplay playerDamageTakenDisplay;
    public EnemyDamageTakenDisplay enemyDamageTakenDisplay;
    public FendScript playerFendScript;

    TextMeshProUGUI textMeshProUGUIStyleDisplay;
    TextMeshProUGUI textMeshProUGUIMoveDisplay;

    [SerializeField] GameObject StyleDisplay;
    [SerializeField] GameObject MoveDisplay;

    public Button secondAttackButton; //to auto select attack on 2nd menu

    public Button firstAttackButton; //to auto select attack on 1st menu
    public Button firstFocusButton; // to autphighlight when other 2 options are disabled

    public int firstMoveIs;
    public int secondMoveIs;

    private void OnEnable()
    {
        CombatEvents.UpdateFirstMoveDisplay += UpdateFirstMoveDisplay;
        CombatEvents.UpdateSecondMoveDisplay += UpdateSecondMoveDisplay;

        secondAttackButton.GetComponent<Button>();  //to auto select attack on 2nd menu
        firstAttackButton.GetComponent<Button>();

        textMeshProUGUIStyleDisplay = StyleDisplay.GetComponentInChildren<TextMeshProUGUI>();
        textMeshProUGUIMoveDisplay = MoveDisplay.GetComponentInChildren<TextMeshProUGUI>();

    }

    private void OnDisable()
    {
        CombatEvents.UpdateFirstMoveDisplay -= UpdateFirstMoveDisplay;
        CombatEvents.UpdateSecondMoveDisplay -= UpdateSecondMoveDisplay;
    }

    public void ShowFirstMoveMenu()

    {
        CombatEvents.InputCoolDown?.Invoke(0.2f);
        firstAttackButton.Select();
                
        firstMoveMenu.SetActive(true);
        StyleDisplay.SetActive(true);
        MoveDisplay.SetActive(true);


        secondMoveMenu.SetActive(false);
        targetmenu.SetActive(false);

        CombatEvents.HighlightBodypartTarget?.Invoke(false, false, false);

        UpdateFirstMoveDisplay("Style?");
        UpdateSecondMoveDisplay("Move?");
    }

    public void ShowSecondMoveMenu()

    {
        secondAttackButton.Select();
        CombatEvents.InputCoolDown?.Invoke(0.2f);


       CombatEvents.HighlightBodypartTarget?.Invoke(false, false, false);

       attackTargetMenuScript.EnableSecondMoveButtonsAgainForNextTurn();

        firstMoveMenu.SetActive(false);
        targetmenu.SetActive(false);
        secondMoveMenu.SetActive(true);
        attackTargetMenuScript.targetSelected = false;

        UpdateSecondMoveDisplay("Move?");
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
        textMeshProUGUIStyleDisplay.text = value;
    }

    void UpdateSecondMoveDisplay(string value)

    {
        textMeshProUGUIMoveDisplay.text = value;
    }

    void HighlightFirstAttack()

    {
        firstAttackButton.Select();
    }


}