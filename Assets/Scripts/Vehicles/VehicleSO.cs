using UnityEngine;

public class VehicleSO : ScriptableObject
{
    [SerializeField] private string vehicleName;
    public string VehicleName => vehicleName;

    [SerializeField] private int vehicleCost;
    public int VehicleCost => vehicleCost;

    [SerializeField] private float vehicleSpeed;
    public float VehicleSpeed => vehicleSpeed;

    [SerializeField] private InventorySO vehicleInventory;
    public InventorySO VehicleInventory => vehicleInventory;

    [SerializeField] private GameObject vehiclePrefabGO;
    public GameObject VehiclePrefabGO => vehiclePrefabGO;
}