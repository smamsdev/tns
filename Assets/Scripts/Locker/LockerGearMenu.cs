using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LockerGearMenu : LockerMenu
{
    public GameObject inventorySlotUIPrefab, noneGO, inventorySlotsParent;
    public TextMeshProUGUI gearDescriptionTMP, gearTypeTMP, gearEquipStatusTMP;
    public List<InventorySlotUI> inventorySlots = new List<InventorySlotUI>();
    public int highlightedButtonIndex = 0;
    public bool isGearSelectedForCache;
    public GearInstance selectedGearInstanceToCache;

    public override void DisplayMenu(bool on)
    {
        throw new System.NotImplementedException();
    }

    public override void EnterMenu()
    {
        if (lockerMenuManager.lockerMainMenu.playerInventory.inventorySO.gearInstanceInventory.Count == 0)
            return;

        isGearSelectedForCache = false;
        lockerMenuManager.lockerBayMenu.HighlightedButtonIndex = 0;
        highlightedButtonIndex = 0;


        lockerMenuManager.lockerMainMenu.mainMenuButtons[0].SetButtonNormalColor(Color.yellow);

        lockerMenuManager.lockerMainMenu.DisplayMainButtons(false);
        lockerMenuManager.lockerMainMenu.SetHeaderTMP("Select GEAR to cache:");

        SetGearSlotsAlpha(1, .7f);

        inventorySlots[highlightedButtonIndex].button.Select();
        //inventorySlots[highlightedButtonIndex].onHighlighted.Invoke();
    }

    public void InitialiseInventoryUI()
    {
        DeleteAllInventoryUI();

        var gearInstanceInventory = lockerMenuManager.lockerMainMenu.playerInventory.inventorySO.gearInstanceInventory;

        if (lockerMenuManager.lockerMainMenu.playerInventory.inventorySO.gearInstanceInventory.Count == 0)
        {
            noneGO.SetActive(true);
            return;
        }

        noneGO.SetActive(false);

        foreach (GearInstance gearInstance in gearInstanceInventory)
        {
            GameObject UIgearSlotGO = Instantiate(inventorySlotUIPrefab);
            UIgearSlotGO.transform.SetParent(inventorySlotsParent.transform, false);

            //scale smaller than usual for this menu
            UIgearSlotGO.transform.localScale = new Vector3(.7f, .7f, .7f);
            UIgearSlotGO.name = gearInstance.gearSO.gearName;
            InventorySlotUI inventorySlotUI = UIgearSlotGO.GetComponent<InventorySlotUI>();

            inventorySlotUI.gearInstance = gearInstance;
            inventorySlotUI.itemNameTMP.text = gearInstance.gearSO.gearName;
            inventorySlotUI.itemQuantityTMP.text = FieldEvents.ItemQuantityRemaining(inventorySlotUI.gearInstance);

            bool isEquipment = gearInstance.gearSO is EquipmentSO;
            inventorySlotUI.icon.sprite = isEquipment ? inventorySlotUI.equipmentIcon : inventorySlotUI.consumableIcon;

            bool isCurrentlyEquipped = gearInstance.isCurrentlyEquipped;
            FieldEvents.SetTextColor(inventorySlotUI.itemNameTMP, Color.white, isCurrentlyEquipped ? .7f : 1);
            FieldEvents.SetTextColor(inventorySlotUI.itemQuantityTMP, Color.white, isCurrentlyEquipped ? .7f : 1);

            if (!isCurrentlyEquipped)
                inventorySlotUI.button.onClick.AddListener(() => OnInventorySlotSelected(inventorySlotUI));

            inventorySlotUI.onHighlighted = () =>
            {
                OnInventorySlotHighlighted(inventorySlotUI, isCurrentlyEquipped);
            };

            inventorySlotUI.onUnHighlighted = () =>
            {
                FieldEvents.SetTextColor(inventorySlotUI.itemNameTMP, Color.white, inventorySlotUI.itemNameTMP.alpha);
                FieldEvents.SetTextColor(inventorySlotUI.itemQuantityTMP, Color.white, inventorySlotUI.itemNameTMP.alpha);
            };

            inventorySlots.Add(inventorySlotUI);
        }

        List<Button> inventorySlotButtons = new List<Button>();
        foreach (var inventorySlot in inventorySlots)
            inventorySlotButtons.Add(inventorySlot.button);

        FieldEvents.SetGridNavigationWrapAroundHorizontal(inventorySlotButtons, 3);
    }

    public void SetGearSlotsAlpha(float alphaIfNotEquipped, float alphaIfEquipped)
    {
        foreach (InventorySlotUI inventorySlotUI in inventorySlots)
        {
            bool isEquipped = inventorySlotUI.gearInstance.isCurrentlyEquipped;

            FieldEvents.SetTextColor(inventorySlotUI.itemNameTMP, Color.white, !isEquipped ? alphaIfNotEquipped : alphaIfEquipped);
            FieldEvents.SetTextColor(inventorySlotUI.itemQuantityTMP, Color.white, !isEquipped ? alphaIfNotEquipped : alphaIfEquipped);
        }
    }

    public void OnInventorySlotSelected(InventorySlotUI inventorySlotUI)
    {
        isGearSelectedForCache = true;
        lockerMenuManager.lockerMainMenu.SetHeaderTMP("Select CACHE bay:");

        selectedGearInstanceToCache = inventorySlotUI.gearInstance;
        lockerMenuManager.lockerBayMenu.inventorySlots[lockerMenuManager.lockerBayMenu.HighlightedButtonIndex].button.Select();
        FieldEvents.SetTextColor(inventorySlotUI.itemNameTMP, Color.yellow, 1);
        FieldEvents.SetTextColor(inventorySlotUI.itemQuantityTMP, Color.yellow, 1);
    }

    public void CacheConsumable(GearInstance lockerInventorySlot)
    {
        ConsumableInstance selectedConsumableToCache = selectedGearInstanceToCache as ConsumableInstance;

        if (lockerInventorySlot == null)
        {
            var lockerGearInstanceInventory = lockerMenuManager.lockerMainMenu.lockerInventorySO.gearInstanceInventory;
            var selectedBay = lockerMenuManager.lockerBayMenu.HighlightedButtonIndex;

            ConsumableInstance newConsumableInstance = new ConsumableInstance(selectedConsumableToCache);
            lockerGearInstanceInventory[selectedBay] = newConsumableInstance;

            lockerMenuManager.lockerMainMenu.playerInventory.inventorySO.RemoveGearFromInventory(selectedConsumableToCache);

            lockerMenuManager.lockerBayMenu.InstantiateUIBays();
            InitialiseInventoryUI();

            highlightedButtonIndex = Mathf.Clamp(highlightedButtonIndex, 0, inventorySlots.Count - 1);

            if (inventorySlots.Count > 0)
                inventorySlots[highlightedButtonIndex].button.Select();
        }

        if (lockerInventorySlot != null && lockerInventorySlot.gearSO == selectedConsumableToCache.gearSO)
        {
            if (((ConsumableInstance)lockerInventorySlot).quantityAvailable >= 9)
                return;

            ((ConsumableInstance)lockerInventorySlot).quantityAvailable++;
            lockerMenuManager.lockerMainMenu.playerInventory.inventorySO.RemoveGearFromInventory(selectedConsumableToCache);

            lockerMenuManager.lockerBayMenu.InstantiateUIBays();
            InitialiseInventoryUI();

            highlightedButtonIndex = Mathf.Clamp(highlightedButtonIndex, 0, inventorySlots.Count - 1);

            if (inventorySlots.Count > 0)
                inventorySlots[highlightedButtonIndex].button.Select();
        }
    }

    public void CacheEquipment(GearInstance lockerInventorySlot)
    {
        if (lockerInventorySlot == null)
        {
            var lockerGearInstanceInventory = lockerMenuManager.lockerMainMenu.lockerInventorySO.gearInstanceInventory;
            var selectedBay = lockerMenuManager.lockerBayMenu.HighlightedButtonIndex;

            lockerGearInstanceInventory[selectedBay] = selectedGearInstanceToCache;
            lockerMenuManager.lockerMainMenu.playerInventory.inventorySO.RemoveGearFromInventory(selectedGearInstanceToCache);

            lockerMenuManager.lockerBayMenu.InstantiateUIBays();
            InitialiseInventoryUI();

            highlightedButtonIndex = Mathf.Clamp(highlightedButtonIndex, 0, inventorySlots.Count - 1);

            if (lockerMenuManager.lockerMainMenu.playerInventory.inventorySO.gearInstanceInventory.Count == 0)
            {
                ExitMenu();
                return;
            }

            inventorySlots[highlightedButtonIndex].button.Select();
            inventorySlots[highlightedButtonIndex].button.onClick.Invoke();
        }
    }

    public void OnInventorySlotHighlighted(InventorySlotUI inventorySlot, bool isCurrentlyEquipped)
    {
        FieldEvents.SetTextColor(inventorySlot.itemNameTMP, Color.yellow, inventorySlot.itemNameTMP.alpha);
        FieldEvents.SetTextColor(inventorySlot.itemQuantityTMP, Color.yellow, inventorySlot.itemNameTMP.alpha);

        highlightedButtonIndex = inventorySlots.IndexOf(inventorySlot);

        gearDescriptionTMP.text = "Description: " + inventorySlot.gearInstance.gearSO.gearDescription;

        //Gear Type
        if (inventorySlot.gearInstance is EquipmentInstance equipmentInstance)
        {
            gearTypeTMP.text = "Charge " + equipmentInstance.Charge + " / " + ((EquipmentSO)equipmentInstance.gearSO).maxPotential;
        }

        else
            gearTypeTMP.text = "Consumable";

        //Availability
        if (inventorySlot.gearInstance.isCurrentlyEquipped)
        {
            var inventorySO = lockerMenuManager.lockerMainMenu.playerInventory.inventorySO;
            gearEquipStatusTMP.text = "Equipped to Slot " + (inventorySO.gearInstanceEquipped.IndexOf(inventorySlot.gearInstance) + 1) + ". PRESS CTRL TO REMOVE";
        }

        else 
        {
            gearEquipStatusTMP.text = "Select to CACHE";

        }
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
        lockerMenuManager.lockerMainMenu.DisplayMainButtons(true);
        lockerMenuManager.lockerMainMenu.SetHeaderTMP(null);

        isGearSelectedForCache  = false;
        lockerMenuManager.EnterMenu(lockerMenuManager.lockerMainMenu);
        lockerMenuManager.lockerMainMenu.mainMenuButtons[0].SetButtonNormalColor(Color.white);
        lockerMenuManager.lockerMainMenu.mainMenuButtons[0].button.Select();
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
           if (isGearSelectedForCache)
           {
                lockerMenuManager.lockerMainMenu.SetHeaderTMP("Select GEAR to cache:");
                inventorySlots[highlightedButtonIndex].button.Select();
                isGearSelectedForCache = false;
                return;
           }

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
        int i = lockerMenuManager.lockerMainMenu.playerInventory.inventorySO.gearInstanceEquipped.IndexOf(gearInstance);

        lockerMenuManager.lockerMainMenu.playerInventory.inventorySO.UnequipGearFromSlot(gearInstance);
        InitialiseInventoryUI();
    }
}
