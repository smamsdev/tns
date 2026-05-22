using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DropSeizedMenu : DropMenu
{
    public GameObject inventorySlotPrefab, inventoryUIParent;
    public List<InventorySlotUI> inventorySlots = new List<InventorySlotUI>();
    List<Button> inventorySlotButtons = new List<Button>();

    [SerializeField] private int highlightedButtonIndex = 0;
    public int HighlightedButtonIndex
    {
        get => highlightedButtonIndex;
        set => highlightedButtonIndex = value;
    }

    public override void DisplayMenu(bool on)
    {
        throw new System.NotImplementedException();
    }

    public override void EnterMenu()
    {
        dropMenuManager.dropGearMenu.highlightedButtonIndex = 0;
        highlightedButtonIndex = 0;
        SetBaySlotsAlpha(.5f, 1);
        dropMenuManager.dropMainMenu.SetHeaderTMP("");

        inventorySlots[highlightedButtonIndex].button.Select();
        dropMenuManager.dropMainMenu.mainMenuButtons[1].SetButtonNormalColor(Color.yellow);

        dropMenuManager.dropMainMenu.DisplayMainButtons(false);
    }

    public void InstantiateUIBays()
    {
        DeleteAllInventoryUI();


        var inventorySO = dropMenuManager.dropMainMenu.dropInventory;

        for (int i = 0; i < inventorySO.gearInstanceInventory.Count; i++)
        {
            GameObject InventorySlotUIGO = Instantiate(inventorySlotPrefab, inventoryUIParent.transform);
            InventorySlotUI inventorySlotUI = InventorySlotUIGO.GetComponent<InventorySlotUI>();

            //scale smaller than usual for this menu
            //InventorySlotUIGO.transform.localScale = new Vector3(.8f, .8f, .8f);
            inventorySlots.Add(inventorySlotUI);
            inventorySlotUI.name = "Bay Slot Free";
            inventorySlotUI.gearInstance = new GearInstance();
            inventorySlotUI.itemNameTMP.text = "";
            inventorySlotUI.itemQuantityTMP.text = "";
            inventorySlotUI.icon.sprite = inventorySlotUI.freeIcon;

            if (inventorySO.gearInstanceInventory[i].gearSO != null)
            {
                inventorySlotUI.name = "Bay Slot " + inventorySO.gearInstanceInventory[i].gearSO.GearName;
                inventorySlotUI.gearInstance = inventorySO.gearInstanceInventory[i];
                inventorySlotUI.itemNameTMP.text = inventorySO.gearInstanceInventory[i].gearSO.GearName;
                inventorySlotUI.itemQuantityTMP.text = inventorySlotUI.gearInstance.QuantityString();

                if (inventorySO.gearInstanceInventory[i] is EquipmentInstance equipmentInstance)
                    inventorySlotUI.icon.sprite = inventorySlotUI.equipmentIcon;

                else
                    inventorySlotUI.icon.sprite = inventorySlotUI.consumableIcon;

                inventorySlotButtons.Add(inventorySlotUI.button);
            }

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

        FieldEvents.SetGridNavigationWrapAround(inventorySlotButtons, 5);
    }

    public void SetBaySlotsAlpha(float alphaIfEmpty, float alphaIfOccupied)
    {
        foreach (InventorySlotUI inventorySlotUI in inventorySlots)
        {
            bool isSlotOccupied = inventorySlotUI.gearInstance.gearSO == null;

            FieldEvents.SetTextColor(inventorySlotUI.itemNameTMP, inventorySlotUI.itemNameTMP.color, isSlotOccupied ? alphaIfEmpty : alphaIfOccupied);
            FieldEvents.SetTextColor(inventorySlotUI.itemQuantityTMP, inventorySlotUI.itemQuantityTMP.color, isSlotOccupied ? alphaIfEmpty : alphaIfOccupied);
        }
    }

    public void SetBaySlotsAlphaUICachingMode(float alphaIfAvailableToCache, float alphaIfUnavailableToCache)
    {
        foreach (InventorySlotUI inventorySlotUI in inventorySlots)
        {
            bool availableToCache = false;
            GearInstance gearToCache = dropMenuManager.dropGearMenu.selectedGearInstanceToCache;

            if (inventorySlotUI.gearInstance.gearSO == null)
            {
                availableToCache = true;
            }

            else if (inventorySlotUI.gearInstance is ConsumableInstance slotConsumable &&
                     gearToCache is ConsumableInstance cacheConsumable &&
                     cacheConsumable.gearSO == slotConsumable.gearSO &&
                     slotConsumable.quantityAvailable < 3)
            {
                availableToCache = true;
            }

            float alpha = availableToCache ? alphaIfAvailableToCache : alphaIfUnavailableToCache;

            FieldEvents.SetTextColor(inventorySlotUI.itemNameTMP, Color.white, alpha);
            FieldEvents.SetTextColor(inventorySlotUI.itemQuantityTMP, Color.white, alpha);
        }
    }

    void SlotHighlighted(InventorySlotUI inventorySlotUI)
    {
        HighlightedButtonIndex = inventorySlots.IndexOf(inventorySlotUI);
        FieldEvents.SetTextColor(inventorySlotUI.itemNameTMP, Color.yellow, inventorySlotUI.itemNameTMP.alpha);
        FieldEvents.SetTextColor(inventorySlotUI.itemQuantityTMP, Color.yellow, inventorySlotUI.itemNameTMP.alpha);

        if (!dropMenuManager.dropGearMenu.isGearSelectedForCache)
        {
            if (inventorySlotUI.gearInstance.gearSO == null)
            {
                dropMenuManager.dropMainMenu.SetHeaderTMP("Bay " + (highlightedButtonIndex + 1) + " empty");
                dropMenuManager.dropMainMenu.ClearAllDescriptionTMPs();
                return;
            }

            dropMenuManager.dropMainMenu.SetHeaderTMP("Retrieve " + inventorySlotUI.gearInstance.gearSO.GearName + "?");
            dropMenuManager.dropMainMenu.UpdateDescriptionDisplayTMPs(inventorySlotUI.gearInstance);
        }

        if (dropMenuManager.dropGearMenu.isGearSelectedForCache)
        {
            GearInstance gearToCache = dropMenuManager.dropGearMenu.selectedGearInstanceToCache;

            if (inventorySlotUI.gearInstance.gearSO == null)
            {
                string toCache = gearToCache.gearSO.GearName;
                dropMenuManager.dropMainMenu.SetHeaderTMP("Cache " + toCache + " in bay " + (highlightedButtonIndex + 1) + "?");
            }

            else if (inventorySlotUI.gearInstance is ConsumableInstance slotConsumable &&
                     gearToCache is ConsumableInstance cacheConsumable &&
                     cacheConsumable.gearSO == slotConsumable.gearSO &&
                     slotConsumable.quantityAvailable < 3)
            {
                string toCache = gearToCache.gearSO.GearName;
                dropMenuManager.dropMainMenu.SetHeaderTMP("Cache " + toCache + " in bay " + (highlightedButtonIndex + 1) + "?");
            }

            else
                dropMenuManager.dropMainMenu.SetHeaderTMP("Bay " + (highlightedButtonIndex + 1) + " is occupied");
        }
    }
     
    void SlotUnHighlighted(InventorySlotUI inventorySlotUI)
    {
        FieldEvents.SetTextColor(inventorySlotUI.itemNameTMP, Color.white, inventorySlotUI.itemNameTMP.alpha);
        FieldEvents.SetTextColor(inventorySlotUI.itemQuantityTMP, Color.white, inventorySlotUI.itemNameTMP.alpha);
    }

    void BaySelected(InventorySlotUI inventorySlotUI)
    {
        var dropInventorySO = dropMenuManager.dropMainMenu.dropInventory;

        if (!dropMenuManager.dropMainMenu.playerInventorySO.AttemptAddGearToInventory(inventorySlotUI.gearInstance, true))
        {
            dropMenuManager.dropMainMenu.SetHeaderTMP("Inventory full");
            return;
        }

        dropInventorySO.RemoveGearFromInventory(inventorySlots[highlightedButtonIndex].gearInstance, true);
        InstantiateUIBays();
        dropMenuManager.dropGearMenu.InitialiseInventoryUI();

        highlightedButtonIndex = Mathf.Clamp(highlightedButtonIndex, 0, inventorySlotButtons.Count - 1);
        inventorySlots[HighlightedButtonIndex].button.Select();
        SetBaySlotsAlpha(.5f, 1);
    }

    void SetEquipSlotColor(InventorySlotUI inventorySlot, Color normalColor)
    {
        inventorySlot.itemNameTMP.color = normalColor;
        inventorySlot.itemQuantityTMP.color = normalColor;
    }

    public void DeleteAllInventoryUI()
    {
        inventorySlots.Clear();
        inventorySlotButtons.Clear();

        for (int i = inventoryUIParent.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(inventoryUIParent.transform.GetChild(i).gameObject);
        }
    }

    public override void ExitMenu()
    {
        dropMenuManager.dropMainMenu.DisplayMainButtons(true);
        dropMenuManager.dropMainMenu.SetHeaderTMP(null);
        dropMenuManager.dropMainMenu.ClearAllDescriptionTMPs();

        dropMenuManager.EnterMenu(dropMenuManager.dropMainMenu);
        dropMenuManager.dropMainMenu.mainMenuButtons[1].SetButtonNormalColor(Color.white);
        dropMenuManager.dropSeizedMenu.SetBaySlotsAlpha(.7f, .7f);
        dropMenuManager.dropMainMenu.mainMenuButtons[1].button.Select();
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitMenu();
        }
    }
}
