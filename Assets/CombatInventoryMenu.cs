using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CombatInventoryMenu : MonoBehaviour
{
    public CombatManager combatManager;
    public PlayerInventory playerInventory;
    [SerializeField] EquippedGear equippedGear;
    public GearSelectUI gearSelectUI;
    public InventorySlot[] inventorySlot;
    [SerializeField] GameObject inventoryMenu;
    [SerializeField] Button inventorySlotOne;
    public int combatGearSlotSelected;
    public int InventorySlotNumberSelected;
    public Gear geartoEquip;
    [SerializeField] CautiousBasic equipGearMove; //just a generic move for the narrator and combat manager to process

    private void Start()
    {
        inventoryMenu.SetActive(false);
        var player = GameObject.Find("Player");

        playerInventory = player.GetComponentInChildren<PlayerInventory>();
        equippedGear = player.GetComponentInChildren<EquippedGear>();
    }

    public void GearSlotSelected(int gearSlot)

    {
        combatGearSlotSelected = gearSlot;
        StartCoroutine(ShowMenuCoroutine(0.1f));
        CombatEvents.UpdateNarrator.Invoke("");
    }

    IEnumerator ShowMenuCoroutine(float waitTime)
    {
        inventoryMenu.SetActive(true);

        yield return new WaitForSeconds(waitTime);

        for (int i = 0; i < playerInventory.inventory.Count; i++)

        {
            Gear gearToLoad = playerInventory.inventory[i].GetComponent<Gear>();
            inventorySlot[i].gear = gearToLoad;
            inventorySlot[i].textMeshProUGUI.text = gearToLoad.name;
        }

        inventorySlotOne.Select();
    }

    public void equipSelectedGear()

    {
        InventorySlotNumberSelected = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);

        equippedGear.equippedSlot[combatGearSlotSelected] = inventorySlot[InventorySlotNumberSelected].gear;
        gearSelectUI.UpdateGearDisplay(combatGearSlotSelected, inventorySlot[InventorySlotNumberSelected].gear.name);
        combatManager.selectedPlayerMove = equipGearMove;

        combatManager.SetState(combatManager.applyMove);
    }
}
