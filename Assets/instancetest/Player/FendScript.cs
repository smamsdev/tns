using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FendScript : MonoBehaviour
{

    TextMeshPro textMeshPro;
    [SerializeField] GameObject fendTextObj;

    public int fend = 0;

    private void OnEnable()
    {
        CombatEvents.UpdateFendDisplay += UpdateFendText;
        CombatEvents.ShowHideFendDisplay += ShowHideFendDisplay;
    }

    private void OnDisable()
    {
        CombatEvents.UpdateFendDisplay -= UpdateFendText;
        CombatEvents.ShowHideFendDisplay -= ShowHideFendDisplay;
    }

    void Awake()
    {
        textMeshPro = fendTextObj.GetComponent<TextMeshPro>();
    }

    void UpdateFendText(int value)
    {
        fend = value;
        textMeshPro.text = "Fend: " + fend.ToString();     
    }

    void ShowHideFendDisplay(bool on)

    {
        if (on && fend > 0 ){ fendTextObj.SetActive(true); }

        else { fendTextObj.SetActive(false); }
    }
}
