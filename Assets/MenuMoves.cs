using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class MenuMoves : Menu
{
    [SerializeField] Button firstButtonToSelect;
    public GameObject moveDescriptionGO;
    public PlayerEquippedMovesSO playerEquippedMoves;
    public PlayerMoveInventorySO playerMoveInventory;
    public bool isSelectingMove;
    Button buttonTypeToReturnTo;


    public override void DisplayMenu(bool on)
    {
        moveDescriptionGO.SetActive(false);
        displayContainer.SetActive(on);
    }

    public override void EnterMenu()
    {
        menuButtonHighlighted.SetButtonColor(menuButtonHighlighted.highlightedColor);
        menuButtonHighlighted.enabled = false;

        firstButtonToSelect.Select();
        moveDescriptionGO.SetActive(true);
    }

    public override void ExitMenu() 
    {
        menuButtonHighlighted.enabled = true;
        menuButtonHighlighted.SetButtonColor(Color.white);
        mainButtonToRevert.Select();
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isSelectingMove)
        {
            ExitMenu();
            menuManagerUI.menuUpdateMethod = menuManagerUI.main;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && isSelectingMove)
        {
            buttonTypeToReturnTo.Select();
            isSelectingMove = false;
        }
    }

    public void EnterMoveList(Button buttonToHighlight)
    {
        buttonToHighlight.Select();
        isSelectingMove = true;
    }

    public void AssignButtonToReturn(Button _buttonTypeToReturnTo)
    {
        buttonTypeToReturnTo = _buttonTypeToReturnTo;
    }


}
