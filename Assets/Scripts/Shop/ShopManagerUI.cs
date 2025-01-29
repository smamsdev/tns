using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopManagerUI : MonoBehaviour
{
    public Menu buy, sell, exit;
    [Header("")]
    public Menu menuUpdateMethod;

    private void Start()
    {
        menuUpdateMethod = buy;
    }

    public void DisplayMenu(Menu menuScript)
    {
        buy.DisplayMenu(false);
        sell.DisplayMenu(false);
        exit.DisplayMenu(false);
        menuScript.DisplayMenu(true);
    }

    public void EnterSubMenu(Menu menuScript)
    {
        menuScript.EnterMenu();
        menuUpdateMethod = menuScript;
    }

    void Update()
    {
        StateUpdate(menuUpdateMethod);
    }

    void StateUpdate(Menu menuUpdateMethod)
    {
        menuUpdateMethod.StateUpdate();
    }
}
