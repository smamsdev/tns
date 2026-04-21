using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerInventorySO))]
public class PlayerInventorySOEditor : InventorySOEditor
{
    public override void OnInspectorGUI()
    {
        PlayerInventorySO playerInventorySO = (PlayerInventorySO)target;

        if (GUILayout.Button("Create empty Equip slot"))
        {
            var emptyInstance = new GearInstance();
            playerInventorySO.gearInstanceEquipped.Add(emptyInstance);
        }

        if (GUILayout.Button("Equip inventory Gear to specific Equip slot"))
        {
            var instanceToEquip =
                playerInventorySO.gearInstanceInventory[playerInventorySO.debugInventorySlotToEquip];

            playerInventorySO.EquipGearToSlot(
                instanceToEquip,
                playerInventorySO.debugEquipSlotToAddTo
            );
        }

        base.OnInspectorGUI();
    }
}