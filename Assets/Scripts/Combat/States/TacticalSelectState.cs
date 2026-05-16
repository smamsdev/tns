using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TacticalSelectState : State
{
    public TacticalSelectMenuUI tacticalSelectMenuUI;
    public bool isEnclosing = false;
    [SerializeField] MoveSO encloseMoveSO;
    [SerializeField] MoveSO returnMoveSO;
    public GameObject encloseOption, returnOption;
    public Button gearButton, retreatButton, encloseButton, returnButton;

    public Vector3 playerDefaultFightingPosition;

    public override IEnumerator StartState()
    {
        CheckEncloseState();
        tacticalSelectMenuUI.DisplayMenu(true);
        tacticalSelectMenuUI.SetButtonNormalColor(tacticalSelectMenuUI.menuButtons[tacticalSelectMenuUI.highlightedButtonIndex], Color.white);
        tacticalSelectMenuUI.menuButtons[tacticalSelectMenuUI.highlightedButtonIndex].Select();

        yield break;
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            tacticalSelectMenuUI.DisplayMenu(false);
            combatManager.SetState(combatManager.styleSelectState);
        }
    }

    public void TacticalOptionSelected(State state)
    {
        combatManager.SetState(state);
    }

    public void EncloseSelected()
    {
        var playerCombat = combatManager.playerCombat;

        playerDefaultFightingPosition = playerCombat.fightingPosition.transform.position;
        isEnclosing = true;

        playerCombat.moveSOSelected = encloseMoveSO;
        playerCombat.InstantiateMoveBehaviour(playerCombat.moveSOSelected);
        playerCombat.currentMoveBehaviour.LoadMoveReferences(playerCombat, combatManager);
        playerCombat.currentMoveBehaviour.CalculateMoveStats();


        combatManager.SetState(combatManager.applyMove);
    }

    public void ReturnSelected()
    {
        var playerCombat = combatManager.playerCombat;

        playerCombat.fightingPosition.transform.position = playerDefaultFightingPosition;
        isEnclosing = false;

        playerCombat.moveSOSelected = returnMoveSO;
        playerCombat.InstantiateMoveBehaviour(playerCombat.moveSOSelected);
        playerCombat.currentMoveBehaviour.LoadMoveReferences(playerCombat, combatManager);
        playerCombat.currentMoveBehaviour.CalculateMoveStats();

        combatManager.SetState(combatManager.applyMove);
    }

    void CheckEncloseState()
    {
        var buttons = new List<Button>();
        buttons.Add(gearButton);
        buttons.Add(retreatButton);

        returnOption.SetActive(false);
        encloseOption.SetActive(false);

        if (isEnclosing)
        {
            returnOption.SetActive(true);
            buttons.Add(returnButton);
        }

        else
        {
            encloseOption.SetActive(true);
            buttons.Add(encloseButton);
        }

       FieldEvents.SetGridNavigationWrapAround(buttons, 4);
    }
}
