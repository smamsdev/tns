using TMPro;using UnityEngine;public class LockerMainMenu : LockerMenu{    public InventorySO lockerInventorySO;
    public PlayerInventorySO playerInventorySO;    public MenuButtonHighlighted[] mainMenuButtons;    public TextMeshProUGUI headerTMP, cacheNameTMP, chargeTMP, gearDescriptionTMP, gearValueTMP, gearEquipStatusTMP;
    public Animator animator;    public void InitializeMenu()    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerInventorySO = player.GetComponent<PlayerCombat>().playerInventorySO;

        displayContainer.SetActive(true);
        this.gameObject.SetActive(true);
        animator.Play("OpenMenu");

        FieldEvents.menuAvailable = false;
        CombatEvents.LockPlayerMovement();        ClearAllDescriptionTMPs();        lockerMenuManager.lockerCacheMenu.InstantiateUIBays();
        lockerMenuManager.lockerCacheMenu.SetBaySlotsAlpha(.7f, .7f);
        lockerMenuManager.lockerGearMenu.InitialiseInventoryUI();
        lockerMenuManager.lockerGearMenu.SetAllGearSlotsAlpha(.5f, .5f);
        mainMenuButtons[0].button.Select();
    }

    public void SetHeaderTMP(string text)
    { 
        headerTMP.text = text;
    }

    public void UpdateDescriptionDisplayTMPs(GearInstance gearInstance)
    {
        gearDescriptionTMP.text = "Description: " + gearInstance.gearSO.GearDescription;
        gearValueTMP.text = "Sell Value: " + gearInstance.gearSO.Value.ToString("N0") + " $MAMS";

        //Gear Type
        if (gearInstance is EquipmentInstance equipmentInstance)
        {
            chargeTMP.text = "Charge " + equipmentInstance.Charge + " / " + ((EquipmentSO)equipmentInstance.gearSO).maxPotential;
        }

        else
            chargeTMP.text = "";

        //Availability
        if (gearInstance.isCurrentlyEquipped)
        {
            var inventorySO = lockerMenuManager.lockerMainMenu.playerInventorySO;
            gearEquipStatusTMP.text = "Equipped to Slot " + (inventorySO.gearInstanceEquipped.IndexOf(gearInstance) + 1) + ". Press CTRL to unequip";
        }

        else
        {
            gearEquipStatusTMP.text = "";
        }
    }

    public void ClearAllDescriptionTMPs()
    {
        gearDescriptionTMP.text = "";        gearValueTMP.text = "";
        chargeTMP.text = "";        gearEquipStatusTMP.text = "";
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