using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class MenuGearMainPage : PauseMenu
{
    [SerializeField] GameObject equippedDisplayContainer, inventoryDisplayContainer, gearPropertiesDisplay;
    public TextMeshProUGUI headerTMP, chargeAmountTMP, gearDescriptionTMP, gearValueTMP, gearEquipStatusTMP;
    public MenuButtonHighlighted equippedHighlightedButton, inventoryHighlightedButton;
    public MenuGearEquipSubPage menuGearEquipSubPage;
    public MenuGearInventorySubPage menuGearInventorySubPage;
    public PlayerInventorySO playerInventorySO;

    public override void DisplayMenu(bool on)
    {
        displayContainer.SetActive(on);
    }

    public void ShowMainButtons(bool on)
    {
        equippedHighlightedButton.gameObject.SetActive(on);
        inventoryHighlightedButton.gameObject.SetActive(on);
    }

    private void WireButton(MenuButtonHighlighted button, GameObject container)
    {
        button.onHighlighted = () => container.SetActive(true);
        button.onUnHighlighted = () => container.SetActive(false);

        container.SetActive(false);
    }

    public override void EnterMenu()
    {
        playerInventorySO = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>().playerInventorySO;
        UpdateHeaderTMP("");
        displayContainer.SetActive(true);
        ShowMainButtons(true);
        WireButton(equippedHighlightedButton, equippedDisplayContainer);
        WireButton(inventoryHighlightedButton, inventoryDisplayContainer);
        pauseMenuManager.ClearThenDisplayMenu(this);
        gearPropertiesDisplay.SetActive(false);
        menuGearEquipSubPage.InitialiseEquipSlots();
        menuGearInventorySubPage.InitialiseInventoryUI();

        foreach (var slot in menuGearEquipSubPage.equipSlots)
            SetSlotAlpha(slot, false);

        foreach (var slot in menuGearInventorySubPage.inventorySlots)
            SetSlotAlpha(slot, false);

        if (lastParentButtonSelected ==null) { lastParentButtonSelected = equippedHighlightedButton; }
        lastParentButtonSelected.button.Select();
    }

    public override void ExitMenu()
    {
        pauseMenuManager.EnterMenu(pauseMenuManager.main);
        pauseMenuManager.menuUpdateMethod.lastParentButtonSelected.SetButtonNormalColor(Color.white);
        pauseMenuManager.menuUpdateMethod.lastParentButtonSelected.button.Select();
    }

    public void EnterEquipSubPage()
    {
        equippedHighlightedButton.ButtonSelectedAndDisabled();
        menuGearInventorySubPage.displayContainer.SetActive(false);
        gearPropertiesDisplay.SetActive(true);
        pauseMenuManager.EnterMenu(pauseMenuManager.gearEquipSubPage);
    }

    public void EnterInventorySubPage()
    {
        if (playerInventorySO.gearInstanceInventory.Count > 0)
        {
            inventoryHighlightedButton.ButtonSelectedAndDisabled();
            menuGearEquipSubPage.displayContainer.SetActive(false);
            gearPropertiesDisplay.SetActive(true);
            pauseMenuManager.EnterMenu(pauseMenuManager.gearInventorySubPage);
        }
    }

    public void UpdateGearDescriptionTMPs(GearInstance gearInstance)
    {
        if (gearInstance.gearSO == null)
        {
            gearDescriptionTMP.text = "";
            chargeAmountTMP.text = "";
            gearEquipStatusTMP.text = "";
            gearValueTMP.text = "";
            return;
        }

        //charge
        if (gearInstance is EquipmentInstance equipmentInstance) 
            chargeAmountTMP.text = equipmentInstance.ChargeTotalString();

        else if (gearInstance is ConsumableInstance consumableInstance)
            chargeAmountTMP.text = consumableInstance.QuantityString();

        gearDescriptionTMP.text = gearInstance.DescriptionFormatted();
        gearValueTMP.text = gearInstance.SellValueFormattedString();

        if (gearInstance.isCurrentlyEquipped)
            gearEquipStatusTMP.text = "Equipped to Slot " + (gearInstance.EquippedSlotInt(playerInventorySO) + 1) + ". Press CTRL to unequip";


        else
            gearEquipStatusTMP.text = "";
    }

    public void UpdateHeaderTMP(string text)
    { 
        headerTMP.text = text;
    }

    public void SetSlotColor(InventorySlotUI inventorySlot, Color normalColor)
    {
        FieldEvents.SetTextColor(inventorySlot.itemNameTMP, normalColor, inventorySlot.itemNameTMP.alpha);
        FieldEvents.SetTextColor(inventorySlot.itemQuantityTMP, normalColor, inventorySlot.itemQuantityTMP.alpha);
    }

    public void SetSlotAlpha(InventorySlotUI inventorySlotUI, bool alphaCondition)
    {
        inventorySlotUI.itemNameTMP.alpha = alphaCondition ? 1 : 0.6f;
        inventorySlotUI.itemQuantityTMP.alpha = alphaCondition ? 1 : 0.6f;
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuGearEquipSubPage.isEquipping = false;
            ExitMenu();
        }
    }
}
