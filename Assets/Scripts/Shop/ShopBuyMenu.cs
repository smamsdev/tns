using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopBuyMenu : ShopMenu
{
    [SerializeField] Button firstButtonToSelect;
    public GameObject UIInventorySlotPrefab, inventorySlotsParent;
    List<Button> slotButtons = new List<Button>();
    public Shop shop;

    public override void DisplayMenu(bool on)
    {
        displayContainer.SetActive(on);
    }

    public void InstantiateUIShopInventorySlots()
    {
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

            bool isEquipment = inventorySlot.gearInstance is EquipmentInstance;
            SetInventorySlotColor(inventorySlot, isEquipment ? inventorySlot.equipmentColor : inventorySlot.consumableColor);

            inventorySlot.gearInstance.gearSO = gearSOToBuy;
            inventorySlot.itemNameTMP.text = gearSOToBuy.gearName;
            inventorySlot.itemQuantityTMP.enabled = false;

            inventorySlot.button.onClick.AddListener(() => BuyGear(inventorySlot.gearInstance));

            inventorySlot.onHighlighted = () =>
            {
                shopMenuManager.mainMenu.UpdateDescriptionField(inventorySlot.gearInstance.gearSO);
            };

            inventorySlot.onUnHighlighted = () =>
            {
                //
            };

            UIInventorySlot.name = "shop item " + gearSOToBuy.gearName;

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
        var stats = shopMenuManager.mainMenu.player.GetComponent<PlayerCombat>().playerPermanentStats;

        if (stats.Smams >= gearInstanceToBuy.gearSO.value)
        {
            stats.Smams -= gearInstanceToBuy.gearSO.value;


            if (gearInstanceToBuy is EquipmentInstance equipmentInstance)
            {
                EquipmentInstance clonedGear = new EquipmentInstance(equipmentInstance);
                shopMenuManager.mainMenu.playerInventory.inventorySO.AddGearToInventory(clonedGear);
            }

            else if (gearInstanceToBuy is ConsumableInstance consumableInstance)
            {
                ConsumableInstance clonedGear = new ConsumableInstance(consumableInstance);
                shopMenuManager.mainMenu.playerInventory.inventorySO.AddGearToInventory(clonedGear);
            }

            else
                Debug.Log("something weird is going on");

            shopMenuManager.mainMenu.smamsInventoryTMP.text = stats.Smams.ToString("N0");
            shopMenuManager.mainMenu.smamsColorAnimator.SetTrigger("minus");
            shopMenuManager.sellMenu.InstantiateUIInventorySlots();
            shopMenuManager.sellMenu.firstButtonToSelect = shopMenuManager.sellMenu.inventorySlotButtons[0];
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
        shopMenuManager.mainMenu.mainShopMenuButtons[0].ButtonSelectedAndDisabled();
        shopMenuManager.mainMenu.GearDescriptionGO.SetActive(true);
        shopMenuManager.mainMenu.firstMenuButton = shopMenuManager.mainMenu.mainShopMenuButtons[0].button;
        firstButtonToSelect.Select();
    }

    public override void ExitMenu()
    {
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
