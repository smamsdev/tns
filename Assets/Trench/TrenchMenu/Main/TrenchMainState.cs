using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class TrenchMainState : TrenchMenuState
{
    public List<MenuButtonHighlighted> mainButtons;
    public int buttonSelectedIndex = 0;

    public override void EnterState()
    {
        mainButtons[buttonSelectedIndex].button.Select();
    }

    public void WireButtons()
    { 
        List<Button> buttons = new List<Button>();

        foreach (MenuButtonHighlighted menuButtonHighlighted in mainButtons)
        {
            menuButtonHighlighted.onHighlighted = () => ButtonHighlighted(menuButtonHighlighted);
            menuButtonHighlighted.onUnHighlighted = () => ButtonUnHighlighted(menuButtonHighlighted);

            buttons.Add(menuButtonHighlighted.button);
        }

        FieldEvents.SetGridNavigationWrapAround(buttons, 4);
    }

    public void ButtonHighlighted(MenuButtonHighlighted menuButtonHighlighted)
    {
        menuButtonHighlighted.SetButtonNormalColor(Color.yellow);
    }

    public void ButtonUnHighlighted(MenuButtonHighlighted menuButtonHighlighted)
    {
        menuButtonHighlighted.SetButtonNormalColor(Color.white);
    }

    public void ConstructSelected()
    {
        mainButtons[0].ButtonSelectedAndDisabled();
        menuManager.ChangeState(menuManager.inventoryState);
    }

    public override void ExitState()
    {
//
    }

    public override void StateUpdate()
    {
    }
}
