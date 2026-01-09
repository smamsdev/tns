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
        var smams = shopMenuManagerUI.player.GetComponent<PlayerCombat>().playerPermanentStats.Smams;
        smams -= gearToBuy.value;

        shopMenuManagerUI.mainMenu.smamsValue.text = smams.ToString(); ;
        shopMenuManagerUI.smamsColorAnimator.SetTrigger("minus");
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
        shopMenuManagerUI.GearDescriptionGO.SetActive(true);
        firstButtonToSelect.Select();
    }

    public override void ExitMenu()
    {
        Debug.Log("fix");
        //shopButtonHighlighted.enabled = true;
        //shopButtonHighlighted.SetButtonColor(Color.white);
        //mainButtonToRevert.Select();
        shopMenuManagerUI.menuUpdateMethod = shopMenuManagerUI.mainMenu;
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitMenu();
        }
    }
}
