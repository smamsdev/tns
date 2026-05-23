using NUnit.Framework;using System.Collections.Generic;using TMPro;using UnityEngine;public class DropMainMenu : DropMenu{
    public PlayerInventorySO playerInventorySO;
    public List<GearSO> rawDropList = new();
    public InventorySO dropManagerInventorySO;    public MenuButtonHighlighted[] mainMenuButtons;    public TextMeshProUGUI headerTMP, chargeTMP, gearDescriptionTMP, gearValueTMP, gearEquipStatusTMP;
    public Animator animator;    public void InitializeMenu()    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerInventorySO = player.GetComponent<PlayerCombat>().playerInventorySO;

        InstantiateDropListSOs(rawDropList);

        displayContainer.SetActive(true);
        this.gameObject.SetActive(true);
        animator.Play("OpenMenu");
        CombatEvents.LockPlayerMovement();        ClearAllDescriptionTMPs();        dropMenuManager.dropSelectMenu.InitDropUI();
        dropMenuManager.dropSelectMenu.SetBaySlotsAlpha(.7f, .7f);
        dropMenuManager.dropGearMenu.InitialiseInventoryUI();
        dropMenuManager.dropGearMenu.SetAllGearSlotsAlpha(.7f, .7f);
        mainMenuButtons[0].button.Select();
    }

    void InstantiateDropListSOs(List<GearSO> dropSOList)
    {
        dropManagerInventorySO.gearInstanceInventory.Clear();
        dropSOList.ShuffleList();

        //init 5 max empty slots
        for (int i = 0; i < 5; i++)
        {
            var emptyInstance = new GearInstance();
            dropManagerInventorySO.gearInstanceInventory.Add(emptyInstance);
        }

        //drop list should be limited to the first 4 items max
        for (int i = 0; i < Mathf.Min(dropSOList.Count, 4); i++)
        {
            if (dropSOList[i] == null)
                continue;

            var gearInstance = dropSOList[i].CreateInstance();

            if (gearInstance is EquipmentInstance equipmentInstance)
            {
                int randomValue = Random.Range(0, equipmentInstance.MaxPotential() / 2);
                equipmentInstance.SetCharge(randomValue);
            }

            if (!dropManagerInventorySO.AttemptAddGearToInventory(gearInstance, true))
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