using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TrenchConstructState : TrenchMenuState
{
    public TrenchStructure structureToConstruct;
    public List<Button> structureSlotButtons;
    public GameObject structuresGO;
    public Sprite emptyStructureSprite;
    public int buttonSelectedIndex = 0;

    public override void EnterState()
    {
        InitializeConstructUI(trenchManager.currentBase());
        ShowEmptySlots(trenchManager.currentBase(), true);
        structureSlotButtons[buttonSelectedIndex].Select();
    }

    public void InitializeConstructUI(TrenchStructureIcon[] structures)
    {
        foreach (TrenchStructureIcon trenchStructureIcon in structures)
        {
            trenchStructureIcon.menuButtonHighlighted.onHighlighted = () =>
            {
                SlotHighlighted(trenchStructureIcon);
            };

            trenchStructureIcon.menuButtonHighlighted.onUnHighlighted = () =>
            {
                SlotUnHighlighted(trenchStructureIcon);
            };

            structureSlotButtons.Add(trenchStructureIcon.menuButtonHighlighted.button);

            trenchStructureIcon.menuButtonHighlighted.button.onClick.AddListener(() => SlotSelected(trenchStructureIcon));
        }

    FieldEvents.SetGridNavigationWrapAroundHorizontal(structureSlotButtons, 4);
    }

    public void ShowEmptySlots(TrenchStructureIcon[] structures, bool on)
    {
        if (on)
        {
            foreach (TrenchStructureIcon trenchStructureIcon in structures)
                if (trenchStructureIcon.trenchStructure == null)
                    trenchStructureIcon.spriteRenderer.enabled = true;
            return;
        }

        foreach (TrenchStructureIcon trenchStructureIcon in structures)
            if (trenchStructureIcon.trenchStructure == null)
                trenchStructureIcon.spriteRenderer.enabled = false;
    }

    void SlotSelected(TrenchStructureIcon trenchStructureIcon)
    {
        if (trenchStructureIcon.trenchStructure != null)
            return;

        trenchStructureIcon.trenchStructure = structureToConstruct;
        trenchStructureIcon.spriteRenderer.sprite = trenchStructureIcon.trenchStructure.structureSprite;
        ShowEmptySlots(trenchManager.currentBase(), false);
        menuManager.mainState.WireButtons();
        menuManager.ChangeState(menuManager.mainState);
    }

    void SlotHighlighted(TrenchStructureIcon trenchStructureIcon)
    {
        trenchStructureIcon.animator.enabled = true;
        trenchStructureIcon.spriteRenderer.sprite = structureToConstruct.structureSprite;
        buttonSelectedIndex = Array.IndexOf(trenchManager.currentBase(), trenchStructureIcon);
    }

    void SlotUnHighlighted(TrenchStructureIcon trenchStructureIcon)
    {
        trenchStructureIcon.animator.enabled = false;
        trenchStructureIcon.spriteRenderer.color = Color.white;

        if (trenchStructureIcon.trenchStructure != null)
        {
            trenchStructureIcon.spriteRenderer.sprite = trenchStructureIcon.trenchStructure.structureSprite;
            return;
        }

        trenchStructureIcon.spriteRenderer.sprite = emptyStructureSprite;
    }

    public override void ExitState()
    {
        ShowEmptySlots(trenchManager.currentBase(), false);
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
