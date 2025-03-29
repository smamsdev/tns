using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GearSelectMenu : MonoBehaviour
{
    public CombatManager combatManager;
    public PlayerInventory playerInventory;
    public GearSelectUI gearSelectUI;
    public GearSelectState gearSelectState;
    [SerializeField] GameObject inventoryMenu;
    [SerializeField] Button equipNoneOption;

    public int InventorySlotNumberSelected;

    public InventorySlot[] inventorySlot;

    public GearEquipSlot[] gearEquipSlot;
    public GearEquipSlot gearEquipSlotSelected;

    [SerializeField] Move equipGearMove; //player needs a move assigned to complete their turn, can put a nice animation in here maybe

    public void DisplayEquipSlots()
    {
        if (gearEquipSlotSelected == null)
        {
            gearEquipSlotSelected = gearEquipSlot[0];
        }

        foreach (GearEquipSlot gearEquipSlot in gearEquipSlot)
        {
            gearEquipSlot.gameObject.SetActive(false);
        }

        var player = GameObject.Find("Player");
        playerInventory = player.GetComponentInChildren<PlayerInventory>();

        inventoryMenu.SetActive(false); //hide inventory container for now

        for (int i = 0; i < playerInventory.inventorySO.equipSlotString.Count; i++)

            if (playerInventory.equippedInventory[i] == null)
            {
                gearEquipSlot[i].buttonTMP.text = "GEAR SLOT EMPTY";
                gearEquipSlot[i].gameObject.SetActive(true);
            }

            else
            {
                Gear gearToLoad = playerInventory.equippedInventory[i].GetComponent<Gear>();
                gearEquipSlot[i].gearEquipped = gearToLoad;
                gearEquipSlot[i].buttonTMP.text = gearToLoad.gearID;
                gearEquipSlot[i].gameObject.SetActive(true);
            }

        gearEquipSlotSelected.GetComponent<Button>().Select();
    }

    public void GearSlotSelected(GearEquipSlot gearEquipSlot)
    {
        gearEquipSlotSelected = gearEquipSlot;

        StartCoroutine(ShowMenuCoroutine(0.1f));
        CombatEvents.UpdateNarrator.Invoke("");
    }

    IEnumerator ShowMenuCoroutine(float waitTime)
    {
        playerInventory.LoadInventoryFromSO();

        foreach (InventorySlot inventorySlot in inventorySlot)
        {
            inventorySlot.gameObject.SetActive(false);
        }


        for (int i = 0; i < playerInventory.inventory.Count; i++)

        {
            Gear gearToLoad = playerInventory.inventory[i];
            inventorySlot[i].gear = gearToLoad;
            inventorySlot[i].itemName.text = gearToLoad.gearID;

            inventorySlot[i].gear = gearToLoad;
            inventorySlot[i].itemName.text = gearToLoad.gearID;
            inventorySlot[i].itemQuantity.text = " x " + gearToLoad.quantityInInventory;
            inventorySlot[i].gameObject.SetActive(true);

            inventorySlot[i].gameObject.SetActive(true);
        }

        equipNoneOption.Select();
        yield return new WaitForSeconds(waitTime);
        inventoryMenu.SetActive(true);
    }

    public void equipNone()
    {
        if (gearEquipSlotSelected.gearEquipped == null)
        {
            gearSelectState.ResetStateGearSelect();
        }

        else
        {
            playerInventory.UnequipGearFromSlot(gearEquipSlotSelected.gearEquipped, gearEquipSlotSelected.equipSlotNumber);
            gearEquipSlotSelected.gearEquipped = null;
            ApplyGearSelected();
        }
    }

    public void equipSelectedGear(InventorySlot inventorySlot)
    {
        var geartoEquip = inventorySlot.gear;

        if (gearEquipSlotSelected.gearEquipped != null)
        {
            playerInventory.UnequipGearFromSlot(gearEquipSlotSelected.gearEquipped, gearEquipSlotSelected.equipSlotNumber);
        }

        playerInventory.RemoveGearFromInventory(geartoEquip);
        playerInventory.EquipGearToSlot(geartoEquip, gearEquipSlotSelected.equipSlotNumber);

        ApplyGearSelected();
    }

    void ApplyGearSelected()
    {
        combatManager.playerCombat.moveSelected = equipGearMove;
        inventoryMenu.SetActive(false);
        gearEquipSlotSelected = gearEquipSlot[0];
        combatManager.SetState(combatManager.applyMove);
    }
}

