using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public abstract class Menu : MonoBehaviour
{
    public GameObject displayContainer;
    public MenuButtonHighlighted lastParentButtonSelected;

    public abstract void EnterMenu();
    public abstract void ExitMenu();
    public abstract void StateUpdate();
    public abstract void DisplayMenu(bool on);
}//

public abstract class PauseMenu : Menu
{
    public PauseMenuManager pauseMenuManager;
}

public abstract class ShopMenu : Menu
{
    public ShopMenuManager shopMenuManager;
}

public abstract class ChargingMenu : Menu
{
    public ChargingMenuManager chargingMenuManager;
}

public abstract class LockerMenu : Menu
{
    public LockerMenuManager lockerMenuManager;
}
