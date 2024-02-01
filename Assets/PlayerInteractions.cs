using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    public RaycastHit2D raycastHit2D;
    public LayerMask layer_mask;

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {

                // this.GetComponent<BoxCollider2D>().enabled = false; to block your own collider hitting the ray, might not need this

                raycastHit2D = Physics2D.BoxCast(this.transform.position, new Vector2(0.2f, 0.5f), 0f, FieldEvents.lookDirection, 0.10f, layer_mask.value);

            if (raycastHit2D.collider != null)
            {
                FieldEvents.PlayerRayCastHit?.Invoke(raycastHit2D);
            }

            //   this.GetComponent<BoxCollider2D>().enabled = true;
        }
    }
}
