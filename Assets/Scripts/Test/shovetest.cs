using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shovetest : MonoBehaviour
{
    void FixedUpdate()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Vector2 shoveDirection = new Vector2(1111f, 111f);
        float shoveForce = 5f;
        rb.AddForce(shoveDirection * shoveForce, ForceMode2D.Impulse);
    }

}
