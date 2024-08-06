using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetStairs : MonoBehaviour
{
    [SerializeField] GameObject descend;

    private void Start()
    {
        this.GetComponent<BoxCollider2D>().enabled = false;
    }

    private void OnTriggerEnter2D()

    {
        Debug.Log("reset");
        descend.GetComponent<DescendStairs>().bottomOfStairs.SetActive(false);
        this.GetComponent<BoxCollider2D>().enabled = false;
        descend.GetComponent<DescendStairs>().GetComponent<BoxCollider2D>().enabled = true;
    }

}
