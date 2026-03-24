using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static TrenchManager;

public class TrenchConstructState : TrenchMenuState
{
    public TrenchStructureSO structureSOToConstruct; //selected by InventoryState button
    public List<Button> structureSlotButtons;
    public GameObject structuresGO;
    public Sprite emptyStructureSprite;
    public int buttonSelectedIndex = 0;
    TrenchManager.Team team;
    //int frontLineIndex;

    public override void EnterState()
    {
        team = TrenchManager.Team.Left;
        //frontLineIndex = 0;
        InitializeConstructUI(trenchManager.GetBaseStructureList(TrenchManager.Team.Left, 0));
        ShowEmptySlots(trenchManager.GetBaseStructureList(TrenchManager.Team.Left, 0), true);

        structureSlotButtons[buttonSelectedIndex].Select();
    }

    public void InitializeConstructUI(TrenchStructureInstance[] structures)
    {
        foreach (TrenchStructureInstance trenchStructureInstance in structures)
        {
            trenchStructureInstance.menuButtonHighlighted.onHighlighted = () =>
            {
                SlotHighlighted(trenchStructureInstance);
            };

            trenchStructureInstance.menuButtonHighlighted.onUnHighlighted = () =>
            {
                SlotUnHighlighted(trenchStructureInstance);
            };

            structureSlotButtons.Add(trenchStructureInstance.menuButtonHighlighted.button);

            trenchStructureInstance.menuButtonHighlighted.button.onClick.AddListener(() => SlotSelected(trenchStructureInstance));
        }

        if (team == TrenchManager.Team.Left)
            FieldEvents.SetGridNavigationWrapAroundMirrored(structureSlotButtons, 4);

        else
            FieldEvents.SetGridNavigationWrapAround(structureSlotButtons, 4);
    }

    public void ShowEmptySlots(TrenchStructureInstance[] structures, bool on)
    {
        if (on)
        {
            foreach (TrenchStructureInstance trenchStructureIcon in structures)
                if (trenchStructureIcon.structureSO == null)
                    trenchStructureIcon.spriteRenderer.enabled = true;
            return;
        }

        foreach (TrenchStructureInstance trenchStructureIcon in structures)
            if (trenchStructureIcon.structureSO == null)
                trenchStructureIcon.spriteRenderer.enabled = false;
    }

    void SlotSelected(TrenchStructureInstance emptyInstanceSelected)
    {
        if (emptyInstanceSelected.structureSO != null)
            return;

        trenchManager.ConstructStructure(TrenchManager.Team.Left, 0, emptyInstanceSelected, structureSOToConstruct);

        ShowEmptySlots(trenchManager.GetBaseStructureList(TrenchManager.Team.Left, 0), false);
        menuManager.mainState.WireButtons();
        menuManager.ChangeState(menuManager.mainState);
    }

    void SlotHighlighted(TrenchStructureInstance structureInstance)
    {
        structureInstance.animator.enabled = true;
        buttonSelectedIndex = Array.IndexOf(trenchManager.GetBaseStructureList(TrenchManager.Team.Left, 0), structureInstance);

        //if slot if empty, show preview
        if (structureInstance.structureSO == null)
            structureInstance.spriteRenderer.sprite = structureSOToConstruct.StructureSprite;
    }

    void SlotUnHighlighted(TrenchStructureInstance trenchStructureInstance)
    {
        trenchStructureInstance.animator.enabled = false;
        trenchStructureInstance.spriteRenderer.color = Color.white;

        if (trenchStructureInstance.structureSO != null)
        {
            return;
        }

        //if slot is still empty show the empty space again
        trenchStructureInstance.spriteRenderer.sprite = emptyStructureSprite;
    }

    public override void ExitState()
    {
        ShowEmptySlots(trenchManager.GetBaseStructureList(TrenchManager.Team.Left, 0), false);
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitState();
            menuManager.ChangeState(menuManager.inventoryState);
        }
    }
}
