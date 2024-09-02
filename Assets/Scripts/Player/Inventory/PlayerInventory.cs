using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public InventorySO inventorySO;

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
         //   inventory.Add(GameObject.Find(inventorySO.inventoryString[i]).GetComponent<Gear>()); ;
        }
    }


}