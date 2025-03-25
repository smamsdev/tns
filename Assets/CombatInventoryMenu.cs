using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CombatInventoryMenu : MonoBehaviour
{
    public CombatManager combatManager;
    public PlayerInventory playerInventory;
    public GearSelectUI gearSelectUI;
    [SerializeField] GameObject inventoryMenu;
    [SerializeField] Button inventorySlotOne;
    public int combatGearSlotSelected;
    public int InventorySlotNumberSelected;
    public Gear geartoEquip;
    [SerializeField] CautiousBasic equipGearMove; //just a generic move for the narrator and combat manager to process.>WTFFFFF
    public InventorySlot[] inventorySlot;

    public GearEquipSlot[] gearEquipSlot;

    private void Start()
    {
        foreach (GearEquipSlot gearEquipSlot in gearEquipSlot)
        {
            gearEquipSlot.gameObject.SetActive(false);
        }

        var player = GameObject.Find("Player");
        playerInventory = player.GetComponentInChildren<PlayerInventory>();

        DisplayEquipSlots();
    }

    void DisplayEquipSlots()
    {
        inventoryMenu.SetActive(false); //hide inventory container for now

        for (int i = 0; i < playerInventory.inventorySO.equipSlotString.Count; i++)

            if (playerInventory.equippedSlots[i] == null)
            {
                gearEquipSlot[i].buttonTMP.text = "GEAR SLOT EMPTY";
                gearEquipSlot[i].gameObject.SetActive(true);
            }

            else
            {
                Gear gearToLoad = playerInventory.equippedSlots[i].GetComponent<Gear>();
                gearEquipSlot[i].gearEquipped = gearToLoad;
                gearEquipSlot[i].buttonTMP.text = gearToLoad.gearID;
                gearEquipSlot[i].gameObject.SetActive(true);
            }
    }

    public void GearSlotSelected(GearEquipSlot gearEquipSlot)

    {
        //combatGearSlotSelected = gearSlot;
        StartCoroutine(ShowMenuCoroutine(0.1f));
        CombatEvents.UpdateNarrator.Invoke("");
    }

    IEnumerator ShowMenuCoroutine(float waitTime)
    {
        inventoryMenu.SetActive(true);

        yield return new WaitForSeconds(waitTime);

        for (int i = 0; i < playerInventory.inventory.Count; i++)

        {
            Gear gearToLoad = playerInventory.inventory[i];
            inventorySlot[i].gear = gearToLoad;
            inventorySlot[i].itemName.text = gearToLoad.gearID;
            inventorySlot[i].itemQuantity.text = gearToLoad.quantityInInventory.ToString();
        }

        inventorySlotOne.Select();
    }

    // public void equipSelectedGear()
    // {
    //     InventorySlotNumberSelected = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
    //
    //     //equippedGear.equippedSlot[combatGearSlotSelected] = inventorySlot[InventorySlotNumberSelected].gear;
    //     gearSelectUI.UpdateGearDisplay(combatGearSlotSelected, inventorySlot[InventorySlotNumberSelected].gear.name);
    //     combatManager.playerCombat.moveSelected = equipGearMove;
    //
    //     combatManager.SetState(combatManager.applyMove);
    // }
}

