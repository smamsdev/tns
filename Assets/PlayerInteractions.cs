using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    public LayerMask layerMask;  
    public float rayDistance;  
    public PlayerMovementScript playerMovementScript;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !FieldEvents.movementLocked)
        {
            Vector2 direction = playerMovementScript.lookDirection;
            Vector2 startPosition = new Vector2(transform.position.x, transform.position.y);

            RaycastHit2D hit = Physics2D.Raycast(startPosition, direction, rayDistance, layerMask);

            Debug.DrawRay(startPosition, direction * rayDistance, Color.green, 1.0f);

            if (hit.collider != null)
            {
                FieldEvents.PlayerRayCastHit?.Invoke(hit);
            }

           //if (hit.collider == null)
           //{
           //    Debug.Log("null");
           //}
        }
    }
}