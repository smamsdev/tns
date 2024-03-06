using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CombatInventoryMenu : MonoBehaviour
{
    public CombatManagerV3 combatManagerV3;
    public PlayerInventory playerInventory;
    [SerializeField] EquippedGear equippedGear;
    [SerializeField] EquippedGearDisplayUI equippedGearDisplayUI;
    public CombatInventorySlot[] inventorySlot;
    [SerializeField] GameObject inventoryMenu;
    [SerializeField] Button inventorySlotOne;
    public int combatGearSlotSelected;
    public int InventorySlotNumberSelected;
    public Gear geartoEquip;

    private void Start()
    {
        inventoryMenu.SetActive(false);
    }

    public void ShowInventoryMenu()

    {
        combatGearSlotSelected = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
        StartCoroutine(ShowMenuCoroutine(0.1f));
        equippedGearDisplayUI.enabled = false;
        CombatEvents.UpdateNarrator("");

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
        equippedGearDisplayUI.enabled = true;
        InventorySlotNumberSelected = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);

        equippedGear.equippedSlot[combatGearSlotSelected] = inventorySlot[InventorySlotNumberSelected].gear;
        equippedGearDisplayUI.UpdateGearDisplay(combatGearSlotSelected, inventorySlot[InventorySlotNumberSelected].gear.name);

        inventoryMenu.SetActive(false); 

        combatManagerV3.SetBattleStateApplyPlayerMove();
    }
}
