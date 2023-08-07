using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class CombatNarrator : MonoBehaviour
{

    TextMeshProUGUI textMeshProUGUI;
    [SerializeField] GameObject combatNarratorTextObj;

    private void Awake()
    {
        textMeshProUGUI = combatNarratorTextObj.GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        CombatEvents.UpdateNarrator += UpdateNarrator;
    }

    private void OnDisable()
    {
        CombatEvents.UpdateNarrator -= UpdateNarrator;

    }

      public void UpdateNarrator(string narratorText)

    {
        textMeshProUGUI.text = narratorText;

    }
}
