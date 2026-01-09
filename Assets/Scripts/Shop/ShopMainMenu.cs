using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShopMainMenu : ShopMenu
{
    [Header("")]
    [HideInInspector] public PlayerPermanentStats playerPermanentStats;
    [HideInInspector] public PlayerInventory playerInventory;
    public TextMeshProUGUI smamsValue;
    public Button firstMenuButton;

    public bool isMenuOn;

    public override void DisplayMenu(bool on)
    {
        if (!isMenuOn)
        {
            isMenuOn = true;
            displayContainer.SetActive(true);
            smamsValue.text = $"{playerPermanentStats.Smams}";
            shopMenuManagerUI.GearDescriptionGO.SetActive(false);
            firstMenuButton.Select(); //Ihandler uses this to trigger DisplayMenu method 
            //menuManagerUI.DisplayMenu(menuManagerUI.buy);

            CombatEvents.LockPlayerMovement();
            return;
        }

        if (isMenuOn)
        {
            CombatEvents.UnlockPlayerMovement();
            displayContainer.SetActive(false);
            isMenuOn = false;
        }
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

        {
            DisplayMenu(true);
        }
    }
}
