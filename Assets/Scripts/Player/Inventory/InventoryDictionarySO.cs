using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[CreateAssetMenu]

public class InventoryDictionarySO : ScriptableObject
{
    public Dictionary<String, int> inventory = new Dictionary<String, int>();
}

