using NUnit.Framework;using System.Collections.Generic;using TMPro;using UnityEngine;public class DropMainMenu : DropMenu{
    public PlayerInventorySO playerInventorySO;
    public InventorySO dropInventory;
    public List<GearSO> testList = new();    public MenuButtonHighlighted[] mainMenuButtons;    public TextMeshProUGUI headerTMP, chargeTMP, gearDescriptionTMP, gearValueTMP, gearEquipStatusTMP;
    public Animator animator;    public void InitializeMenu()    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerInventorySO = player.GetComponent<PlayerCombat>().playerInventorySO;

        InstantiateDropListSOs(testList);

        displayContainer.SetActive(true);
        this.gameObject.SetActive(true);
        animator.Play("OpenMenu");
        CombatEvents.LockPlayerMovement();        ClearAllDescriptionTMPs();        dropMenuManager.dropSeizedMenu.InstantiateUIBays();
        dropMenuManager.dropSeizedMenu.SetBaySlotsAlpha(.7f, .7f);
        dropMenuManager.dropGearMenu.InitialiseInventoryUI();
        dropMenuManager.dropGearMenu.SetAllGearSlotsAlpha(.7f, .7f);
        mainMenuButtons[0].button.Select();
    }

    void InstantiateDropListSOs(List<GearSO> dropSOList)
    {
        dropInventory.gearInstanceInventory.Clear();
        dropSOList.ShuffleList();

        //5 max slots
        for (int i = 0; i < 5; i++)
        {
            var emptyInstance = new GearInstance();
            dropInventory.gearInstanceInventory.Add(emptyInstance);
        }

        //4 max drops
        for (int i = 0; i < 4; i++)
        {
            var gearInstance = dropSOList[i].CreateInstance();

            if (gearInstance is EquipmentInstance equipmentInstance)
            {
                int randomValue = Random.Range(0, equipmentInstance.MaxPotential() / 2);
                equipmentInstance.SetCharge(randomValue);
            }

            if (!dropInventory.AttemptAddGearToInventory(gearInstance, true))
                return;
        }

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
        CombatEvents.UnlockPlayerMovement();
        //Manager GO will be disabled via attached MenuAnimationFunctions script event once completed
    }    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ExitMenu();
    }}