using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CombatInventoryMenu : MonoBehaviour
{
    public CombatManagerV3 combatManagerV3;
    public PlayerInventory playerInventory;
    [SerializeField] CombatInventorySlot[] inventorySlot;
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

        for (int i = 0; i < playerInventory.inventory.Count; i++)

        {
            gearToLoad = playerInventory.inventory[i].GetComponent<Gear>();
            inventorySlot[i].gear = gearToLoad;
            inventorySlot[i].textMeshProUGUI.text = gearToLoad.name;
        }
    }

}
