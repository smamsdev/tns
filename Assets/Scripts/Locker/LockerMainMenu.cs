using UnityEngine;public class LockerMainMenu : LockerMenu{    public InventorySO lockerInventorySO;
    public PlayerInventory playerInventory;    public MenuButtonHighlighted[] mainMenuButtons;    public void InitializeMenu()    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerInventory = player.GetComponentInChildren<PlayerInventory>();

        FieldEvents.menuAvailable = false;
        CombatEvents.LockPlayerMovement();        lockerMenuManager.lockerBayMenu.InstantiateUIBays();
        lockerMenuManager.lockerGearMenu.InitialiseInventoryUI();

        mainMenuButtons[0].button.Select();
    }

    void WireMainButtons()
    {
        mainMenuButtons[0].onHighlighted = () =>
        {
            lockerMenuManager.DisplaySubMenu(lockerMenuManager.lockerGearMenu);
        };

        mainMenuButtons[1].onHighlighted = () =>
        {
            lockerMenuManager.DisplaySubMenu(lockerMenuManager.lockerBayMenu);
        };
    }    public override void DisplayMenu(bool on)    {        displayContainer.SetActive(on);    }    public override void EnterMenu()    {        //    }    public override void ExitMenu()    {        throw new System.NotImplementedException();    }    public override void StateUpdate()    {//    }}