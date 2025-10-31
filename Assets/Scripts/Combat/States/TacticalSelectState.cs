using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TacticalSelectState : State
{
    public bool isEnclosing = false;
    [SerializeField] EncloseMove encloseMove;
    [SerializeField] ReturnMove returnMove;
    public GameObject encloseOption, returnOption;
    public Button gearButton, retreatButton, encloseButton, returnButton;

    public Vector3 playerDefaultFightingPosition;

    public override IEnumerator StartState()
    {
        CheckEncloseState();

        combatManager.combatMenuManager.DisplayMenuGO(combatManager.combatMenuManager.TacticalSelectMenu, true);
        combatManager.tacticalSelectState.lastButtonSelected.Select();

        yield break;
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            combatManager.combatMenuManager.DisplayMenuGO(combatManager.combatMenuManager.TacticalSelectMenu, false);
            combatManager.combatMenuManager.SetButtonNormalColor(combatManager.firstMove.lastButtonSelected, Color.white);
            combatManager.firstMove.lastButtonSelected.Select();
            combatManager.SetState(combatManager.firstMove);
        }
    }

    public void TacticalOptionSelected(State state)
    {
        combatManager.combatMenuManager.tacticalSelectMenuDefaultButton = lastButtonSelected;
        combatManager.SetState(state);
    }

    public void EncloseSelected()
    {
        playerDefaultFightingPosition = combatManager.playerCombat.fightingPosition.transform.position;
        isEnclosing = true;
        combatManager.playerCombat.moveSelected = encloseMove;
    }

    public void ReturnSelected()
    {
        combatManager.playerCombat.fightingPosition.transform.position = playerDefaultFightingPosition;
        isEnclosing = false;
        combatManager.playerCombat.moveSelected = returnMove;
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
