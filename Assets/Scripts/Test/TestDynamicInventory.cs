using System.Collections.Generic;
using UnityEngine;

public class TestDynamicInventory : MonoBehaviour
{
    // Runtime inventory stores clones of GearSO items
    public List<GearSO> gearInventory = new List<GearSO>();

    // Add a piece of gear to inventory
    public void AddGear(GearSO gearTemplate)
    {
        if (gearTemplate is ConsumbableSO consumableTemplate)
        {
            // Try to find an existing stack
            var existing = gearInventory.Find(g =>
                g is ConsumbableSO c && c.name == consumableTemplate.name);

            if (existing != null)
            {
                ((ConsumbableSO)existing).quantityAvailable++;
            }
            else
            {
                var clone = Instantiate(consumableTemplate);
                clone.quantityAvailable = 1;
                gearInventory.Add(clone);
            }
        }
        else // EquipmentSO or other GearSO
        {
            var clone = Instantiate(gearTemplate);
            gearInventory.Add(clone);
        }

        // Optional: sort by name
        gearInventory.Sort((a, b) => a.name.CompareTo(b.name));
    }

    // Remove gear from inventory (by instance)
    public void RemoveGear(GearSO gearInstance)
    {
        if (gearInventory.Contains(gearInstance))
        {
            gearInventory.Remove(gearInstance);
        }
    }

    // Get string for UI (stack or %)
    public string GetItemQuantityText(GearSO gear)
    {
        if (gear is EquipmentSO eq)
            return $": {eq.Potential}%";
        else if (gear is ConsumbableSO c)
            return $"x {c.quantityAvailable}";
        else
            return "";
    }
}
