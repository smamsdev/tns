using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LockerGearMenu : LockerMenu
{
    public GameObject inventorySlotUIPrefab, noneGO, inventorySlotsParent;
    public List<InventorySlotUI> inventorySlots = new();
    public int highlightedButtonIndex = 0;
    public bool isGearSelectedForCache;
    public GearInstance selectedGearInstanceToCache;

    public override void DisplayMenu(bool on)
    {
        throw new System.NotImplementedException();
    }

    public override void EnterMenu()
    {
        var inventory = lockerMenuManager.lockerMainMenu.playerInventorySO.gearInstanceInventory;
        if (inventory.TrueForAll(x => x.gearSO == null))
        {
            lockerMenuManager.menuUpdateMethod = lockerMenuManager.lockerMainMenu;
            return;
        }

        isGearSelectedForCache = false;
        lockerMenuManager.lockerCacheMenu.HighlightedButtonIndex = 0;
        highlightedButtonIndex = 0;
        lockerMenuManager.lockerMainMenu.mainMenuButtons[0].SetButtonNormalColor(Color.yellow);
        lockerMenuManager.lockerCacheMenu.SetBaySlotsAlpha(.5f, .5f);
        SetAllGearSlotsAlpha(1, .5f);
        lockerMenuManager.lockerMainMenu.DisplayMainButtons(false);
        lockerMenuManager.lockerMainMenu.SetHeaderTMP("");
        inventorySlots[highlightedButtonIndex].button.Select();
    }

    public void InitialiseInventoryUI()
    {
        DeleteAllInventoryUI();

        var inventorySO = lockerMenuManager.lockerMainMenu.playerInventorySO;

        for (int i = 0; i < inventorySO.gearInstanceInventory.Count; i++)
        {
            GameObject UIgearSlotGO = Instantiate(inventorySlotUIPrefab);
            UIgearSlotGO.transform.SetParent(inventorySlotsParent.transform, false);

            //scale smaller than usual for this menu
            UIgearSlotGO.transform.localScale = new Vector3(.7f, .7f, .7f);
            UIgearSlotGO.name = "gear slot " + i;
            InventorySlotUI inventorySlotUI = UIgearSlotGO.GetComponent<InventorySlotUI>();

            inventorySlotUI.itemNameTMP.text = "";
            inventorySlotUI.itemQuantityTMP.text = "";
            inventorySlotUI.icon.sprite = inventorySlotUI.freeIcon;

            if (i < inventorySO.gearInstanceInventory.Count && inventorySO.gearInstanceInventory[i].gearSO != null)
            {
                var gearInstance = inventorySO.gearInstanceInventory[i];

                inventorySlotUI.gearInstance = gearInstance;
                inventorySlotUI.itemNameTMP.text = gearInstance.gearSO.gearName;
                inventorySlotUI.itemQuantityTMP.text = inventorySlotUI.gearInstance.GearQuantityRemainingString();

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
        isGearSelectedForCache = true;
        selectedGearInstanceToCache = inventorySlotUI.gearInstance;
        lockerMenuManager.lockerCacheMenu.SetBaySlotsAlphaUICachingMode(1, .5f);
        lockerMenuManager.lockerMainMenu.ClearAllDescriptionTMPs();
        lockerMenuManager.lockerCacheMenu.inventorySlots[lockerMenuManager.lockerCacheMenu.HighlightedButtonIndex].button.Select();
        FieldEvents.SetTextColor(inventorySlotUI.itemNameTMP, Color.yellow, 1);
        FieldEvents.SetTextColor(inventorySlotUI.itemQuantityTMP, Color.yellow, 1);
    }

    public void CacheConsumable(GearInstance lockerInventorySlot)
    {
        ConsumableInstance selectedConsumableToCache = selectedGearInstanceToCache as ConsumableInstance;

        if (lockerInventorySlot.gearSO == null)
        {
            var lockerGearInstanceInventory = lockerMenuManager.lockerMainMenu.lockerInventorySO.gearInstanceInventory;
            var selectedBay = lockerMenuManager.lockerCacheMenu.HighlightedButtonIndex;

            ConsumableInstance newConsumableInstance = new(selectedConsumableToCache);
            lockerGearInstanceInventory[selectedBay] = newConsumableInstance;

            lockerMenuManager.lockerMainMenu.playerInventorySO.RemoveGearFromInventory(selectedConsumableToCache, true);

            lockerMenuManager.lockerCacheMenu.InstantiateUIBays();
            InitialiseInventoryUI();
            SetAllGearSlotsAlpha(1, .5f);
            isGearSelectedForCache = false;

            var inventory = lockerMenuManager.lockerMainMenu.playerInventorySO.gearInstanceInventory;

            if (inventory.TrueForAll(x => x.gearSO == null))
            {
                ExitMenu();
                return;
            }

            highlightedButtonIndex = Mathf.Clamp(highlightedButtonIndex, 0, inventorySlots.Count - 1);
            inventorySlots[highlightedButtonIndex].button.Select();
        }

        else if (lockerInventorySlot.gearSO == selectedConsumableToCache.gearSO &&
            ((ConsumableInstance)lockerInventorySlot).quantityAvailable < 3)
        {
            ((ConsumableInstance)lockerInventorySlot).quantityAvailable++;

            lockerMenuManager.lockerMainMenu.playerInventorySO
                .RemoveGearFromInventory(selectedConsumableToCache, true);

            lockerMenuManager.lockerCacheMenu.InstantiateUIBays();
            InitialiseInventoryUI();
            SetAllGearSlotsAlpha(1, .5f);
            isGearSelectedForCache = false;

            var inventory = lockerMenuManager.lockerMainMenu.playerInventorySO.gearInstanceInventory;

            if (inventory.TrueForAll(x => x.gearSO == null))
            {
                ExitMenu();
                return;
            }

            highlightedButtonIndex = Mathf.Clamp(highlightedButtonIndex, 0, inventorySlots.Count - 1);
            inventorySlots[highlightedButtonIndex].button.Select();
        }
    }

    public void CacheEquipment(GearInstance lockerInventorySlot)
    {
        if (lockerInventorySlot.gearSO == null)
        {
            var lockerGearInstanceInventory = lockerMenuManager.lockerMainMenu.lockerInventorySO.gearInstanceInventory;
            var selectedBay = lockerMenuManager.lockerCacheMenu.HighlightedButtonIndex;

            lockerGearInstanceInventory[selectedBay] = selectedGearInstanceToCache;
            lockerMenuManager.lockerMainMenu.playerInventorySO.RemoveGearFromInventory(selectedGearInstanceToCache, true);

            lockerMenuManager.lockerCacheMenu.InstantiateUIBays();
            lockerMenuManager.lockerCacheMenu.SetBaySlotsAlpha(1, .5f);
            InitialiseInventoryUI();
            SetAllGearSlotsAlpha(1, .5f);
            isGearSelectedForCache = false;

            var inventory = lockerMenuManager.lockerMainMenu.playerInventorySO.gearInstanceInventory;

            if (inventory.TrueForAll(x => x.gearSO == null))
            {
                ExitMenu();
                return;
            }

            highlightedButtonIndex = Mathf.Clamp(highlightedButtonIndex, 0, inventorySlots.Count - 1);
            inventorySlots[highlightedButtonIndex].button.Select();
        }
    }

    public void OnInventorySlotHighlighted(InventorySlotUI inventorySlot)
    {
        FieldEvents.SetTextColor(inventorySlot.itemNameTMP, Color.yellow, inventorySlot.itemNameTMP.alpha);
        FieldEvents.SetTextColor(inventorySlot.itemQuantityTMP, Color.yellow, inventorySlot.itemNameTMP.alpha);
        highlightedButtonIndex = inventorySlots.IndexOf(inventorySlot);

        var gearHighlightedInstance = inventorySlot.gearInstance;

        lockerMenuManager.lockerMainMenu.UpdateDescriptionDisplayTMPs(gearHighlightedInstance);
        lockerMenuManager.lockerMainMenu.SetHeaderTMP("Cache " + gearHighlightedInstance.gearSO.gearName + "?");
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
        lockerMenuManager.lockerMainMenu.ClearAllDescriptionTMPs();

        foreach (InventorySlotUI inventorySlotUI in inventorySlots)   
        SetAllGearSlotsAlpha(.5f, .5f);

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
                lockerMenuManager.lockerCacheMenu.SetBaySlotsAlpha(1, .5f);
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
        lockerMenuManager.lockerMainMenu.playerInventorySO.UnequipGearFromSlot(gearInstance);
        InitialiseInventoryUI();
        SetAllGearSlotsAlpha(1, .5f);
    }
}
