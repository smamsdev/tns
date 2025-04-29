using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class State : MonoBehaviour
{
    public CombatManager combatManager;
    public Button buttonSelected;

    public virtual IEnumerator StartState()
    {
        yield break;
    }

    public virtual void StateUpdate()
    {
        
    }

    public virtual void CombatOptionSelected(int Value)
    { 
    
    }

    public void ButtonSelected(Button button)
    {
        buttonSelected = button;
        var combatMenuManager = combatManager.combatMenuManager;
        combatMenuManager.SetButtonNormalColor(button, combatMenuManager.buttonSelectedYellow);
    }
}
