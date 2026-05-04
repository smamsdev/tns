using TMPro;
using UnityEngine;

public class BatteryMainMenu : BatteryMenu
{
    public PlayerInventorySO playerInventorySO;
    public MenuButtonHighlighted[] mainMenuButtons;
    public VehicleInstance vehicleInstance;
    public Animator animator;

    public TextMeshProUGUI headerTMP, vehicleNameTMP, chargeTMP, gearDescriptionTMP, gearValueTMP, gearEquipStatusTMP;

    public void InitializeMenu()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerInventorySO = player.GetComponent<PlayerCombat>().playerInventorySO;
        vehicleInstance = GetComponentInParent<VehicleInstance>();

        FieldEvents.menuAvailable = false;
        CombatEvents.LockPlayerMovement();

        ClearAllDescriptionTMPs();

        batteryMenuManager.batteryEquipMenu.InitialiseBatterySlot();
        batteryMenuManager.batteryEquipMenu.InitialiseInventoryUI();
        //lockerMenuManager.lockerGearMenu.SetAllGearSlotsAlpha(.5f, .5f);
        vehicleNameTMP.text = vehicleInstance.vehicleName;

        displayContainer.SetActive(true);
        this.gameObject.SetActive(true);
        animator.Play("OpenMenu");

        mainMenuButtons[0].button.Select();
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
            gearEquipStatusTMP.text = "Equipped to Slot " + (playerInventorySO.gearInstanceEquipped.IndexOf(gearInstance) + 1) + ". Press CTRL to unequip";
        }

        else
        {
            gearEquipStatusTMP.text = "";
        }
    }

    public void ClearAllDescriptionTMPs()
    {
        gearDescriptionTMP.text = "";
        gearValueTMP.text = "";
        chargeTMP.text = "";
        gearEquipStatusTMP.text = "";
    }

    public override void DisplayMenu(bool on)
    {
        displayContainer.SetActive(on);
    }

    public override void EnterMenu()
    {
        //
    }

    public void DisplayMainButtons(bool on)
    {
        foreach (MenuButtonHighlighted menuButtonHighlighted in mainMenuButtons)
        {
            menuButtonHighlighted.gameObject.SetActive(on);
        }
    }

    public override void ExitMenu()
    {
        animator.Play("CloseMenu", 0, 0f);
        FieldEvents.menuAvailable = true;
        CombatEvents.UnlockPlayerMovement();
        //Manager GO will be disabled via attached MenuAnimationFunctions script event once completed
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ExitMenu();
    }
}
