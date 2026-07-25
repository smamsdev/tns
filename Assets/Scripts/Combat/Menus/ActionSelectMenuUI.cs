using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;
using TMPro;
using System.Collections.Generic;


public class ActionSelectMenuUI : CombatMenu
{
    public ActionSelectState actionSelectState;
    [SerializeField] List<MenuButtonHighlighted> menuButtonHighlighteds = new();
    public MoveSO[][] equippedArrays = new MoveSO[3][];
    string[] moveTypeName = new string[3];

    public override void DisplayMenu(bool on)
    {
        this.gameObject.SetActive(on);

        if (!on) return;

        SetButtonNormalColor(menuButtons[highlightedButtonIndex], Color.white);
        InitMenu();
        menuButtons[highlightedButtonIndex].Select();
    }

    void AssignViolentMoves(PlayerMoveInventorySO playerMoveInventorySO)
    {
        equippedArrays[0] = playerMoveInventorySO.violentAttacksEquipped;
        moveTypeName[0] = "Violent Attacks";
        equippedArrays[1] = playerMoveInventorySO.violentFendsEquipped;
        moveTypeName[1] = "Violent Fends";
        equippedArrays[2] = playerMoveInventorySO.violentFocusesEquipped;
        moveTypeName[2] = "Violent Focuses";
    }

    void AssignCautiousMoves(PlayerMoveInventorySO playerMoveInventorySO)
    {
        equippedArrays[0] = playerMoveInventorySO.cautiousAttacksEquipped;
        moveTypeName[0] = "Cautious Attacks";
        equippedArrays[1] = playerMoveInventorySO.cautiousFendsEquipped;
        moveTypeName[0] = "Cautious Fends";
        equippedArrays[2] = playerMoveInventorySO.cautiousFocusesEquipped;
        moveTypeName[0] = "Cautious Focuses";
    }

    void AssignPreciseMoves(PlayerMoveInventorySO playerMoveInventorySO)
    {
        equippedArrays[0] = playerMoveInventorySO.preciseAttacksEquipped;
        moveTypeName[0] = "Precise Attacks";
        equippedArrays[1] = playerMoveInventorySO.preciseFendsEquipped;
        moveTypeName[0] = "Precise Fends";
        equippedArrays[2] = playerMoveInventorySO.preciseFocusesEquipped;
        moveTypeName[0] = "Precise Focuses";
    }

    void AssignEquipArrays()
    {
        PlayerMoveInventorySO playerMoveInventorySO = menuManager.combatManager.playerCombat.playerMoveInventorySO;

        if (menuManager.styleSelectMenuUI.highlightedButtonIndex == 0)
            AssignViolentMoves(playerMoveInventorySO);
        else if (menuManager.styleSelectMenuUI.highlightedButtonIndex == 1)
            AssignCautiousMoves(playerMoveInventorySO);
        else
            AssignPreciseMoves(playerMoveInventorySO);
    }

    bool IsContainsMoves(MoveSO[] array)
    {
        foreach (MoveSO moveSO in array)
        {
            if (moveSO != null)
                return true;
        }

        return false;
    }

    public void InitMenu()
    {
        AssignEquipArrays();

        for (int i = 0; i < menuButtonHighlighteds.Count; i++)
        {
            int index = i;

            if (IsContainsMoves(equippedArrays[index]))
            {
                FieldEvents.SetTextColor(menuButtonHighlighteds[index].tmp, menuButtonHighlighteds[index].tmp.color, 1);
                menuButtonHighlighteds[index].onHighlighted = () => MenuOptionHighlighted(menuButtonHighlighteds[index].button);
                menuButtonHighlighteds[index].button.onClick.AddListener(() => ActionButtonSelected(index));
            }

            else
            {
                FieldEvents.SetTextColor(menuButtonHighlighteds[index].tmp, menuButtonHighlighteds[index].tmp.color, 0.7f);

                menuButtonHighlighteds[index].onHighlighted = () =>
                {
                    menuManager.UpdateNarrator("No " + moveTypeName[index] + " available");
                    highlightedButtonIndex = menuButtons.IndexOf(menuButtonHighlighteds[index].button);
                };
            }
        }
    }

    public void ActionButtonSelected(int moveValue)
    {
        SetButtonNormalColor(menuButtons[highlightedButtonIndex], Color.yellow);
        actionSelectState.ActionButtonSelected(moveValue);
    }

    void MenuOptionHighlighted(Button button)
    {
        highlightedButtonIndex = menuButtons.IndexOf(button);
        UpdateNarrator(highlightedButtonIndex);
    }

    void UpdateNarrator(int index)
    {
        string text;

        if (menuManager.styleSelectMenuUI.highlightedButtonIndex == 0)
        {
            switch (index)
            {
                case 0:
                    text = "Execute a Violent Attack?";
                    break;
                case 1:
                    text = "Execute a Violent Fend?";
                    break;
                case 2:
                    text = "Execute a Violent Focus?";
                    break;
                default:
                    Debug.Log("something went wrong");
                    text = null;
                    break;
            }
        }

        else if (menuManager.styleSelectMenuUI.highlightedButtonIndex == 1)
        {
            switch (index)
            {
                case 0:
                    text = "Execute a Cautious Attack?";
                    break;
                case 1:
                    text = "Execute a Cautious Fend?";
                    break;
                case 2:
                    text = "Execute a Cautious Focus?";
                    break;
                default:
                    Debug.Log("something went wrong");
                    text = null;
                    break;
            }
        }

        else
        {
            switch (index)
            {
                case 0:
                    text = "Execute a Precise Attack?";
                    break;
                case 1:
                    text = "Execute a Precise Fend?";
                    break;
                case 2:
                    text = "Execute a Precise Focus?";
                    break;
                default:
                    Debug.Log("something went wrong");
                    text = null;
                    break;
            }
        }

        menuManager.UpdateNarrator(text);
    }
}
