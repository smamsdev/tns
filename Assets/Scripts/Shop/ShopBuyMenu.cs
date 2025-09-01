using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopBuyMenu : ShopMenu
{
    [SerializeField] GameObject descriptionGO;
    public InventorySlot[] inventorySlot;
    [SerializeField] Button firstButtonToSelect;
    public List<Gear> shopDynamicInventory = new List<Gear>();

    public override void DisplayMenu(bool on)
    {
        displayContainer.SetActive(on);
        descriptionGO.SetActive(false);
        DisableAllSlots();
        LoadInventoryStringFromSO();
        DisplayInventoryToSlot();
    }

    void DisplayInventoryToSlot()
    {
        for (int i = 0; i < shopDynamicInventory.Count; i++)

        {
            Debug.Log("fix this");
            //GearSO gearToLoad = shopDynamicInventory[i].GetComponent<Gear>();
            //inventorySlot[i].gear = gearToLoad;
            //inventorySlot[i].itemName.text = gearToLoad.gearID;
            //inventorySlot[i].itemQuantity.text = " x " + gearToLoad.quantityInInventory;
            //inventorySlot[i].gameObject.SetActive(true);
        }
    }

    public void LoadInventoryStringFromSO()

    {
        Debug.Log("fix this");
        //var gearParent = menuManagerUI.playerInventory.GearParent;
        ShopMainMenu main = menuManagerUI.mainMenu;

        shopDynamicInventory.Clear();

        Debug.Log("fix");
        // for (int i = 0; i < main.shopInventorySO.inventoryString.Count; i++)
        // {
        //     shopDynamicInventory.Add(gearParent.Find(main.shopInventorySO.inventoryString[i]).GetComponent<Gear>()); ;
        // }
    }

    void DisableAllSlots()
    {
        for (int i = 0; i < inventorySlot.Length; i++)
        {
            inventorySlot[i].gameObject.SetActive(false);
        }
    }

    public override void EnterMenu()
    {
        shopButtonHighlighted.SetButtonColor(shopButtonHighlighted.highlightedColor);
        shopButtonHighlighted.enabled = false;
        descriptionGO.SetActive(true);
        firstButtonToSelect.Select();
    }

    public override void ExitMenu()
    {
        shopButtonHighlighted.enabled = true;
        shopButtonHighlighted.SetButtonColor(Color.white);
        mainButtonToRevert.Select();
        menuManagerUI.menuUpdateMethod = menuManagerUI.main;
    }

    public void BuyGearInSlot(InventorySlot inventorySlot)
    {
        GearSO gearToBuy = inventorySlot.gear;
        //menuManagerUI.playerInventory.inventorySO.inventoryString.Add(gearToBuy.name);
        //menuManagerUI.playerInventory.LoadInventoryFromSO();
        Debug.Log("fix this");

        var main = menuManagerUI.mainMenu;
        main.playerPermanentStats.smams -= gearToBuy.value;
        main.smamsValue.text = $"{main.playerPermanentStats.smams}";
        menuManagerUI.smamsColorAnimator.SetTrigger("minus");

        DisableAllSlots();
        LoadInventoryStringFromSO();
        DisplayInventoryToSlot();
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitMenu();
        }
    }
}
