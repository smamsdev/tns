using NUnit.Framework;using System.Collections.Generic;using TMPro;using UnityEngine;public class DropMainMenu : DropMenu{
    public PlayerInventorySO playerInventorySO;
    public List<GearSO> remainingDropList = new();
    public InventorySO dropManagerInventorySO;    public MenuButtonHighlighted[] mainMenuButtons;    public TextMeshProUGUI headerTMP, chargeTMP, gearDescriptionTMP, gearValueTMP, gearEquipStatusTMP;
    public DropMenuState dropMenuState;
    public Animator animator;    public void InitializeMenu()    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerInventorySO = player.GetComponent<PlayerCombat>().playerInventorySO;
        displayContainer.SetActive(true);
        this.gameObject.SetActive(true);
        animator.Play("OpenMenu");
        CombatEvents.LockPlayerMovement();        ClearAllDescriptionTMPs();        dropMenuManager.dropSelectMenu.InitDropUI();
        dropMenuManager.dropSelectMenu.SetBaySlotsAlpha(.7f, .7f);
        dropMenuManager.dropGearMenu.InitialiseInventoryUI();
        dropMenuManager.dropGearMenu.SetAllGearSlotsAlpha(.7f, .7f);
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
            chargeTMP.text = "Charge " + equipmentInstance.Charge + " / " + ((EquipmentSO)equipmentInstance.gearSO).MaxCharge;
        }

        else
            chargeTMP.text = "";

        //Availability
        if (gearInstance.isCurrentlyEquipped)
        {
            var inventorySO = dropMenuManager.dropMainMenu.playerInventorySO;
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
        dropMenuState.ExitState();
        //Manager GO will be disabled via attached MenuAnimationFunctions script event once completed
    }    public override void StateUpdate()
    {
       if (Input.GetKeyDown(KeyCode.Escape))
           ExitMenu();
    }}