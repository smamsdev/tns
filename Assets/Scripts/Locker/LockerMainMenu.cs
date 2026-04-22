using TMPro;using UnityEngine;public class LockerMainMenu : LockerMenu{    public InventorySO lockerInventorySO;
    public PlayerInventorySO playerInventorySO;    public MenuButtonHighlighted[] mainMenuButtons;    public TextMeshProUGUI headerTMP,chargeTMP, gearDescriptionTMP, gearValueTMP, gearEquipStatusTMP;
    public Animator animator;    public void InitializeMenu()    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerInventorySO = player.GetComponent<PlayerCombat>().playerInventorySO;

        displayContainer.SetActive(true);
        this.gameObject.SetActive(true);
        animator.Play("OpenMenu");

        FieldEvents.menuAvailable = false;
        CombatEvents.LockPlayerMovement();        gearDescriptionTMP.text = "";        gearValueTMP.text = "";
        chargeTMP.text = "";        gearEquipStatusTMP.text = "";        lockerMenuManager.lockerCacheMenu.InstantiateUIBays();
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
        gearDescriptionTMP.text = "Description: " + gearInstance.gearSO.gearDescription;
        gearValueTMP.text = "Sell Value: " + gearInstance.gearSO.value.ToString("N0") + " $MAMS";

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
            gearEquipStatusTMP.text = "Equipped to Slot " + (inventorySO.gearInstanceEquipped.IndexOf(gearInstance) + 1) + ". PRESS CTRL TO REMOVE";
        }

        else
        {
            gearEquipStatusTMP.text = "Select to CACHE";
        }
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