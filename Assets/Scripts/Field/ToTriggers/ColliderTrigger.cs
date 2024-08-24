using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")

        {
            FieldEvents.HasCompleted.Invoke(this.gameObject);
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
