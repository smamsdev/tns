using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CombatGearMenu : MonoBehaviour
{
    public CombatManager combatManager;
    public PlayerInventory playerInventory;
    public GearSelectState gearSelectState;
    [SerializeField] GameObject inventoryMenu;
    [SerializeField] Button equipNoneOption;

    public int InventorySlotNumberSelected;

    public InventorySlot[] inventorySlot;

    public uiGearSlot[] gearEquipSlot;
    public uiGearSlot gearEquipSlotSelected;

    [SerializeField] Move equipGearMove; //player needs a move assigned to complete their turn, can put a nice animation in here maybe

    public void DisplayEquipSlots()
    {
        if (gearEquipSlotSelected == null)
        {
            gearEquipSlotSelected = gearEquipSlot[0];
        }

        foreach (uiGearSlot gearEquipSlot in gearEquipSlot)
        {
            gearEquipSlot.gameObject.SetActive(false);
        }

        var player = GameObject.Find("Player");
        playerInventory = player.GetComponentInChildren<PlayerInventory>();

        inventoryMenu.SetActive(false); //hide inventory container for now

        Debug.Log("fix");
       // for (int i = 0; i < playerInventory.inventorySO.equipSlotString.Count; i++)
       //
       //     if (playerInventory.equippedInventory[i] == null)
       //     {
       //         gearEquipSlot[i].buttonTMP.text = "SLOT " + (i+1) + " EMPTY";
       //         gearEquipSlot[i].gameObject.SetActive(true);
       //     }
       //
       //     else
       //     {
       //         Gear gearToLoad = playerInventory.equippedInventory[i].GetComponent<Gear>();
       //         gearEquipSlot[i].gearEquipped = gearToLoad;
       //         gearEquipSlot[i].buttonTMP.text = gearToLoad.gearID;
       //         gearEquipSlot[i].gameObject.SetActive(true);
       //     }

        SetNavigationWrapAround();
        gearEquipSlotSelected.GetComponent<Button>().Select();
    }

    void SetNavigationWrapAround()
    {
        Debug.Log("fix");
        //int buttonCount = playerInventory.inventorySO.equipSlotString.Count;
        //
        //if (buttonCount > 1)
        //{
        //    for (int i = 0; i < buttonCount; i++)
        //    {
        //        Button button = gearEquipSlot[i].GetComponent<Button>();
        //        Navigation newNav = button.navigation;
        //
        //        // Default navigation: each button's up/down points to the adjacent buttons
        //        newNav.selectOnUp = gearEquipSlot[(i - 1 + buttonCount) % buttonCount].GetComponent<Button>();  // Wrap around up
        //        newNav.selectOnDown = gearEquipSlot[(i + 1) % buttonCount].GetComponent<Button>();  // Wrap around down
        //
        //        button.navigation = newNav;
        //    }
        //}
    }

    public void GearSlotSelected(uiGearSlot gearEquipSlot)
    {
        gearEquipSlotSelected = gearEquipSlot;
        StartCoroutine(ShowGearInventoryCoroutine());
        CombatEvents.UpdateNarrator.Invoke("");
    }

    IEnumerator ShowGearInventoryCoroutine()
    {
        //playerInventory.LoadInventoryFromSO();
        Debug.Log("fix this");
        DisableAllInventorySlots();

        Debug.Log("fix this");
        //for (int i = 0; i < playerInventory.inventory.Count; i++)
        //{
        //    Gear gearToLoad = playerInventory.inventory[i].GetComponent<Gear>();
        //    inventorySlot[i].gear = gearToLoad;
        //    inventorySlot[i].itemName.text = gearToLoad.gearID;
        //    inventorySlot[i].itemQuantity.text = " x " + gearToLoad.quantityInInventory;
        //    SetTextAlpha(inventorySlot[i].itemName, gearToLoad.isCurrentlyEquipped ? 0.7f : 1f);
        //    SetTextAlpha(inventorySlot[i].itemQuantity, gearToLoad.isCurrentlyEquipped ? 0.7f : 1f);
        //    inventorySlot[i].gameObject.SetActive(true);
        //}

        equipNoneOption.Select();
        yield return new WaitForEndOfFrame();

        inventoryMenu.SetActive(true);
    }

    void DisableAllInventorySlots()
    {
        for (int i = 0; i < inventorySlot.Length; i++)
        {
            inventorySlot[i].gameObject.SetActive(false);
        }
    }

    void SetTextAlpha(TextMeshProUGUI textMeshProUGUI, float alpha)
    {
        textMeshProUGUI.color = Color.white;
        Color color = textMeshProUGUI.color;
        color.a = alpha;
        textMeshProUGUI.color = color;
    }

    public void equipNone()
    {
        if (gearEquipSlotSelected.gearEquipped == null)
        {
            gearSelectState.ResetStateGearSelect();
        }

        else
        {
            //playerInventory.UnequipGearFromSlot(gearEquipSlotSelected.gearEquipped);
            Debug.Log("fix this");
            combatManager.combatMenuManager.SetButtonNormalColor(gearEquipSlotSelected.GetComponent<Button>(), Color.white);
            gearEquipSlotSelected.gearEquipped = null;
            ApplyGearSelected();
        }
    }

    public void equipSelectedGear(InventorySlot inventorySlot)
    {
        if (!inventorySlot.gear.isCurrentlyEquipped) //if gear selected is already equipped do nothing
        {
            var geartoEquip = inventorySlot.gear;

            if (gearEquipSlotSelected.gearEquipped != null)
            {
                Debug.Log("fix this");
                //playerInventory.UnequipGearFromSlot(gearEquipSlotSelected.gearEquipped); //if a gear exists remove it before equipping a new one
            }

            Debug.Log("fix this"); 
            //playerInventory.EquipGearToSlot(geartoEquip, gearEquipSlotSelected.equipSlotNumber);
            combatManager.combatMenuManager.SetButtonNormalColor(gearEquipSlotSelected.GetComponent<Button>(), Color.white);

            ApplyGearSelected();
        }
    }

    void ApplyGearSelected()
    {
        combatManager.playerCombat.moveSelected = equipGearMove;
        inventoryMenu.SetActive(false);
        gearEquipSlotSelected = gearEquipSlot[0];
        combatManager.SetState(combatManager.applyMove);
    }
}

