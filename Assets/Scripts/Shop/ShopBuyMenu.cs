using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopBuyMenu : ShopMenu
{
    [SerializeField] GameObject descriptionGO;
    [SerializeField] InventorySO shopInventorySO;
    public InventorySlot[] inventorySlot;
    [SerializeField] Button firstButtonToSelect;

    public Dictionary<string, int> shopDictionary;
    public List<Gear> shopInventory = new List<Gear>();

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
        for (int i = 0; i < shopInventory.Count; i++)

        {
            Gear gearToLoad = shopInventory[i].GetComponent<Gear>();
            inventorySlot[i].gear = gearToLoad;
            inventorySlot[i].itemName.text = gearToLoad.gearID;
            inventorySlot[i].itemQuantity.text = " x " + gearToLoad.quantityInInventory;
            inventorySlot[i].gameObject.SetActive(true);
        }
    }

    public void LoadInventoryStringFromSO()

    {
        shopInventory.Clear();

        for (int i = 0; i < shopInventorySO.inventoryString.Count; i++)
        {
            shopInventory.Add(GameObject.Find(shopInventorySO.inventoryString[i]).GetComponent<Gear>()); ;
        }
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

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitMenu();
        }
    }

    public void ONtest()
    {
        Debug.Log("isthison");
    }
}
