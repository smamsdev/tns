using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using TMPro;
using System;
using static PlayerMoveInventorySO;

public class MenuMoveEquipSlotSelect : PauseMenu
{
    public Color colourForSelectedParent;
    public MenuMoves menuMoves;
    public MenuMoveInventory menuMoveInventory;
    public GameObject moveSlotsParentGO, moveSlotUIPrefab;
    public MoveSlotUI[] moveEquipArrayUI;
    public int highlightedButtonIndex = 0;
    public MoveType moveType;

    public override void DisplayMenu(bool on)
    {
        throw new System.NotImplementedException();
    }

    public override void EnterMenu()
    {
        menuMoves.DisplayMenu(true);
        displayContainer.SetActive(true);
        SetMoveArrayEquipMode(true);
        moveEquipArrayUI[highlightedButtonIndex].button.Select();
        //Can't figure out why I need to do this vvv
        moveEquipArrayUI[highlightedButtonIndex].onHighlighted.Invoke();
    }

    public override void ExitMenu()
    {
        FieldEvents.SetTextColor(menuMoves.allMenuButtonHighlighteds[menuMoves.highlightedButtonIndex].tmp, Color.yellow, 1);
        pauseMenuManager.EnterMenu(menuMoves);
        SetMoveArrayEquipMode(false);
    }

    public void SetMoveArrayEquipMode(bool on)
    {
        foreach (MoveSlotUI moveSlotUI in moveEquipArrayUI)
        {
            MoveSO move = moveSlotUI.moveSO;

            bool isFlawRestricted =
                move != null &&
                move.IsFlaw &&
                !menuMoves.playerMoveInventorySO.isFlawReassignmentEnabled;

            float alpha = (on && !isFlawRestricted) ? 1f : 0.7f;

            FieldEvents.SetTextColor(moveSlotUI.slotText, moveSlotUI.slotText.color, alpha);
        }
    }

   public void InitMoveEquipSlotList()
   {
        MoveSO[] equippedMoveArray = menuMoves.playerMoveInventorySO.GetEquippedArrayOfType(moveType);
        List<Button> buttons = new();

        DeleteAllInventoryUI();
        moveEquipArrayUI = new MoveSlotUI[5];

        for (int i = 0; i < equippedMoveArray.Length; i++)
        {
            GameObject newMoveSlotGO = Instantiate(moveSlotUIPrefab, moveSlotsParentGO.transform);
            MoveSlotUI moveSlotUI = newMoveSlotGO.GetComponent<MoveSlotUI>();
            moveEquipArrayUI[i] = moveSlotUI;
            moveSlotUI.onHighlighted = () => MoveSlotHighlighted(moveSlotUI);
            moveSlotUI.button.onClick.AddListener(() => MoveSlotSelected(moveSlotUI));
            buttons.Add(moveSlotUI.button);
            FieldEvents.SetTextColor(moveSlotUI.slotText, Color.white, .7f);


            if (equippedMoveArray[i] != null)
            {
                moveSlotUI.moveSO = equippedMoveArray[i];
                moveSlotUI.slotText.text = $"Slot {i + 1}: {equippedMoveArray[i].MoveName}";
                moveSlotUI.gameObject.name = $"Slot {i + 1}: {equippedMoveArray[i].MoveName}";
                moveSlotUI.icon.sprite = equippedMoveArray[i].IsFlaw ? moveSlotUI.flawIcon : moveSlotUI.moveIcon;
                continue;
            }

            moveSlotUI.slotText.text = "Unassigned";
            moveSlotUI.gameObject.name = "Unassigned";
            moveSlotUI.icon.sprite = moveSlotUI.freeIcon;
        }

        FieldEvents.SetGridNavigationWrapAround(buttons, 5);
    }

    public void DeleteAllInventoryUI()
    {
        for (int i = moveSlotsParentGO.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(moveSlotsParentGO.transform.GetChild(i).gameObject);
        }
    }

    void MoveSlotHighlighted(MoveSlotUI moveSlotUI)
    {
        highlightedButtonIndex = System.Array.IndexOf(moveEquipArrayUI, moveSlotUI);
        UpdateMoveDescriptions(moveSlotUI);
        menuMoves.headerTMP.text = "";
    }

    public void MoveSlotSelected(MoveSlotUI moveSlotToEquipTo)
    {
        if (moveSlotToEquipTo.moveSO != null && moveSlotToEquipTo.moveSO.IsFlaw && !menuMoves.playerMoveInventorySO.isFlawReassignmentEnabled)
            return;

        //this needs to happen before checking the list count
        menuMoveInventory.menuMoveEquipSlotSelectInPlay = this;
        menuMoveInventory.moveList = menuMoves.playerMoveInventorySO.GetMoveInventoryListOfType(moveType);

        if (menuMoveInventory.moveList.Count == 0)
        {
            menuMoves.ClearAllDescriptionTMPs();
            menuMoves.moveNameTMP.text = "No moves available to assign";
            return;
        }

        menuMoveInventory.highlightedButtonIndex = 0;
        pauseMenuManager.EnterMenu(menuMoveInventory);
    }

    public void UpdateMoveDescriptions(MoveSlotUI moveSlotUI)
    {
        if (moveSlotUI.moveSO == null)
        {
            menuMoves.moveNameTMP.text = "Assign a " + GetEquippedArrayStringOfType(moveType) + " move to slot " + (highlightedButtonIndex + 1) + "?";
            menuMoves.probabilityTMP.text = "";
            menuMoves.moveDescriptionTMP.text = "";
            menuMoves.movePotentialChangeTMP.text = "";
            menuMoves.moveEquipStatusTMP.text = "";
            return;
        }

        menuMoves.moveNameTMP.text = moveSlotUI.moveSO.MoveName;
        menuMoves.probabilityTMP.text = moveSlotUI.moveSO.GetRarityDescription();
        menuMoves.moveDescriptionTMP.text = moveSlotUI.moveSO.MoveDescription;
        menuMoves.movePotentialChangeTMP.text = moveSlotUI.moveSO.PotentialChangeDescription;

        if (moveSlotUI.moveSO.IsFlaw && !menuMoves.playerMoveInventorySO.isFlawReassignmentEnabled)
        {
            menuMoves.moveEquipStatusTMP.text = "Unable to assign a FLAW";
            return;
        }

        menuMoves.moveEquipStatusTMP.text = "Press CTRL to unassign";
    }

    public string GetEquippedArrayStringOfType(MoveType moveType)
    {
        switch (moveType)
        {
            case MoveType.ViolentAttack:
                return "Violent Attack";

            case MoveType.ViolentFend:
                return "Violent Fend";

            case MoveType.ViolentFocus:
                return "Violent Focus";

            case MoveType.CautiousAttack:
                return "Cautious Attack";

            case MoveType.CautiousFend:
                return "Cautious Fend";

            case MoveType.CautiousFocus:
                return "Cautious Focus";

            case MoveType.PreciseAttack:
                return "Precise Attack";

            case MoveType.PreciseFend:
                return "Precise Fend";

            case MoveType.PreciseFocus:
                return "Precise Focus";

            default:
                Debug.Log("something went wrong");
                return null;
        }
    }

    public void UnassignSlot(MoveSlotUI moveSlotUI)
    {
        if (moveSlotUI.moveSO == null)
            return;

        if (moveSlotUI.moveSO.IsFlaw && !menuMoves.playerMoveInventorySO.isFlawReassignmentEnabled)
            return;

        menuMoves.playerMoveInventorySO.UnequipMoveFromSlot(moveType, moveSlotUI.moveSO);
        InitMoveEquipSlotList();
        SetMoveArrayEquipMode(true);
        moveEquipArrayUI[highlightedButtonIndex].button.Select();
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitMenu();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            UnassignSlot(moveEquipArrayUI[highlightedButtonIndex]);
        }
    }
}
