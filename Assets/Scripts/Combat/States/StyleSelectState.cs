using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StyleSelectState : State
{
    public StyleSelectMenuUI styleSelectMenuUI;

    public override IEnumerator StartState()
    {
        styleSelectMenuUI.DisplayMenu(true);
        styleSelectMenuUI.menuButtons[styleSelectMenuUI.highlightedButtonIndex].Select();
        styleSelectMenuUI.SetButtonNormalColor(styleSelectMenuUI.menuButtons[styleSelectMenuUI.highlightedButtonIndex], Color.white);
        combatManager.playerCombat.styleType = -1;

        yield break;
    }

    public void StyleButtonSelected(int moveValue)
    {
        styleSelectMenuUI.highlightedButtonIndex = moveValue;
        styleSelectMenuUI.SetButtonNormalColor(styleSelectMenuUI.menuButtons[styleSelectMenuUI.highlightedButtonIndex], Color.yellow);
        combatManager.playerCombat.styleType = moveValue;


        if (moveValue == 3)
            combatManager.SetState(combatManager.tacticalSelectState);

        else
            combatManager.SetState(combatManager.actionSelectState);
    }
}