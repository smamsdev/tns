using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShopMainMenu : ShopMenu
{
    public PlayerPermanentStats playerPermanentStats;
    public PlayerInventory playerInventory;
    [SerializeField] TextMeshProUGUI smamsValue;

    public Button firstMenuButton;

    public bool isMenuOn;

    private void Start()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        playerInventory = player.GetComponentInChildren<PlayerInventory>();
    }

    public override void DisplayMenu(bool on)
    {
        if (!isMenuOn)
        {
            isMenuOn = true;
            displayContainer.SetActive(true);
            smamsValue.text = $"{playerPermanentStats.smams}";
            firstMenuButton.Select(); //Ihandler uses this to trigger DisplayMenu method 
            menuManagerUI.DisplayMenu(menuManagerUI.buy);

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
