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
        combatManager.playerCombat.playerMoveManager.secondMoveIs = 0;

        yield break;
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            styleSelectMenuUI.DisplayMenu(false);

            combatManager.SetState(combatManager.actionSelectState);
        }
    }

    public void StyleButtonSelected(int moveValue) //triggered via Button
    {
        combatManager.playerCombat.playerMoveManager.secondMoveIs = moveValue;
        combatManager.playerCombat.playerMoveManager.CombineStanceAndMove();

        if (!combatManager.playerCombat.moveSelected.moveSO.ApplyMoveToSelfOnly)
        {
            combatManager.SetState(combatManager.enemySelectState);
        }
        else
        {
            combatManager.playerCombat.targetCombatant = combatManager.playerCombat;
            combatManager.SetState(combatManager.applyMove);
        }
    }
}