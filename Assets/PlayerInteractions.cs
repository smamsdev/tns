using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    public RaycastHit2D raycastHit2D;
    public LayerMask layer_mask;

    public Gear testGear;
    public PlayerInventory inventory;
    public InventorySO inventorySO;

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
                raycastHit2D = Physics2D.BoxCast(this.transform.position, new Vector2(0.2f, 0.5f), 0f, FieldEvents.lookDirection, 0.10f, layer_mask.value);

            if (raycastHit2D.collider != null)
            {
                FieldEvents.PlayerRayCastHit?.Invoke(raycastHit2D);
            }

        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!inventorySO.inventoryString.Contains(testGear.gearID))

            {
                inventorySO.inventoryString.Add(testGear.gearID);
            }

            if (!inventory.inventory.Contains(testGear))

            {
                inventory.inventory.Add(testGear);
            }
        }
    }
}
