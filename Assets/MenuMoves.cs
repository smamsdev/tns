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
    IMenuMoveTypeHighlighted buttonTypeToReturnTo;
    public MoveSlot[] moveSlotGOs = new MoveSlot[5];

    public override void DisplayMenu(bool on)
    {
        moveDescriptionGO.SetActive(false);
        displayContainer.SetActive(on);
        LoadEquippedMovesFromSO();
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitMenu();
            menuManagerUI.menuUpdateMethod = menuManagerUI.main;
        }
    }

    void LoadEquippedMovesFromSO()
    {
        for (int i = 0; i < moveSlotGOs.Length; i++)
        {
          //  GameObject.Find(playerEquippedMoves[i].move)
          //  moveSlotGOs[i].move = playerEquippedMoves[i].move;
        }

        //GameObject.Find("Player").GetComponent<EquippedGear>().playerPermanentStats.attackPowerGearMod
    }
}
