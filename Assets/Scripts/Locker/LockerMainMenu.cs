using TMPro;using UnityEngine;public class LockerMainMenu : LockerMenu{    public InventorySO lockerInventorySO;
    public PlayerInventory playerInventory;    public MenuButtonHighlighted[] mainMenuButtons;    public TextMeshProUGUI headerTMP;    public void InitializeMenu()    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerInventory = player.GetComponentInChildren<PlayerInventory>();

        FieldEvents.menuAvailable = false;
        CombatEvents.LockPlayerMovement();        lockerMenuManager.lockerCacheMenu.InstantiateUIBays();
        lockerMenuManager.lockerCacheMenu.SetBaySlotsAlpha(.7f, .7f);
        lockerMenuManager.lockerGearMenu.InitialiseInventoryUI();

        foreach (InventorySlotUI inventorySlotUI in lockerMenuManager.lockerGearMenu.inventorySlots)
        {
            lockerMenuManager.lockerGearMenu.SetGearSlotsAlpha(inventorySlotUI, 1);
        }

        mainMenuButtons[0].button.Select();
    }

    public void SetHeaderTMP(string text)
    { 
        headerTMP.text = text;
    }

    public void DisplayMainButtons(bool on)
    {
        foreach (MenuButtonHighlighted menuButtonHighlighted in mainMenuButtons)
        { 
            menuButtonHighlighted.gameObject.SetActive(on);
        }
    }

    void WireMainButtons()
    {
        mainMenuButtons[0].onHighlighted = () =>
        {
            lockerMenuManager.DisplaySubMenu(lockerMenuManager.lockerGearMenu);
        };

        mainMenuButtons[1].onHighlighted = () =>
        {
            lockerMenuManager.DisplaySubMenu(lockerMenuManager.lockerCacheMenu);
        };
    }    public override void DisplayMenu(bool on)    {        displayContainer.SetActive(on);    }    public override void EnterMenu()    {        //    }    public override void ExitMenu()    {        throw new System.NotImplementedException();    }    public override void StateUpdate()    {//    }}