using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stairsReset : MonoBehaviour
{
    [SerializeField] GameObject ascend;
    [SerializeField] GameObject descend;


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")

        {
            this.GetComponent<EdgeCollider2D>().enabled = false;
            descend.SetActive(true);
            descend.GetComponent<BoxCollider2D>().enabled = true;
            ascend.GetComponent<BoxCollider2D>().enabled = true;
            this.GetComponent<BoxCollider2D>().enabled=false;

        }

    }
}
