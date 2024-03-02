using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CombatInventoryMenu : MonoBehaviour
{
    public CombatManagerV3 combatManagerV3;
    public PlayerInventory playerInventory;
    [SerializeField] GameObject inventorySlotPrefab;
    GameObject inventorySlot;
    [SerializeField] GameObject background;

    public Gear gearToLoad;

    private void OnEnable()
    {
        CombatEvents.BattleMode += init;
    }

    private void OnDisable()
    {
        CombatEvents.BattleMode -= init;
    }

    void init(bool on)

    {
        StartCoroutine(LoadInventoryItemsToMenu(0.1f));
    }

    IEnumerator LoadInventoryItemsToMenu(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        Debug.Log(playerInventory.inventory.Count);


        for (int i = 0; i < playerInventory.inventory.Count; i++)

        {
            inventorySlot = Instantiate(inventorySlotPrefab, background.transform);

            gearToLoad = playerInventory.inventory[i].GetComponent<Gear>();

            CombatInventorySlot combatInventorySlot = inventorySlot.GetComponent<CombatInventorySlot>();
            combatInventorySlot.gear = gearToLoad;
            combatInventorySlot.textMeshProUGUI.text = gearToLoad.name;
            combatInventorySlot.name = gearToLoad.name + "" + "CombatMenuItem";

        }
    }

}
