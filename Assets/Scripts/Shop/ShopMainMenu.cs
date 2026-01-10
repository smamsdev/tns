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
    public TextMeshProUGUI smamsValue;
    public Button firstMenuButton;

    private void OnEnable()
    {
        FieldEvents.isShopping = false;
    }

    public override void DisplayMenu(bool on)
    {
        //
    }

    public void OpenShop()
    {
        CombatEvents.LockPlayerMovement();
        FieldEvents.isShopping = true;
        UpdateSmamsUI();
        EnterMenu();
    }

    public override void EnterMenu()
    {
            shopMenuManagerUI.GearDescriptionGO.SetActive(false);
            firstMenuButton.Select(); 
    }

    public void UpdateSmamsUI()
    {
        smamsValue.text = shopMenuManagerUI.playerPermanentStats.Smams.ToString("N0");
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
