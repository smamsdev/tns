using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockerBayMenu : LockerMenu
{
    public GameObject inventorySlotPrefab, lockerBorderPrefab, inventoryUIParent;
    public List<InventorySlotUI> inventorySlots = new List<InventorySlotUI>();
    public int highlightedButtonIndex = 0;

    public override void DisplayMenu(bool on)
    {
        throw new System.NotImplementedException();
    }

    public override void EnterMenu()
    {
        inventorySlots[highlightedButtonIndex].button.Select();
        lockerMenuManager.lockerMainMenu.mainMenuButtons[1].SetButtonNormalColor(Color.yellow);
    }

    public void InstantiateUIBays()
    {
        DeleteAllInventoryUI();

        List<Button> inventorySlotButtons = new List<Button>();
        var inventory = lockerMenuManager.lockerMainMenu.lockerInventorySO;

        foreach (GearInstance lockerInstanceSlot in inventory.gearInstanceInventory)
        {
            GameObject InventorySlotUIGO = Instantiate(inventorySlotPrefab, inventoryUIParent.transform);

            InventorySlotUI inventorySlotUI = InventorySlotUIGO.GetComponent<InventorySlotUI>();
            inventorySlots.Add(inventorySlotUI);

            GameObject lockerBorderGO = Instantiate(lockerBorderPrefab,InventorySlotUIGO.transform);

            if (lockerInstanceSlot == null || lockerInstanceSlot.gearSO == null)
            {
                inventorySlotUI.name = "Bay Slot Free";
                inventorySlotUI.itemNameTMP.text = "FREE";
                inventorySlotUI.itemQuantityTMP.text = "";
                inventorySlotUI.icon.enabled = false;
            }

            else
            {
                inventorySlotUI.gearInstance = lockerInstanceSlot;
                inventorySlotUI.icon.enabled = true;

                if (lockerInstanceSlot is EquipmentInstance equipmentInstance)
                {
                    inventorySlotUI.icon.sprite = inventorySlotUI.equipmentIcon;
                    inventorySlotUI.itemQuantityTMP.text = FieldEvents.ItemQuantityRemaining(inventorySlotUI.gearInstance);

                }

                if (lockerInstanceSlot is ConsumableInstance consumableInstance)
                {
                    inventorySlotUI.icon.sprite = inventorySlotUI.consumableIcon;
                    inventorySlotUI.itemQuantityTMP.text = FieldEvents.ItemQuantityRemaining(inventorySlotUI.gearInstance);
                }

                inventorySlotUI.itemNameTMP.text = lockerInstanceSlot.gearSO.gearName.ToUpper();
                inventorySlotUI.name = "Bay Slot " + lockerInstanceSlot.gearSO.gearName;
            }

            int magicNumberBuffer = 190;
            var totalHeight = inventorySlotUI.itemNameTMP.preferredHeight + magicNumberBuffer;

            lockerBorderGO.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, totalHeight);

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

        FieldEvents.SetGridNavigationWrapAroundHorizontal(inventorySlotButtons, inventory.gearInstanceInventory.Count);

    }

    void SlotHighlighted(InventorySlotUI inventorySlotUI)
    {
        inventorySlotUI.itemNameTMP.color = Color.yellow;
        inventorySlotUI.itemQuantityTMP.color = Color.yellow;
        highlightedButtonIndex = inventorySlots.IndexOf(inventorySlotUI);
    }

    void SlotUnHighlighted(InventorySlotUI inventorySlotUI)
    {
        inventorySlotUI.itemNameTMP.color = Color.white;
        inventorySlotUI.itemQuantityTMP.color = Color.white;
    }

    void BaySelected(InventorySlotUI inventorySlotUI)
    {
        if (lockerMenuManager.lockerGearMenu.isCaching)
            lockerMenuManager.lockerGearMenu.CacheGear(inventorySlotUI);        
    }

    void RetrieveGear(InventorySlotUI inventorySlotUI)
    {
        var lockerInventorySO = lockerMenuManager.lockerMainMenu.lockerInventorySO;
        var playerInventory = lockerMenuManager.lockerMainMenu.playerInventory;

        playerInventory.inventorySO.AddGearToInventory(inventorySlotUI.gearInstance);
        lockerInventorySO.gearInstanceInventory[highlightedButtonIndex] = null;

        InstantiateUIBays();

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
        lockerMenuManager.EnterMenu(lockerMenuManager.lockerMainMenu);
        lockerMenuManager.lockerMainMenu.mainMenuButtons[1].SetButtonNormalColor(Color.white);
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
