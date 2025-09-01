using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopMenuSell : ShopMenu
{
    [SerializeField] Button firstButtonToSelect;
    public GameObject itemDescriptionGO;
    public InventorySlot[] inventorySlot;
    public GameObject noItemsDisplay;

    public override void DisplayMenu(bool on)

    {
        itemDescriptionGO.SetActive(false);
        LoadInventory();
        displayContainer.SetActive(on);
    }

    void LoadInventory()

    {
        DisableAllSlots();

        Debug.Log("fix this");
        //for (int i = 0; i < menuManagerUI.playerInventory.inventory.Count; i++)
        //
        //{
        //    Gear gearToLoad = menuManagerUI.playerInventory.inventory[i].GetComponent<Gear>();
        //    inventorySlot[i].gear = gearToLoad;
        //    inventorySlot[i].itemName.text = gearToLoad.gearID;
        //    inventorySlot[i].itemQuantity.text = " x " + gearToLoad.quantityInInventory;
        //    inventorySlot[i].gameObject.SetActive(true);
        //}
        //
        //if (menuManagerUI.playerInventory.inventory.Count == 0)
        //
        //{
        //    ExitMenu();
        //    noItemsDisplay.SetActive(true);
        //}
    }

    void DisableAllSlots()
    {
        for (int i = 0; i < inventorySlot.Length; i++)
        {
            inventorySlot[i].gameObject.SetActive(false);
        }
        noItemsDisplay.SetActive(false);
    }

    public override void EnterMenu()
    {
        Debug.Log("fix this");
        //if (menuManagerUI.playerInventory.inventory.Count != 0)
        //
        //{
        //    shopButtonHighlighted.SetButtonColor(shopButtonHighlighted.highlightedColor);
        //    shopButtonHighlighted.enabled = false;
        //    itemDescriptionGO.SetActive(true);
        //    firstButtonToSelect.Select();
        //}
        //
        //if (menuManagerUI.playerInventory.inventory.Count == 0) 
        //
        //{
        //    return;
        //}
    }

    public override void ExitMenu()

    {
        shopButtonHighlighted.enabled = true;
        shopButtonHighlighted.SetButtonColor(Color.white);
        mainButtonToRevert.Select();
        menuManagerUI.menuUpdateMethod = menuManagerUI.main;
    }

    public void SellGearInSlot(InventorySlot slotPressed)

    { 
        GearSO gearToSell = slotPressed.gear;
        gearToSell.quantityInInventory--;
        //menuManagerUI.playerInventory.inventorySO.inventoryString.Remove(gearToSell.name);
        //menuManagerUI.playerInventory.LoadInventoryFromSO();
        Debug.Log("fix this");
        LoadInventory();

        var main = menuManagerUI.mainMenu;
        main.playerPermanentStats.smams += gearToSell.value / 2;
        main.smamsValue.text = $"{main.playerPermanentStats.smams}";
        menuManagerUI.smamsColorAnimator.SetTrigger("plus");

        if (gearToSell.quantityInInventory == 0)

        {
            if (slotPressed == inventorySlot[0])
            {
                return;
            }

            else 
            {
                inventorySlot[(int.Parse(slotPressed.name) - 1)].GetComponent<Button>().Select();
            }
        }
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitMenu();
        }
    }
}
