using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class InventorySO : ScriptableObject
{
    public List<string> inventoryString = new List<string>();
    public List<string> equipSlotString = new List<string>();
}