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

            inventorySlot.gearInstance.gearSO = gearSOToBuy;
            inventorySlot.itemNameTMP.text = gearSOToBuy.gearName;
            inventorySlot.itemQuantityTMP.enabled = false;

            inventorySlot.button.onClick.AddListener(() => BuyGear(inventorySlot.gearInstance));

            inventorySlot.onHighlighted = () =>
            {
                shopMenuManager.mainMenu.UpdateDescriptionField(inventorySlot.gearInstance.gearSO);
                shopMenuManager.mainMenu.SetHeaderTMP("Purchase " + inventorySlot.gearInstance.gearSO.gearName + " ?");
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
            bool inventorySpaceAvailable = shopMenuManager.mainMenu.playerInventorySO.AttemptAddGearToInventory(gearInstanceToBuy, true);

            if (inventorySpaceAvailable)
            {
                int smamsInitialValue = stats.Smams;
                int smamsFinalValue = stats.Smams -= gearInstanceToBuy.gearSO.value;

                stats.Smams = smamsFinalValue;

                StartCoroutine(FieldEvents.LerpValuesCoRo(smamsInitialValue, smamsFinalValue, .5f, UpdateSmamsText));

                void UpdateSmamsText(float smamsValue)
                {
                    shopMenuManager.mainMenu.smamsInventoryTMP.text = Mathf.RoundToInt(smamsValue).ToString("N0");
                }

                shopMenuManager.mainMenu.smamsColorAnimator.Play("SmamsRedText");
                shopMenuManager.sellMenu.InitialiseInventoryUI();
            }

            else
                shopMenuManager.mainMenu.SetHeaderTMP("Inventory full");
        }

        else
        {
            shopMenuManager.mainMenu.SetHeaderTMP("Insufficient $MAMS");
            shopMenuManager.mainMenu.smamsColorAnimator.SetTrigger("SmamsRedText");
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

        shopMenuManager.mainMenu.GearDescriptionGO.SetActive(true);
        shopMenuManager.mainMenu.firstMenuButton = shopMenuManager.mainMenu.mainShopMenuButtons[0].button;
        firstButtonToSelect.Select();
    }

    public override void ExitMenu()
    {
        shopMenuManager.mainMenu.DisplayMainButtons(true);
        shopMenuManager.mainMenu.SetHeaderTMP("");

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
