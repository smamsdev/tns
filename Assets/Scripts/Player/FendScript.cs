using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FendScript : MonoBehaviour
{

    TextMeshPro textMeshPro;
    [SerializeField] GameObject fendTextObj;
    [SerializeField] PlayerMoveManagerSO playerMoveManager;

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
        if (value <= 0) { textMeshPro.text = "SMASHED!"; }

        else 
        {
            fend = value;
            textMeshPro.text = "Fend: " + fend.ToString();
        }
    }

    void ShowHideFendDisplay(bool on)

    {
        if (on && (playerMoveManager.firstMoveIs==2 || playerMoveManager.secondMoveIs == 2))
      
        { 
            fendTextObj.SetActive(true);
        }

        else 
        { 
            fendTextObj.SetActive(false); 
        }
    }
}
