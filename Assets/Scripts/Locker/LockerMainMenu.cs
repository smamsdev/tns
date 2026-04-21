using TMPro;using UnityEngine;public class LockerMainMenu : LockerMenu{    public InventorySO lockerInventorySO;
    public PlayerInventorySO playerInventorySO;    public MenuButtonHighlighted[] mainMenuButtons;    public TextMeshProUGUI headerTMP;
    public Animator animator;    public void InitializeMenu()    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerInventorySO = player.GetComponent<PlayerCombat>().playerInventorySO;

        displayContainer.SetActive(true);
        this.gameObject.SetActive(true);
        animator.Play("OpenMenu");

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
    public override void DisplayMenu(bool on)    {        displayContainer.SetActive(on);    }    public override void EnterMenu()    {        //    }    public override void ExitMenu()    {        animator.Play("CloseMenu", 0, 0f);
        FieldEvents.menuAvailable = true;
        CombatEvents.UnlockPlayerMovement();
        //Manager GO will be disabled via attached MenuAnimationFunctions script event once completed
    }    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ExitMenu();
    }}