using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class MenuGear : Menu
{
    [SerializeField] Button firstButtonToSelect;
    public PlayerInventory playerInventory;
    [SerializeField] EquippedGear equippedGear;
    public GameObject itemDescriptionGO;
    public TextMeshProUGUI descriptionFieldTMP;
    public TextMeshProUGUI itemTypeTMP;

    public InventorySlot[] inventorySlot;

    public override void DisplayMenu(bool on)

    {
        itemDescriptionGO.SetActive(false);
        var player = GameObject.Find("Player");
        playerInventory = player.GetComponentInChildren<PlayerInventory>();
        equippedGear = player.GetComponentInChildren<EquippedGear>();

        DisableAllSlots();

        for (int i = 0; i < playerInventory.inventory.Count; i++)

        {
            Gear gearToLoad = playerInventory.inventory[i].GetComponent<Gear>();
            inventorySlot[i].gear = gearToLoad;
            inventorySlot[i].textMeshProUGUI.text = gearToLoad.gearID + " x " + gearToLoad.quantityInInventory;
            inventorySlot[i].gameObject.SetActive(true);
        }

        displayContainer.SetActive(on);
    }

    void DisableAllSlots()
    {
        for (int i = 0; i < inventorySlot.Length; i++)
        {
            inventorySlot[i].gameObject.SetActive(false);
        }
    }

    public void UpdateDescriptionField(string text, bool isEquipment)
    {
        descriptionFieldTMP.text = text;

        if (!isEquipment)
        {
            itemTypeTMP.text = "Equipment";
        }

        else 
        {
            itemTypeTMP.text = "Consumable";
        }

    }

    public override void EnterMenu()
    {
        menuButtonHighlighted.SetButtonColor(menuButtonHighlighted.highlightedColor);
        menuButtonHighlighted.enabled = false;
        firstButtonToSelect.Select();
        itemDescriptionGO.SetActive(true);
    }

    public override void ExitMenu()

    {
        menuButtonHighlighted.enabled = true;
        menuButtonHighlighted.SetButtonColor(Color.white);
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
}
