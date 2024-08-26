using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    public LayerMask layerMask;  
    public float rayDistance = 0.10f;  

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector2 direction = FieldEvents.lookDirection;
            Vector2 startPosition = new Vector2(transform.position.x, transform.position.y);

            RaycastHit2D hit = Physics2D.Raycast(startPosition, direction, rayDistance, layerMask);

            Debug.DrawRay(startPosition, direction * rayDistance, Color.green, 1.0f);

            if (hit.collider != null)
            {
                FieldEvents.PlayerRayCastHit?.Invoke(hit);
            }
        }
    }
}