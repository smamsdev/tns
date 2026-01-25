using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InventorySO))]
public class InventorySOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        InventorySO inventorySO = (InventorySO)target;

        if (GUILayout.Button("Add gearSO as Instance"))
        {
            AddNewGearAsInstance(inventorySO);
        }

        DrawDefaultInspector();
    }

    private void AddNewGearAsInstance(InventorySO inventorySO)
    {
        var gearSOToAdd = inventorySO.debugGearToAddAsInstance;

        if (gearSOToAdd is EquipmentSO)
        {
            EquipmentInstance equipmentInstanceToAdd = new EquipmentInstance();
            equipmentInstanceToAdd.gearSO = gearSOToAdd;
            inventorySO.gearInstanceInventory.Add(equipmentInstanceToAdd);
        }

        else

        {
            ConsumableInstance consumableInstanceToAdd = new ConsumableInstance();
            consumableInstanceToAdd.gearSO = gearSOToAdd;
            inventorySO.gearInstanceInventory.Add(consumableInstanceToAdd);
        }
    }
}