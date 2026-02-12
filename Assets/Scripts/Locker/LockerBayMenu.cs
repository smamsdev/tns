using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockerBayMenu : LockerMenu
{
    public GameObject inventorySlotPrefab, inventoryUIParent;
    List<Button> buttons = new List<Button>();
    Button firstButtonToSelect;

    public override void DisplayMenu(bool on)
    {
        throw new System.NotImplementedException();
    }

    public override void EnterMenu()
    {
        throw new System.NotImplementedException();
    }

    public override void ExitMenu()
    {
        throw new System.NotImplementedException();
    }

    public void InstantiateUIBays()
    {
        DeleteAllInventoryUI();

        var inventory = lockerMenuManager.lockerMainMenu.inventory;

        foreach (GearInstance lockerInstanceSlot in inventory.gearInstanceInventory)
        {
            GameObject InventorySlotUIGO = Instantiate(inventorySlotPrefab);
            InventorySlotUIGO.transform.SetParent(inventoryUIParent.transform, false);

            InventorySlotUI inventorySlotUI = InventorySlotUIGO.GetComponent<InventorySlotUI>();

            if (lockerInstanceSlot == null || lockerInstanceSlot.gearSO == null)
            {
                inventorySlotUI.name = "Bay Slot Free";
                inventorySlotUI.itemQuantityTMP.text = "";
            }

            else
            {
                if (lockerInstanceSlot is EquipmentInstance equipmentInstance)
                {
                    inventorySlotUI.icon.sprite = inventorySlotUI.equipmentIcon;
                    inventorySlotUI.itemQuantityTMP.text = equipmentInstance.ChargePercentage().ToString() + "%";

                }

                if (lockerInstanceSlot is ConsumableInstance consumableInstance)
                {
                    inventorySlotUI.icon.sprite = inventorySlotUI.consumableIcon;
                    inventorySlotUI.itemQuantityTMP.text = consumableInstance.quantityAvailable.ToString();
                }

                inventorySlotUI.itemNameTMP.text = lockerInstanceSlot.gearSO.gearName.ToUpper();
                inventorySlotUI.name = "Bay Slot " + lockerInstanceSlot.gearSO.gearName;
            }

           //inventorySlotUI.button.onClick.AddListener(() => BuyGear(inventorySlot.gearInstance));
           //
           //inventorySlotUI.onHighlighted = () =>
           //{
           //    shopMenuManager.mainMenu.UpdateDescriptionField(inventorySlot.gearInstance.gearSO);
           //};
           //
           //inventorySlotUI.onUnHighlighted = () =>
           //{
           //    //
           //};

            buttons.Add(inventorySlotUI.button);
        }

        FieldEvents.SetGridNavigationWrapAroundHorizontal(buttons, 1);
        firstButtonToSelect = buttons[0];
    }

    public void DeleteAllInventoryUI()
    {
        buttons.Clear();

        for (int i = inventoryUIParent.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(inventoryUIParent.transform.GetChild(i).gameObject);
        }
    }


    public override void StateUpdate()
    {
        throw new System.NotImplementedException();
    }
}
