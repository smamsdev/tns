using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public List<GearSO> shopGearInventory = new List<GearSO>();
    public ShopMenuManagerUI shopMenuManagerUI;

    private void OnEnable()
    {
        shopGearInventory.Sort((a, b) => a.name.CompareTo(b.name));
    }
}
