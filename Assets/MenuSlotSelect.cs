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
    public GameObject moveSlotGOHighlighted;
    public MenuMoveInventory menuMoveInventory;

    public TextMeshProUGUI movePropertyTMP;

    public IMenuMoveStyleHighlighted iMenuMoveStyleHighlighted;

    private void OnEnable()
    {
        movePropertyTMP.text = "";
    }

    public void SlotSelected()
    {
        var moveSlotSelected = menuMoveInventory.moveSlotToEquipTo.GetComponent<MoveSlot>();
        var move = moveSlotSelected.move;

        if (move == null || !move.isFlaw)
        {
            menuManagerUI.EnterSubMenu(menuMoveInventory);
        }
    }

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
        ToggleHighlightMenu(false); //have to toggle this damn thing off because of the deselecter

        displayContainer.SetActive(true);
        moveSlotGOHighlighted = firstButtonToHighlight.gameObject;

        ColorBlock colors = mainButtonToRevert.colors;
        colors.normalColor = colourForSelectedParent;
        mainButtonToRevert.colors = colors;

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

    public void UnassignSlot()
    {
        var movesPage = (MenuMoves)menuManagerUI.movesPage;
        IHighlightMoveSlotSelect highlightMoveSlotSelect = moveSlotGOHighlighted.GetComponent<IHighlightMoveSlotSelect>();

        MoveSlot moveSlotToRemove = moveSlotGOHighlighted.GetComponent<MoveSlot>();

        if (moveSlotToRemove.move == null)
        {
            return;
        }

        moveSlotToRemove.move.isEquipped = false;
        moveSlotToRemove.move = null;
        moveSlotToRemove.textMeshProUGUI.text = "Slot " + (int.Parse(moveSlotGOHighlighted.name) + 1) + ": Free";
        menuMoveInventory.stringArrayToUpdateInSO[int.Parse(moveSlotGOHighlighted.name)] = null;

        movesPage.LoadAllMoveLists();

        highlightMoveSlotSelect.UpdateDescriptionText();
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
