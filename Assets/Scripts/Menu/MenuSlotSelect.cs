using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using TMPro;

public class MenuSlotSelect : PauseMenu
{
    public Button firstButtonToHighlight;
    public Color colourForSelectedParent;
    public MoveSlotUI moveSlotHighlighted;
    public MoveSlotUI MoveSlotUIToRemove;
    public MenuMoves menuMoves;
    public MenuMoveInventory menuMoveInventory;
    public List<MoveSO> equippedMoveListOfType;

    public override void DisplayMenu(bool on)
    {
        throw new System.NotImplementedException();
    }

    public override void EnterMenu()
    {
        if (moveSlotHighlighted == null)
        {
            moveSlotHighlighted = firstButtonToHighlight.GetComponent<MoveSlotUI>();
        }

        displayContainer.SetActive(true);



        firstButtonToHighlight = moveSlotHighlighted.GetComponent<Button>();
        firstButtonToHighlight.Select();
    }

    public override void ExitMenu()
    {

    }

    void MoveSlotHighlighted(MoveSlotUI moveSlot)
    { 
        moveSlotHighlighted = moveSlot;
    }

    public void MoveSlotSelected(MoveSlotUI moveSlotToEquipTo)
    {
        if (moveSlotToEquipTo.moveSO == null || !moveSlotToEquipTo.moveSO.IsFlaw)
        {
            moveSlotHighlighted = moveSlotToEquipTo;
            menuMoveInventory.MoveSlotToEquipTo(moveSlotToEquipTo);
            pauseMenuManager.EnterMenu(menuMoveInventory);
        }
    }

    public void UnassignSlot()
    {
        var movesPage = (MenuMoves)pauseMenuManager.movesPage;

        MoveSlotUIToRemove = moveSlotHighlighted;

        if (MoveSlotUIToRemove.moveSO == null || MoveSlotUIToRemove.moveSO.IsFlaw)
        {
            return;
        }

        menuMoves.playerMoveManager.playerMoveInventorySO.UnequipMove(MoveSlotUIToRemove.moveSO);
        MoveSlotUIToRemove.moveSO = null;
        movesPage.LoadAllEquippedMovesToUISlots();
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitMenu();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            UnassignSlot();
        }
    }
}
