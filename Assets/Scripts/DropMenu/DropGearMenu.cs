using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DropGearMenu : DropMenu
{
    public GameObject inventorySlotUIPrefab, inventorySlotsParent;
    public List<InventorySlotUI> inventorySlots = new();
    public int highlightedButtonIndex = 0;

    public override void DisplayMenu(bool on)
    {
        throw new System.NotImplementedException();
    }

    public override void EnterMenu()
    {
        var inventory = dropMenuManager.dropMainMenu.playerInventorySO.gearInstanceInventory;
        if (inventory.TrueForAll(x => x.gearSO == null))
        {
            dropMenuManager.menuUpdateMethod = dropMenuManager.dropMainMenu;
            return;
        }

        dropMenuManager.dropSelectMenu.HighlightedButtonIndex = 0;
        highlightedButtonIndex = 0;
        dropMenuManager.dropMainMenu.mainMenuButtons[1].SetButtonNormalColor(Color.yellow);
        dropMenuManager.dropSelectMenu.SetBaySlotsAlpha(.5f, .5f);
        SetAllGearSlotsAlpha(1, .5f);
        dropMenuManager.dropMainMenu.DisplayMainButtons(false);
        dropMenuManager.dropMainMenu.SetHeaderTMP("");
        inventorySlots[highlightedButtonIndex].button.Select();
    }

    public void InitialiseInventoryUI()
    {
        DeleteAllInventoryUI();

        var inventorySO = dropMenuManager.dropMainMenu.playerInventorySO;

        for (int i = 0; i < inventorySO.gearInstanceInventory.Count; i++)
        {
            GameObject UIgearSlotGO = Instantiate(inventorySlotUIPrefab);
            UIgearSlotGO.transform.SetParent(inventorySlotsParent.transform, false);

            //scale smaller than usual for this menu
            //UIgearSlotGO.transform.localScale = new Vector3(.8f, .8f, .8f);
            UIgearSlotGO.name = "gear slot " + i;
            InventorySlotUI inventorySlotUI = UIgearSlotGO.GetComponent<InventorySlotUI>();

            inventorySlotUI.itemNameTMP.text = "";
            inventorySlotUI.itemQuantityTMP.text = "";
            inventorySlotUI.icon.sprite = inventorySlotUI.freeIcon;

            if (i < inventorySO.gearInstanceInventory.Count && inventorySO.gearInstanceInventory[i].gearSO != null)
            {
                var gearInstance = inventorySO.gearInstanceInventory[i];

                inventorySlotUI.gearInstance = gearInstance;
                inventorySlotUI.itemNameTMP.text = gearInstance.gearSO.GearName;
                inventorySlotUI.itemQuantityTMP.text = inventorySlotUI.gearInstance.QuantityString();

                bool isEquipment = gearInstance.gearSO is EquipmentSO;
                inventorySlotUI.icon.sprite = isEquipment ? inventorySlotUI.equipmentIcon : inventorySlotUI.consumableIcon;
           
                bool isCurrentlyEquipped = gearInstance.isCurrentlyEquipped;
                FieldEvents.SetTextColor(inventorySlotUI.itemNameTMP, Color.white, .5f);
                FieldEvents.SetTextColor(inventorySlotUI.itemQuantityTMP, Color.white, .5f);

                if (!isCurrentlyEquipped)
                    inventorySlotUI.button.onClick.AddListener(() => OnInventorySlotSelected(inventorySlotUI));

                inventorySlotUI.onHighlighted = () =>
                {
                    OnInventorySlotHighlighted(inventorySlotUI);
                };

                inventorySlotUI.onUnHighlighted = () =>
                {
                    FieldEvents.SetTextColor(inventorySlotUI.itemNameTMP, Color.white, inventorySlotUI.itemNameTMP.alpha);
                    FieldEvents.SetTextColor(inventorySlotUI.itemQuantityTMP, Color.white, inventorySlotUI.itemNameTMP.alpha);
                };

                inventorySlots.Add(inventorySlotUI);
            }
        }

        List<Button> inventorySlotButtons = new();
        foreach (var inventorySlot in inventorySlots)
            if (inventorySlot.gearInstance != null) inventorySlotButtons.Add(inventorySlot.button);

        FieldEvents.SetGridNavigationWrapAroundHorizontal(inventorySlotButtons, 3);
    }

    public void SetAllGearSlotsAlpha(float alphaIfAvailable, float alphaIfEquipped)
    {
        foreach (InventorySlotUI inventorySlotUI in inventorySlots)
        {
            bool isNotEquipped = !inventorySlotUI.gearInstance.isCurrentlyEquipped;

            FieldEvents.SetTextColor(inventorySlotUI.itemNameTMP, inventorySlotUI.itemNameTMP.color, isNotEquipped ? alphaIfAvailable : alphaIfEquipped);
            FieldEvents.SetTextColor(inventorySlotUI.itemQuantityTMP, inventorySlotUI.itemQuantityTMP.color, isNotEquipped ? alphaIfAvailable : alphaIfEquipped);
        }
    }

    public void OnInventorySlotSelected(InventorySlotUI inventorySlotUI)
    {
        InventorySO dropManagerInventorySOSO = dropMenuManager.dropMainMenu.dropManagerInventorySO;
        InventorySO playerInventorySO = dropMenuManager.dropMainMenu.playerInventorySO;
        GearInstance gearInstanceToAbandon = inventorySlotUI.gearInstance;

        if (!dropManagerInventorySOSO.AttemptAddGearToInventory(gearInstanceToAbandon, true))
        {
            dropMenuManager.dropMainMenu.headerTMP.text = "No space available";
            return;
        }

        dropMenuManager.dropMainMenu.playerInventorySO.RemoveGearFromInventory(gearInstanceToAbandon, true);

        dropMenuManager.dropSelectMenu.InitDropUI();
        dropMenuManager.dropSelectMenu.SetBaySlotsAlpha(1, .5f);
        InitialiseInventoryUI();
        SetAllGearSlotsAlpha(1, .5f);

        if (playerInventorySO.gearInstanceInventory.TrueForAll(x => x.gearSO == null))
        {
            ExitMenu();
            return;
        }

        highlightedButtonIndex = Mathf.Clamp(highlightedButtonIndex, 0, inventorySlots.Count - 1);
        inventorySlots[highlightedButtonIndex].button.Select();
    }

    public void OnInventorySlotHighlighted(InventorySlotUI inventorySlot)
    {
        FieldEvents.SetTextColor(inventorySlot.itemNameTMP, Color.yellow, inventorySlot.itemNameTMP.alpha);
        FieldEvents.SetTextColor(inventorySlot.itemQuantityTMP, Color.yellow, inventorySlot.itemNameTMP.alpha);
        highlightedButtonIndex = inventorySlots.IndexOf(inventorySlot);

        var gearHighlightedInstance = inventorySlot.gearInstance;

        dropMenuManager.dropMainMenu.UpdateDescriptionDisplayTMPs(gearHighlightedInstance);
        dropMenuManager.dropMainMenu.SetHeaderTMP("Abandon " + gearHighlightedInstance.gearSO.GearName + "?");
    }

    public void DeleteAllInventoryUI()
    {
        inventorySlots.Clear();

        for (int i = inventorySlotsParent.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(inventorySlotsParent.transform.GetChild(i).gameObject);
        }
    }

    public override void ExitMenu()
    {
        dropMenuManager.dropMainMenu.DisplayMainButtons(true);
        dropMenuManager.dropMainMenu.SetHeaderTMP(null);
        dropMenuManager.dropMainMenu.ClearAllDescriptionTMPs();

        foreach (InventorySlotUI inventorySlotUI in inventorySlots)   
            SetAllGearSlotsAlpha(.5f, .5f);

        dropMenuManager.EnterMenu(dropMenuManager.dropMainMenu);
        dropMenuManager.dropMainMenu.mainMenuButtons[1].SetButtonNormalColor(Color.white);
        dropMenuManager.dropMainMenu.mainMenuButtons[1].button.Select();
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitMenu();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (inventorySlots[highlightedButtonIndex].gearInstance.isCurrentlyEquipped)
            {
                UnequipHighlightedGearInstance(inventorySlots[highlightedButtonIndex].gearInstance);
                inventorySlots[highlightedButtonIndex].button.Select();
            }
        }
    }

    public void UnequipHighlightedGearInstance(GearInstance gearInstance)
    {
        dropMenuManager.dropMainMenu.playerInventorySO.UnequipGear(gearInstance, GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>());
        InitialiseInventoryUI();
        SetAllGearSlotsAlpha(1, .5f);
    }
}
