using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InventorySO), true)]
public class InventorySOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        InventorySO inventorySO = (InventorySO)target;

        if (GUILayout.Button("Add empty Inventory Slot"))
        {
            GearInstance newEmptyInstance = new GearInstance();
            inventorySO.gearInstanceInventory.Add(newEmptyInstance);
        }

        if (GUILayout.Button("Add gearSO as Instance"))
        {
            AddNewGearAsInstance(inventorySO);
        }

        DrawDefaultInspector();
    }

    private void AddNewGearAsInstance(InventorySO inventorySO)
    {
        var gearSOToAdd = inventorySO.debugGearToAddAsInstance;
        bool test;

        if (gearSOToAdd is EquipmentSO)
        {
            EquipmentInstance equipmentInstanceToAdd = new EquipmentInstance(gearSOToAdd);
            test = inventorySO.AttemptAddGearToInventory(equipmentInstanceToAdd, false);
        }

        else
        {
            ConsumableInstance consumableInstanceToAdd = new ConsumableInstance(gearSOToAdd);
            test = inventorySO.AttemptAddGearToInventory(consumableInstanceToAdd, false);
        }

        if (!test)
            Debug.Log("unable to add new Gear Instance, no slots available?");
    }
}