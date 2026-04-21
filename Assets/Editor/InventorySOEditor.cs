using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InventorySO), true)]
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
            EquipmentInstance equipmentInstanceToAdd = new EquipmentInstance(gearSOToAdd);
            bool test = inventorySO.AttemptAddGearToInventory(equipmentInstanceToAdd, false);
        }

        else
        {
            ConsumableInstance consumableInstanceToAdd = new ConsumableInstance(gearSOToAdd);
            inventorySO.AttemptAddGearToInventory(consumableInstanceToAdd, false);
        }
    }
}