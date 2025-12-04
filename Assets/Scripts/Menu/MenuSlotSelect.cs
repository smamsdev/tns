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
    public MenuMoveInventory menuMoveInventory;
    public IMenuMoveStyleHighlighted iMenuMoveStyleHighlighted;

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

    public void MoveSlotSelected(MoveSlot moveSlot)
    {
        if (moveSlot.moveSO == null || !moveSlot.moveSO.IsFlaw)
        {
            moveSlotHighlighted = moveSlot;
            menuMoveInventory.moveSlotToEquipTo = moveSlot;
            menuManagerUI.EnterMenu(menuMoveInventory);
        }
    }

    public void UnassignSlot()
    {
        var movesPage = (MenuMoves)menuManagerUI.movesPage;

        MoveSlot moveSlotToRemove = moveSlotHighlighted;

        if (moveSlotToRemove.moveSO == null)
        {
            return;
        }

        moveSlotToRemove.moveSO.isEquipped = false;
        moveSlotToRemove.moveSO = null;
        moveSlotToRemove.slotText.text = "Slot " + (int.Parse(moveSlotHighlighted.name) + 1) + ": Free";
        menuMoveInventory.stringArrayToUpdateInSO[int.Parse(moveSlotHighlighted.name)] = null;
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
