using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopMainMenu : ShopMenu
{
    public override void DisplayMenu(bool on)
    {
        throw new System.NotImplementedException();
    }

    public override void EnterMenu()
    {
        throw new System.NotImplementedException();
    }

    public override void ExitMenu()
    {
        throw new System.NotImplementedException();
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Tab))

            Debug.Log("open shop");

    }
}
