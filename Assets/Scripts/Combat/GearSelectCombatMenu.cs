using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GearSelectCombatMenu : MonoBehaviour
{
    public CombatManager combatManager;
    PlayerInventory playerInventory;
    public GameObject UICombatGearSlotPrefab, inventorySlotsParent;
    public GearSelectCombatState gearSelectCombatState;

    public List<UICombatGearSlot> gearSlots = new List<UICombatGearSlot>();
    List<Button> slotButtons = new List<Button>();
    public Dictionary<GearSO, UICombatGearSlot> gearToSlot = new Dictionary<GearSO, UICombatGearSlot>();

    public UICombatGearSlot UICombatGearSlotHighlighed;

    public bool isGearSlotsInitialized;

    public void InstantiateUIGearSlots()
    {
        if (isGearSlotsInitialized)
        { return; }

        isGearSlotsInitialized = true;

        playerInventory = combatManager.playerCombat.playerInventory;

        ClearSlots();

        foreach (GearSO gear in playerInventory.inventorySO.gearInventory)
        {
            if (!gearToSlot.ContainsKey(gear))
            {
                GameObject UICombatgearSlot = Instantiate(UICombatGearSlotPrefab);
                UICombatgearSlot.transform.SetParent(inventorySlotsParent.transform, false);

                UICombatGearSlot inventorySlot = UICombatgearSlot.GetComponent<UICombatGearSlot>();
                UICombatgearSlot.name = gear.gearID;
                inventorySlot.gearSO = gear;
                inventorySlot.combatMenuManager = combatManager.combatMenuManager;
                inventorySlot.gearSelectCombatMenu = this;
                inventorySlot.itemName.text = gear.gearName;
                inventorySlot.itemQuantity.text = "x" + gear.quantityInInventory;

                float alpha = gear.isCurrentlyEquipped ? 0.5f : 1f;
                combatManager.combatMenuManager.SetTextAlpha(inventorySlot.itemName, alpha);
                combatManager.combatMenuManager.SetTextAlpha(inventorySlot.itemQuantity, alpha);

                inventorySlot.button.onClick.AddListener(() => gearSelectCombatState.EquipSelectedGear(inventorySlot));
                gearSlots.Add(inventorySlot);
                slotButtons.Add(inventorySlot.button);
                gearToSlot[gear] = inventorySlot;
            }
        }

        FieldEvents.SetGridNavigationWrapAround(slotButtons, 5);
        combatManager.combatMenuManager.gearSelectMenuDefaultButton = gearSlots[0].button;
    }

    public void ClearSlots()
    {
        foreach (Transform child in inventorySlotsParent.transform)
        {
            Destroy(child.gameObject);
        }

        gearSlots.Clear();
        slotButtons.Clear();
        gearToSlot.Clear();
    }


}
