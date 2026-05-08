using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using TMPro;
using System;
using static PlayerMoveInventorySO;
using static UnityEditor.Progress;
using NUnit.Framework.Interfaces;

public class MenuMoveEquipSlotSelect : PauseMenu
{
    public Color colourForSelectedParent;
    public MoveSlotUI MoveSlotUIToRemove;
    public MenuMoves menuMoves;
    public MenuMoveInventory menuMoveInventory;
    public MoveSlotUI[] moveEquipArrayUI = new MoveSlotUI[5];
    public int highlightedButtonIndex = 0;
    public MoveType moveType;

    public override void DisplayMenu(bool on)
    {
        throw new System.NotImplementedException();
    }

    public override void EnterMenu()
    {
        displayContainer.SetActive(true);
        moveEquipArrayUI[highlightedButtonIndex].button.Select();
        SetMoveArrayAlpha(1);
    }

    public override void ExitMenu()
    {
        FieldEvents.SetTextColor(menuMoves.allMenuButtonHighlighteds[menuMoves.highlightedButtonIndex].tmp, Color.yellow, 1);
        pauseMenuManager.EnterMenu(menuMoves);
        SetMoveArrayAlpha(.7f);
    }

    public void SetMoveArrayAlpha(float alpha)
    { 
        foreach (MoveSlotUI moveSlotUI in moveEquipArrayUI)
            FieldEvents.SetTextColor(moveSlotUI.slotText, moveSlotUI.slotText.color, alpha);
    }

   public void InitMoveEquipSlotList()
   {
       MoveSO[] equippedMoveArray = menuMoves.playerMoveManager.playerMoveInventorySO.GetEquippedArrayOfType(moveType);
   
       for (int i = 0; i < moveEquipArrayUI.Length; i++)
       {
           MoveSlotUI moveSlotUI = moveEquipArrayUI[i];
   
           if (i < equippedMoveArray.Length && equippedMoveArray[i] != null)
           {
               moveSlotUI.moveSO = equippedMoveArray[i];
               moveSlotUI.moveSO.isEquipped = true;
               moveSlotUI.slotText.text = $"Slot {i + 1}: {equippedMoveArray[i].MoveName}";
               moveSlotUI.gameObject.name = $"Slot {i + 1}: {equippedMoveArray[i].MoveName}";
           }
   
           else
           {
               moveSlotUI.slotText.text = $"Slot {i + 1}: Empty";
               moveSlotUI.gameObject.name = $"Slot {i + 1}: Empty";
           }

            moveSlotUI.onHighlighted = () => MoveSlotHighlighted(moveSlotUI);
            moveSlotUI.button.onClick.AddListener(() => MoveSlotSelected(moveSlotUI));

           FieldEvents.SetTextColor(moveSlotUI.slotText, Color.white, .7f);
       }
   }

    void MoveSlotHighlighted(MoveSlotUI moveSlotUI)
    {
        highlightedButtonIndex = System.Array.IndexOf(moveEquipArrayUI, moveSlotUI);
        UpdateMoveDescriptions(moveSlotUI);
    }

    public void MoveSlotSelected(MoveSlotUI moveSlotToEquipTo)
    {
        if (moveSlotToEquipTo.moveSO == null || !moveSlotToEquipTo.moveSO.IsFlaw)
        {
            menuMoveInventory.menuMoveEquipSlotSelectInPlay = this;
            menuMoveInventory.moveList = menuMoves.playerMoveManager.playerMoveInventorySO.GetMoveListOfType(moveType);

            if (menuMoveInventory.moveList.Count == 0)
            {
                menuMoves.ClearAllDescriptionTMPs();
                menuMoves.moveNameTMP.text = "No moves available to assign";
                return;
            }

            pauseMenuManager.EnterMenu(menuMoveInventory);
        }
    }

    public void UpdateMoveDescriptions(MoveSlotUI moveSlotUI)
    {
        if (moveSlotUI.moveSO == null)
        {
            menuMoves.moveNameTMP.text = "Slot free";
            menuMoves.moveDescriptionTMP.text = "Assign a " + GetEquippedArrayStringOfType(moveType) + " to slot " + (highlightedButtonIndex + 1) + "?";
            menuMoves.movePotentialChangeTMP.text = "";
            menuMoves.moveEquipStatusTMP.text = "";
            return;
        }

        menuMoves.moveNameTMP.text = moveSlotUI.moveSO.MoveName;
        menuMoves.probabilityTMP.text = moveSlotUI.moveSO.GetRarityDescription();
        menuMoves.moveDescriptionTMP.text = moveSlotUI.moveSO.MoveDescription;
        menuMoves.movePotentialChangeTMP.text = moveSlotUI.moveSO.PotentialChangeDescription;
        //menuMoves.headerTMP.text = "Replace " + moveSlotUI.moveSO.MoveName + " in slot " + (highlightedButtonIndex + 1) + "?";

        if (moveSlotUI.moveSO.IsFlaw)
        { 
            menuMoves.moveEquipStatusTMP.text = "Unable to unassign a FLAW"; 
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

    public void UnassignSlot()
    {
        var movesPage = (MenuMoves)pauseMenuManager.movesPage;

       // MoveSlotUIToRemove = moveSlotHighlighted;

        if (MoveSlotUIToRemove.moveSO == null || MoveSlotUIToRemove.moveSO.IsFlaw)
        {
            return;
        }

        menuMoves.playerMoveManager.playerMoveInventorySO.UnequipMove(MoveSlotUIToRemove.moveSO);
        MoveSlotUIToRemove.moveSO = null;
        movesPage.InitAllEquippedMovesToUISlots();
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
