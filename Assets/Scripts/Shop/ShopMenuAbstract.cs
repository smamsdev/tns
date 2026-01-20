using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public abstract class ShopMenu : MonoBehaviour
{
    public GameObject displayContainer;
    public ShopMenuManager shopMenuManager;

    public abstract void EnterMenu();
    public abstract void ExitMenu();
    public abstract void StateUpdate();
    public abstract void DisplayMenu(bool on);
}

public abstract class ChargingMenu : MonoBehaviour
{
    public GameObject displayContainer;
    public ChargingMenuManager chargingMenuManager;

    public abstract void EnterMenu();
    public abstract void ExitMenu();
    public abstract void StateUpdate();
    public abstract void DisplayMenu(bool on);
}


