using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LambdasTest : MonoBehaviour
{
    void Start()
    {
        // Define and use the lambda expression
        ExecuteLambda((x, y) =>
        {
            int result = x + y;
            Debug.Log("The sum is: " + result);
        }, 5, 3);
    }

    // Method that accepts a lambda as a parameter
    void ExecuteLambda(System.Action<int, int> lambda, int a, int b)
    {
        lambda(a, b);
    }
}