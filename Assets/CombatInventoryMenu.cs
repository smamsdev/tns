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
    public Gear geartoEquip;

    public InventorySlot[] inventorySlot;

    public GearEquipSlot[] gearEquipSlot;
    public GearEquipSlot combatGearSlotSelected;

    [SerializeField] Move equipGearMove; //player needs a move assigned to complete their turn, can put a nice animation in here maybe

    private void Start()
    {
        foreach (GearEquipSlot gearEquipSlot in gearEquipSlot)
        {
            gearEquipSlot.gameObject.SetActive(false);
        }

        var player = GameObject.Find("Player");
        playerInventory = player.GetComponentInChildren<PlayerInventory>();

        DisplayEquipSlots();
    }

    void DisplayEquipSlots()
    {
        inventoryMenu.SetActive(false); //hide inventory container for now

        for (int i = 0; i < playerInventory.inventorySO.equipSlotString.Count; i++)

            if (playerInventory.equippedSlot[i] == null)
            {
                gearEquipSlot[i].buttonTMP.text = "GEAR SLOT EMPTY";
                gearEquipSlot[i].gameObject.SetActive(true);
            }

            else
            {
                Gear gearToLoad = playerInventory.equippedSlot[i].GetComponent<Gear>();
                gearEquipSlot[i].gearEquipped = gearToLoad;
                gearEquipSlot[i].buttonTMP.text = gearToLoad.gearID;
                gearEquipSlot[i].gameObject.SetActive(true);
            }
    }

    public void GearSlotSelected(GearEquipSlot gearEquipSlot)
    {
        combatGearSlotSelected = gearEquipSlot;
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
            inventorySlot[i].itemQuantity.text = " x " + gearToLoad.quantityInInventory.ToString();
            inventorySlot[i].gameObject.SetActive(true);
        }

        inventorySlotOne.Select();
    }

    public void equipSelectedGear(InventorySlot inventorySlot)
    {
        combatGearSlotSelected.gearEquipped = inventorySlot.gear;
        playerInventory.inventorySO.inventoryString.Remove(inventorySlot.gear.name);
        playerInventory.inventorySO.equipSlotString[combatGearSlotSelected.equipSlotNumber] = inventorySlot.gear.name;
        playerInventory.equippedSlot[combatGearSlotSelected.equipSlotNumber] = inventorySlot.gear;
        gearSelectUI.UpdateGearDisplay(combatGearSlotSelected);
        combatManager.playerCombat.moveSelected = equipGearMove;
        inventoryMenu.SetActive(false);
        combatManager.SetState(combatManager.applyMove);
    }
}

