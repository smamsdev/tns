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

        foreach (GearSO gear in shop.shopGearInventory)
        {
            GameObject UIInventorySlot = Instantiate(UIInventorySlotPrefab);
            UIInventorySlot.transform.SetParent(inventorySlotsParent.transform, false);

            InventorySlotUI inventorySlot = UIInventorySlot.GetComponent<InventorySlotUI>();
            inventorySlot.gear = gear;
            inventorySlot.itemNameTMP.text = gear.gearName;
            inventorySlot.itemQuantityTMP.enabled = false;

            inventorySlot.button.onClick.AddListener(() => BuyGear(inventorySlot.gear));

            inventorySlot.onHighlighted = () =>
            {
                shopMenuManagerUI.UpdateDescriptionField(inventorySlot.gear);
            };

            inventorySlot.onUnHighlighted = () =>
            {
                //
            };

            UIInventorySlot.name = "shop item " + gear.gearName;

            slotButtons.Add(inventorySlot.button);
        }

        FieldEvents.SetGridNavigationWrapAround(slotButtons, 5);
        firstButtonToSelect = slotButtons[0];
    }

    public void BuyGear(GearSO gearToBuy)
    {
        var stats = shopMenuManagerUI.player.GetComponent<PlayerCombat>().playerPermanentStats;

        if (stats.Smams >= gearToBuy.value)
        {
            stats.Smams -= gearToBuy.value;

            shopMenuManagerUI.playerInventory.AddGearToInventory(gearToBuy);

            shopMenuManagerUI.mainMenu.smamsValue.text = stats.Smams.ToString(); ;
            shopMenuManagerUI.smamsColorAnimator.SetTrigger("minus");
            shopMenuManagerUI.sellMenu.InstantiateUIInventorySlots();
            shopMenuManagerUI.sellMenu.firstButtonToSelect = shopMenuManagerUI.sellMenu.inventorySlotButtons[0];
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
        shopMenuManagerUI.mainShopMenuButtons[0].ButtonSelectedAndDisabled();
        shopMenuManagerUI.GearDescriptionGO.SetActive(true);
        shopMenuManagerUI.mainMenu.firstMenuButton = shopMenuManagerUI.mainShopMenuButtons[0].button;
        firstButtonToSelect.Select();
    }

    public override void ExitMenu()
    {
        shopMenuManagerUI.WireAllMainButtons();
        shopMenuManagerUI.menuUpdateMethod = shopMenuManagerUI.mainMenu;
        shopMenuManagerUI.EnterSubMenu(shopMenuManagerUI.mainMenu);
        shopMenuManagerUI.mainShopMenuButtons[0].SetButtonNormalColor(Color.white);
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitMenu();
        }
    }
}
