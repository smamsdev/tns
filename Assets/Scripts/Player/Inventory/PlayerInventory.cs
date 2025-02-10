using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public InventorySO inventorySO;
    [SerializeField] Transform GearParent;

    public List<Gear> inventory = new List<Gear>();


    private void Start()
    {
        LoadInventoryFromSO();
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
        for (int i = 0; i < inventorySO.inventoryString.Count; i++)
        {
            Transform gearTransform = GearParent.Find(inventorySO.inventoryString[i]);

            if (gearTransform != null)
            {
                Gear gearToLoad = gearTransform.GetComponent<Gear>();
                gearToLoad.quantityInInventory++;

                if (!inventory.Contains(gearToLoad))
                {
                    inventory.Add(gearToLoad);
                }
            }
            else
            {
                Debug.LogWarning("Gear not found in scene: " + inventorySO.inventoryString[i]);
            }
        }
    }
}