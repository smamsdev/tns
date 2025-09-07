using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GearSelectCombatMenu : MonoBehaviour
{
    public CombatManager combatManager;
    PlayerInventory playerInventory;
    public GameObject UICombatGearSlotPrefab, inventorySlotsParent;

    public List<UICombatGearSlot> gearSlots = new List<UICombatGearSlot>();
    List<Button> slotButtons = new List<Button>();
    public Dictionary<GearSO, UICombatGearSlot> gearToSlot = new Dictionary<GearSO, UICombatGearSlot>();

    public void InstantiateUIGearSlots()
    {
        playerInventory = combatManager.playerCombat.playerInventory;

        gearSlots.Clear();
        slotButtons.Clear();
        gearToSlot.Clear();

        foreach (GearSO gear in playerInventory.inventorySO.gearInventory)
        {
            if (!gearToSlot.ContainsKey(gear))
            {
                GameObject UICombatgearSlot = Instantiate(UICombatGearSlotPrefab);
                UICombatgearSlot.transform.SetParent(inventorySlotsParent.transform, false);

                UICombatGearSlot inventorySlot = UICombatgearSlot.GetComponent<UICombatGearSlot>();
                UICombatgearSlot.name = gear.gearID;
                inventorySlot.gear = gear;
                inventorySlot.combatMenuManager = combatManager.combatMenuManager;
                inventorySlot.itemName.text = gear.gearName;
                inventorySlot.itemQuantity.text = "x" + gear.quantityInInventory;

                float alpha = gear.isCurrentlyEquipped ? 0.5f : 1f;
                combatManager.combatMenuManager.SetTextAlpha(inventorySlot.itemName, alpha);
                combatManager.combatMenuManager.SetTextAlpha(inventorySlot.itemQuantity, alpha);

                //inventorySlot.button.onClick.AddListener(() => InventorySlotSelected(inventorySlot));
                gearSlots.Add(inventorySlot);
                slotButtons.Add(inventorySlot.button);
                gearToSlot[gear] = inventorySlot;
            }
        }

        FieldEvents.SetGridNavigationWrapAround(slotButtons, 5);
        gearSlots[0].button.Select();
        gearSlots[0].DefaultButtonSelected();
    }


}
