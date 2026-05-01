using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class ShopBuyMenu : ShopMenu
{
    [SerializeField] Button firstButtonToSelect;
    public GameObject UIInventorySlotPrefab, inventorySlotsParent;
    List<Button> slotButtons = new List<Button>();
    public Shop shop;
    Coroutine smamsCoroutine;
    float currentDisplayedSmams;

    public override void DisplayMenu(bool on)
    {
        displayContainer.SetActive(on);
    }

    public void InstantiateUIShopInventorySlots()
    {
        currentDisplayedSmams = shopMenuManager.mainMenu.playerPermanentStats.Smams;
        DeleteAllInventoryUI();

        if (shop.shopGearInventory == null || shop.shopGearInventory.Count == 0)
        {
            Debug.Log("shop has no items to sell");
            return;
        }

        foreach (GearSO gearSOToBuy in shop.shopGearInventory)
        {
            GameObject UIInventorySlot = Instantiate(UIInventorySlotPrefab);
            UIInventorySlot.transform.SetParent(inventorySlotsParent.transform, false);

            InventorySlotUI inventorySlot = UIInventorySlot.GetComponent<InventorySlotUI>();

            if (gearSOToBuy is EquipmentSO)
            {
                EquipmentInstance equipmentInstanceToBuy = new EquipmentInstance();
                inventorySlot.gearInstance = equipmentInstanceToBuy;
                inventorySlot.icon.sprite = inventorySlot.equipmentIcon;
            }

            else
            { 
                ConsumableInstance consumableInstanceToBuy = new ConsumableInstance();
                inventorySlot.gearInstance = consumableInstanceToBuy;
                inventorySlot.icon.sprite= inventorySlot.consumableIcon;
            }

            inventorySlot.gearInstance.gearSO = gearSOToBuy;
            inventorySlot.itemNameTMP.text = gearSOToBuy.GearName;
            inventorySlot.itemQuantityTMP.enabled = false;

            inventorySlot.button.onClick.AddListener(() => BuyGear(inventorySlot.gearInstance));

            inventorySlot.onHighlighted = () =>
            {
                shopMenuManager.mainMenu.UpdateDescriptionField(inventorySlot.gearInstance);
                shopMenuManager.mainMenu.SetHeaderTMP("Purchase " + inventorySlot.gearInstance.gearSO.GearName + " ?");
            };

            inventorySlot.onUnHighlighted = () =>
            {
                //
            };

            UIInventorySlot.name = "shop item " + gearSOToBuy.GearName;

            slotButtons.Add(inventorySlot.button);
        }

        FieldEvents.SetGridNavigationWrapAroundHorizontal(slotButtons, 3);
        firstButtonToSelect = slotButtons[0];
    }

    void SetInventorySlotColor(InventorySlotUI inventorySlot, Color normalColor)
    {
        inventorySlot.itemNameTMP.color = normalColor;
        inventorySlot.itemQuantityTMP.color = normalColor;
    }

    public void BuyGear(GearInstance gearInstanceToBuy)
    {
        var stats = shopMenuManager.mainMenu.playerPermanentStats;
        int cost = Mathf.RoundToInt(gearInstanceToBuy.gearSO.Value * (1 + shop.shopMarkupPer / 100f));

        if (stats.Smams < cost)
        {
            shopMenuManager.mainMenu.SetHeaderTMP("Insufficient $MAMS");
            shopMenuManager.mainMenu.smamsColorAnimator.Play("SmamsRedText", 0, 0);
            return;
        }

        if (!shopMenuManager.mainMenu.playerInventorySO.AttemptAddGearToInventory(gearInstanceToBuy, true))
        {
            shopMenuManager.mainMenu.SetHeaderTMP("Inventory full");
            return;
        }

        int smamsInitial = Mathf.RoundToInt(currentDisplayedSmams);
        stats.Smams -= cost;
        int smamsFinal = stats.Smams;

        if (smamsCoroutine != null)
            StopCoroutine(smamsCoroutine);

        smamsCoroutine = StartCoroutine(FieldEvents.LerpValuesCoRo(smamsInitial, smamsFinal, 0.5f, UpdateSmamsText));

        shopMenuManager.mainMenu.smamsColorAnimator.Play("SmamsRedText", 0, 0);
        shopMenuManager.sellMenu.InitialiseInventoryUI();

        void UpdateSmamsText(float value)
        {
            currentDisplayedSmams = value;
            shopMenuManager.mainMenu.smamsInventoryTMP.text = "Account: " + Mathf.RoundToInt(value).ToString("N0") + " $MAMS";
        }
    }

    public void DeleteAllInventoryUI()
    {
        slotButtons.Clear();

        for (int i = inventorySlotsParent.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(inventorySlotsParent.transform.GetChild(i).gameObject);
        }
    }

    public override void EnterMenu()
    {
        shopMenuManager.mainMenu.DisplayMainButtons(false);
        shopMenuManager.mainMenu.SetHeaderTMP("Select GEAR to purchase:");
        shopMenuManager.mainMenu.firstMenuButton = shopMenuManager.mainMenu.mainShopMenuButtons[0].button;
        firstButtonToSelect.Select();
    }

    public override void ExitMenu()
    {
        shopMenuManager.mainMenu.DisplayMainButtons(true);
        shopMenuManager.mainMenu.SetHeaderTMP("");
        shopMenuManager.mainMenu.DescriptionFieldClear();
        shopMenuManager.menuUpdateMethod = shopMenuManager.mainMenu;
        shopMenuManager.EnterMenu(shopMenuManager.mainMenu);
        shopMenuManager.mainMenu.mainShopMenuButtons[0].SetButtonNormalColor(Color.white);
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitMenu();
        }
    }
}
