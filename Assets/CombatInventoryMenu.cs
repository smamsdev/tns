using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CombatInventoryMenu : MonoBehaviour
{
    public CombatManager combatManager;
    public PlayerInventory playerInventory;
    public GearSelectUI gearSelectUI;
    [SerializeField] GameObject inventoryMenu;
    [SerializeField] Button inventorySlotOne;

    public int InventorySlotNumberSelected;

    public InventorySlot[] inventorySlot;

    public GearEquipSlot[] gearEquipSlot;
    public GearEquipSlot gearEquipSlotSelected;

    [SerializeField] Move equipGearMove; //player needs a move assigned to complete their turn, can put a nice animation in here maybe

    void DisplayEquipSlots()
    {
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

        inventoryMenu.SetActive(true);

        yield return new WaitForSeconds(waitTime);

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

        inventorySlotOne.Select();
    }

    public void equipSelectedGear(InventorySlot inventorySlot)
    {
        var geartoEquip = inventorySlot.gear;

        if (gearEquipSlotSelected.gearEquipped != null)
        {
            playerInventory.UnequipGearFromSlot(gearEquipSlotSelected.gearEquipped);
        }

        playerInventory.RemoveGearFromInventory(geartoEquip);
        playerInventory.EquipGearToSlot(geartoEquip, gearEquipSlotSelected.equipSlotNumber);

        //update display
        combatManager.playerCombat.moveSelected = equipGearMove;
        inventoryMenu.SetActive(false);

        combatManager.SetState(combatManager.applyMove);
    }

}

