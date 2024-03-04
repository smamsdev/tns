using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CombatInventoryMenu : MonoBehaviour
{
    public CombatManagerV3 combatManagerV3;
    public PlayerInventory playerInventory;
    [SerializeField] CombatInventorySlot[] inventorySlot;
    [SerializeField] GameObject inventoryMenu;

    private void Start()
    {
        inventoryMenu.SetActive(false);
    }

    public void ShowMenu()

    {
        StartCoroutine(ShowMenuCoroutine(0.1f));
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
    }

}
