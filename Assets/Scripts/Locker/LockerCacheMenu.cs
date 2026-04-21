using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockerCacheMenu : LockerMenu
{
    public GameObject inventorySlotPrefab, lockerBorderPrefab, inventoryUIParent;
    public List<InventorySlotUI> inventorySlots = new List<InventorySlotUI>();

    private int highlightedButtonIndex = 0;

    public int HighlightedButtonIndex
    {
        get => highlightedButtonIndex;
        set
        {
            highlightedButtonIndex = value;
        }
    }

    public override void DisplayMenu(bool on)
    {
        throw new System.NotImplementedException();
    }

    public override void EnterMenu()
    {
        lockerMenuManager.lockerGearMenu.highlightedButtonIndex = 0;
        highlightedButtonIndex = 0;
        SetBaySlotsAlpha(.7f, 1);

        inventorySlots[highlightedButtonIndex].button.Select();
        lockerMenuManager.lockerMainMenu.mainMenuButtons[1].SetButtonNormalColor(Color.yellow);

        lockerMenuManager.lockerMainMenu.DisplayMainButtons(false);
        lockerMenuManager.lockerMainMenu.SetHeaderTMP("Select GEAR to retrieve:");
    }

    public void InstantiateUIBays()
    {
        DeleteAllInventoryUI();

        List<Button> inventorySlotButtons = new List<Button>();
        var inventorySO = lockerMenuManager.lockerMainMenu.lockerInventorySO;

        for (int i = 0; i < inventorySO.gearInstanceInventory.Count; i++)
        {
            GameObject InventorySlotUIGO = Instantiate(inventorySlotPrefab, inventoryUIParent.transform);
            InventorySlotUI inventorySlotUI = InventorySlotUIGO.GetComponent<InventorySlotUI>();

            inventorySlots.Add(inventorySlotUI);
            inventorySlotUI.name = "Bay Slot Free";
            inventorySlotUI.itemNameTMP.text = "FREE";
            inventorySlotUI.itemQuantityTMP.text = "";
            inventorySlotUI.icon.enabled = false;

            if (inventorySO.gearInstanceInventory[i] != null)
            {
                inventorySlotUI.gearInstance = inventorySO.gearInstanceInventory[i];
                inventorySlotUI.icon.enabled = true;
            
                if (inventorySO.gearInstanceInventory[i] is EquipmentInstance equipmentInstance)
                {
                    inventorySlotUI.icon.sprite = inventorySlotUI.equipmentIcon;
                    inventorySlotUI.itemQuantityTMP.text = inventorySlotUI.gearInstance.GearQuantityRemainingString();
                }
            
                if (inventorySO.gearInstanceInventory[i] is ConsumableInstance consumableInstance)
                {
                    inventorySlotUI.icon.sprite = inventorySlotUI.consumableIcon;
                    inventorySlotUI.itemQuantityTMP.text = inventorySlotUI.gearInstance.GearQuantityRemainingString();
                }
            
                inventorySlotUI.itemNameTMP.text = inventorySO.gearInstanceInventory[i].gearSO.gearName.ToUpper();
                inventorySlotUI.name = "Bay Slot " + inventorySO.gearInstanceInventory[i].gearSO.gearName;
            }

                ResizeBorder(inventorySlotUI);
                inventorySlotUI.button.onClick.AddListener(() => BaySelected(inventorySlotUI));
                inventorySlotUI.onHighlighted = () =>
               {
                   SlotHighlighted(inventorySlotUI);
               };
           
               inventorySlotUI.onUnHighlighted = () =>
               {
                   SlotUnHighlighted(inventorySlotUI);
               };
        }

        foreach (var inventorySlot in inventorySlots)
            inventorySlotButtons.Add(inventorySlot.button);

        FieldEvents.SetGridNavigationWrapAroundHorizontal(inventorySlotButtons, inventorySO.gearInstanceInventory.Count);
    }

    public void SetBaySlotsAlpha(float alphaIfEmpty, float alphaIfOccupied)
    {
        foreach (InventorySlotUI inventorySlotUI in inventorySlots)
        {
            bool isSlotOccupied = inventorySlotUI.gearInstance == null;

            FieldEvents.SetTextColor(inventorySlotUI.itemNameTMP, inventorySlotUI.itemNameTMP.color, isSlotOccupied ? alphaIfEmpty : alphaIfOccupied);
            FieldEvents.SetTextColor(inventorySlotUI.itemQuantityTMP, inventorySlotUI.itemQuantityTMP.color, isSlotOccupied ? alphaIfEmpty : alphaIfOccupied);
        }
    }

    public void SetBaySlotsAlphaUICachingMode(float alphaIfAvailableToCache, float alphaIfUnavailableToCache)
    {
        foreach (InventorySlotUI inventorySlotUI in inventorySlots)
        {
            bool availableToCache = false;
            GearInstance gearToCache = lockerMenuManager.lockerGearMenu.selectedGearInstanceToCache;

            if (inventorySlotUI.gearInstance == null)
            {
                availableToCache = true;
            }

            else if (inventorySlotUI.gearInstance is ConsumableInstance slotConsumable &&
                     gearToCache is ConsumableInstance cacheConsumable &&
                     cacheConsumable.gearSO == slotConsumable.gearSO &&
                     slotConsumable.quantityAvailable < 9)
            {
                availableToCache = true;
            }

            float alpha = availableToCache ? alphaIfAvailableToCache : alphaIfUnavailableToCache;

            FieldEvents.SetTextColor(inventorySlotUI.itemNameTMP, Color.white, alpha);
            FieldEvents.SetTextColor(inventorySlotUI.itemQuantityTMP, Color.white, alpha);
        }
    }

    void ResizeBorder(InventorySlotUI inventorySlotUI)
    {
        int magicNumberBuffer = 190;
        var totalHeight = inventorySlotUI.itemNameTMP.preferredHeight + magicNumberBuffer;

        GameObject lockerBorderGO = Instantiate(lockerBorderPrefab, inventorySlotUI.gameObject.transform);
        lockerBorderGO.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, totalHeight);
    }

    void SlotHighlighted(InventorySlotUI inventorySlotUI)
    {
        if (!lockerMenuManager.lockerGearMenu.isGearSelectedForCache)
            lockerMenuManager.lockerMainMenu.SetHeaderTMP("Select GEAR to retrieve:");

        if (lockerMenuManager.lockerGearMenu.isGearSelectedForCache)
            lockerMenuManager.lockerMainMenu.SetHeaderTMP("Select CACHE bay:");

        FieldEvents.SetTextColor(inventorySlotUI.itemNameTMP, Color.yellow, inventorySlotUI.itemNameTMP.alpha);
        FieldEvents.SetTextColor(inventorySlotUI.itemQuantityTMP, Color.yellow, inventorySlotUI.itemNameTMP.alpha);

        HighlightedButtonIndex = inventorySlots.IndexOf(inventorySlotUI);
    }
     
    void SlotUnHighlighted(InventorySlotUI inventorySlotUI)
    {
        FieldEvents.SetTextColor(inventorySlotUI.itemNameTMP, Color.white, inventorySlotUI.itemNameTMP.alpha);
        FieldEvents.SetTextColor(inventorySlotUI.itemQuantityTMP, Color.white, inventorySlotUI.itemNameTMP.alpha);
    }

    void BaySelected(InventorySlotUI inventorySlotUI)
    {
        var lockerInventorySO = lockerMenuManager.lockerMainMenu.lockerInventorySO;

        if (lockerMenuManager.lockerGearMenu.isGearSelectedForCache)
        {
            var instanceToCache = lockerMenuManager.lockerGearMenu.selectedGearInstanceToCache;

            if (instanceToCache is ConsumableInstance)
                lockerMenuManager.lockerGearMenu.CacheConsumable(lockerInventorySO.gearInstanceInventory[highlightedButtonIndex]);

            if (instanceToCache is EquipmentInstance)
                lockerMenuManager.lockerGearMenu.CacheEquipment(lockerInventorySO.gearInstanceInventory[highlightedButtonIndex]);
        }

        else
        {
            if (inventorySlotUI.gearInstance == null) return;

            bool inventorySpaceAvailable = lockerMenuManager.lockerMainMenu.playerInventory.inventorySO.AttemptAddGearToInventory(inventorySlotUI.gearInstance, true);
            if (!inventorySpaceAvailable)
            {
                lockerMenuManager.lockerMainMenu.SetHeaderTMP("No slot available");
                return;
            }

            lockerInventorySO.RemoveGearFromInventory(lockerInventorySO.gearInstanceInventory[highlightedButtonIndex], false);
            InstantiateUIBays();
            lockerMenuManager.lockerGearMenu.InitialiseInventoryUI();
            inventorySlots[HighlightedButtonIndex].button.Select();
            SetBaySlotsAlpha(.7f, 1);
        }
    }

    void SetEquipSlotColor(InventorySlotUI inventorySlot, Color normalColor)
    {
        inventorySlot.itemNameTMP.color = normalColor;
        inventorySlot.itemQuantityTMP.color = normalColor;
    }

    public void DeleteAllInventoryUI()
    {
        inventorySlots.Clear();

        for (int i = inventoryUIParent.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(inventoryUIParent.transform.GetChild(i).gameObject);
        }
    }

    public override void ExitMenu()
    {
        lockerMenuManager.lockerMainMenu.DisplayMainButtons(true);
        lockerMenuManager.lockerMainMenu.SetHeaderTMP(null);

        lockerMenuManager.EnterMenu(lockerMenuManager.lockerMainMenu);
        lockerMenuManager.lockerMainMenu.mainMenuButtons[1].SetButtonNormalColor(Color.white);
        lockerMenuManager.lockerCacheMenu.SetBaySlotsAlpha(.7f, .7f);
        lockerMenuManager.lockerMainMenu.mainMenuButtons[1].button.Select();
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitMenu();
        }
    }
}
