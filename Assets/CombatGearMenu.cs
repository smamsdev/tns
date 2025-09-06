using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CombatGearMenu : MonoBehaviour
{
    public CombatManager combatManager;
    PlayerInventory playerInventory;
    public GearSelectState gearSelectState;
    [SerializeField] GameObject inventoryMenu;
    [SerializeField] Button equipNoneOption;

    public int InventorySlotNumberSelected;

    public InventorySlot[] inventorySlot;

    public List<UIGearEquipSlot> uIGearEquipSlots = new List<UIGearEquipSlot>();
    public List<Button> uIGearEquipSlotButtons = new List<Button>();
    public UIGearEquipSlot gearEquipSlotSelected;

    [SerializeField] Move equipGearMove; //player needs a move assigned to complete their turn

    public void DisplayEquipSlots()
    {
        playerInventory = combatManager.playerCombat.playerInventory;

        foreach (UIGearEquipSlot gearEquipSlot in uIGearEquipSlots)
        {
            gearEquipSlot.gearEquipped = null;
            gearEquipSlot.gameObject.SetActive(false);
        }

        for (int i = 0; i < playerInventory.inventorySO.equipSlotsAvailable; i++)
        {
            if (playerInventory.inventorySO.equippedGear[i] == null)
            {
                uIGearEquipSlots[i].buttonTMP.text = "SLOT " + (i + 1) + ": " + "EMPTY";
                uIGearEquipSlots[i].gameObject.SetActive(true);
            }

            else
            {
                GearSO gearToLoad = playerInventory.inventorySO.equippedGear[i];
                uIGearEquipSlots[i].gearEquipped = gearToLoad;
                uIGearEquipSlots[i].buttonTMP.text = "SLOT " + (i + 1) + ": " + gearToLoad.gearName;
                uIGearEquipSlots[i].gameObject.SetActive(true);
            }
        }

        FieldEvents.SetGridNavigationWrapAround(uIGearEquipSlotButtons, playerInventory.inventorySO.equipSlotsAvailable);
    }

    public void GearSlotSelected(UIGearEquipSlot gearEquipSlot)
    {
        gearEquipSlotSelected = gearEquipSlot;
        StartCoroutine(ShowGearInventoryCoroutine());
        CombatEvents.UpdateNarrator.Invoke("");
    }

    IEnumerator ShowGearInventoryCoroutine()
    {
        //playerInventory.LoadInventoryFromSO();
        Debug.Log("fix this");
        DisableAllInventorySlots();

        Debug.Log("fix this");
        //for (int i = 0; i < playerInventory.inventory.Count; i++)
        //{
        //    Gear gearToLoad = playerInventory.inventory[i].GetComponent<Gear>();
        //    inventorySlot[i].gear = gearToLoad;
        //    inventorySlot[i].itemName.text = gearToLoad.gearID;
        //    inventorySlot[i].itemQuantity.text = " x " + gearToLoad.quantityInInventory;
        //    SetTextAlpha(inventorySlot[i].itemName, gearToLoad.isCurrentlyEquipped ? 0.7f : 1f);
        //    SetTextAlpha(inventorySlot[i].itemQuantity, gearToLoad.isCurrentlyEquipped ? 0.7f : 1f);
        //    inventorySlot[i].gameObject.SetActive(true);
        //}

        equipNoneOption.Select();
        yield return new WaitForEndOfFrame();

        inventoryMenu.SetActive(true);
    }

    void DisableAllInventorySlots()
    {
        for (int i = 0; i < inventorySlot.Length; i++)
        {
            inventorySlot[i].gameObject.SetActive(false);
        }
    }

    void SetTextAlpha(TextMeshProUGUI textMeshProUGUI, float alpha)
    {
        textMeshProUGUI.color = Color.white;
        Color color = textMeshProUGUI.color;
        color.a = alpha;
        textMeshProUGUI.color = color;
    }

    public void equipNone()
    {
        if (gearEquipSlotSelected.gearEquipped == null)
        {
            gearSelectState.ResetStateGearSelect();
        }

        else
        {
            //playerInventory.UnequipGearFromSlot(gearEquipSlotSelected.gearEquipped);
            Debug.Log("fix this");
            combatManager.combatMenuManager.SetButtonNormalColor(gearEquipSlotSelected.GetComponent<Button>(), Color.white);
            gearEquipSlotSelected.gearEquipped = null;
            ApplyGearSelected();
        }
    }

    public void equipSelectedGear(InventorySlot inventorySlot)
    {
        if (!inventorySlot.gear.isCurrentlyEquipped) //if gear selected is already equipped do nothing
        {
            var geartoEquip = inventorySlot.gear;

            if (gearEquipSlotSelected.gearEquipped != null)
            {
                Debug.Log("fix this");
                //playerInventory.UnequipGearFromSlot(gearEquipSlotSelected.gearEquipped); //if a gear exists remove it before equipping a new one
            }

            Debug.Log("fix this"); 
            //playerInventory.EquipGearToSlot(geartoEquip, gearEquipSlotSelected.equipSlotNumber);
            combatManager.combatMenuManager.SetButtonNormalColor(gearEquipSlotSelected.GetComponent<Button>(), Color.white);

            ApplyGearSelected();
        }
    }

    void ApplyGearSelected()
    {
        combatManager.playerCombat.moveSelected = equipGearMove;
        inventoryMenu.SetActive(false);
        gearEquipSlotSelected = uIGearEquipSlots[0];
        combatManager.SetState(combatManager.applyMove);
    }
}

