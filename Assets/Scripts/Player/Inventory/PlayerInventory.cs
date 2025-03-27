using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public InventorySO inventorySO;
    public List<Gear> inventory = new List<Gear>();
    public List <Gear> equippedSlot = new List<Gear>();

    public Transform GearParent;

    private void Start()
    {
        LoadInventoryFromSO();
        LoadEquippedSlotsFromSO();
    }

    private void SaveInventory()
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if (!inventorySO.inventoryString.Contains(inventory[i].gearID))
                inventorySO.inventoryString.Add(inventory[i].gearID);
        }
    }

    public void LoadInventoryFromSO()
    {
        inventorySO.inventoryString.Sort();
        inventory.Clear();

        for (int i = 0; i < inventorySO.inventoryString.Count; i++)
        {
            Transform gearTransform = GearParent.Find(inventorySO.inventoryString[i]);

            if (gearTransform != null)
            {
                Gear gearToLoad = gearTransform.GetComponent<Gear>();
                gearToLoad.quantityInInventory++;

                if (!inventory.Contains(gearToLoad))
                {
                    gearToLoad.quantityInInventory = 1;
                    inventory.Add(gearToLoad);
                }
            }
            else
            {
                Debug.LogWarning("Gear not found in scene: " + inventorySO.inventoryString[i]);
            }
        }
    }

    public void LoadEquippedSlotsFromSO()
    {
        equippedSlot.Clear();

        for (int i = 0; i < inventorySO.equipSlotString.Count; i++)
        {
            if (inventorySO.equipSlotString[i] == "")

            {
                equippedSlot.Add(null);
            }


            else
            {
                Transform gearTransform = GearParent.Find(inventorySO.equipSlotString[i]);

                if (gearTransform != null)
                {
                    Gear gearToLoad = gearTransform.GetComponent<Gear>();

                    equippedSlot.Add(gearToLoad);
                }
                else
                {
                    Debug.LogWarning("Gear not found in scene: " + inventorySO.inventoryString[i]);
                }
            }   
        }
    }

    public void AddGearToInventory(Gear gear)
    {
        inventorySO.inventoryString.Add(gear.name);
        inventorySO.inventoryString.Sort();
        inventory.Add(gear);
    }

    public void RemoveGearFromInventory(Gear gear)
    {
        inventorySO.inventoryString.Remove(gear.name);
        inventory.Remove(gear);
    }

    public void EquipGearToSlot(Gear gearToEquip, int slotNumber)
    {
        inventorySO.equipSlotString[slotNumber] = gearToEquip.name;
        equippedSlot[slotNumber] = gearToEquip;
    }

    public void UnequipGearFromSlot(Gear gearToUnequip, int slotNumber)
    {
        inventorySO.equipSlotString[slotNumber] = null;
        equippedSlot[slotNumber] = null;

        AddGearToInventory(gearToUnequip);
    }
}