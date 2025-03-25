using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public InventorySO inventorySO;
    public Transform GearParent;
    public List<Gear> inventory = new List<Gear>();
    public List <Gear> equippedSlots = new List<Gear>();

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
        equippedSlots.Clear();

        for (int i = 0; i < inventorySO.equipSlotString.Count; i++)
        {
            if (inventorySO.equipSlotString[i] == "")

            {
                equippedSlots.Add(null);
            }


            else
            {
                Transform gearTransform = GearParent.Find(inventorySO.equipSlotString[i]);

                if (gearTransform != null)
                {
                    Gear gearToLoad = gearTransform.GetComponent<Gear>();

                    equippedSlots.Add(gearToLoad);
                }
                else
                {
                    Debug.LogWarning("Gear not found in scene: " + inventorySO.inventoryString[i]);
                }
            }   
        }
    }
}