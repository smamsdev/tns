using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using TMPro;

public class MenuSlotSelect : Menu
{
    public Button firstButtonToHighlight;
    public Color colourForSelectedParent;
    public MenuMoveType menuMoveTypeScript;
    public MoveSlot moveSlotHighlighted;
    public MenuMoves menuMoves;
    public MenuMoveInventory menuMoveInventory;
    public IMenuMoveStyleHighlighted iMenuMoveStyleHighlighted;
    public List<MoveSO> equippedMoveListOfType;

    public override void DisplayMenu(bool on)
    {
        throw new System.NotImplementedException();
    }

    public void ToggleHighlightMenu(bool isOn)

    {
        iMenuMoveStyleHighlighted.enabled = isOn;
    }

    public override void EnterMenu()
    {
        if (moveSlotHighlighted == null)
        {
            moveSlotHighlighted = firstButtonToHighlight.GetComponent<MoveSlot>();
        }

        ToggleHighlightMenu(false); //have to toggle this damn thing off because of the deselecter
        displayContainer.SetActive(true);

        ColorBlock colors = mainButtonToRevert.colors;
        colors.normalColor = colourForSelectedParent;
        mainButtonToRevert.colors = colors;

        firstButtonToHighlight = moveSlotHighlighted.GetComponent<Button>();
        firstButtonToHighlight.Select();
    }

    public override void ExitMenu()
    {
        ColorBlock colors = mainButtonToRevert.colors;
        colors.normalColor = Color.white;
        mainButtonToRevert.colors = colors;

        ToggleHighlightMenu(true);
        mainButtonToRevert.Select();
        menuManagerUI.menuUpdateMethod = menuMoveTypeScript;
    }

    void MoveSlotHighlighted(MoveSlot moveSlot)
    { 
        moveSlotHighlighted = moveSlot;
    }

    public void MoveSlotSelected(MoveSlot moveSlotToEquipTo)
    {
        if (moveSlotToEquipTo.moveSO == null || !moveSlotToEquipTo.moveSO.IsFlaw)
        {
            moveSlotHighlighted = moveSlotToEquipTo;
            menuMoveInventory.MoveSlotToEquipTo(moveSlotToEquipTo);
            menuManagerUI.EnterMenu(menuMoveInventory);
        }
    }

    public void UnassignSlot()
    {
        var movesPage = (MenuMoves)menuManagerUI.movesPage;

        MoveSlot moveSlotToRemove = moveSlotHighlighted;

        if (moveSlotToRemove.moveSO == null || moveSlotToRemove.moveSO.IsFlaw)
        {
            return;
        }

        menuMoves.playerMoveManager.playerMoveInventorySO.UnequipMove(moveSlotToRemove.moveSO);
        moveSlotToRemove.moveSO = null;
        movesPage.LoadAllEquippedMovesToUISlots();
        moveSlotToRemove.UpdateMoveDescriptionText();
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
