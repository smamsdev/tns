using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerDamageTakenDisplay : MonoBehaviour
{

    [SerializeField] Animator animator;
    [SerializeField] TextMeshPro textMeshPro;

    private void OnEnable()
    {

        CombatEvents.UpdatePlayerHP += ShowPlayerDamageDisplay;
    }

    private void OnDisable()
    {
        CombatEvents.UpdatePlayerHP -= ShowPlayerDamageDisplay;
    }

    void Start()
    {
        textMeshPro.enabled = false;
    }

    public void ShowPlayerDamageDisplay(int value)

    {
        textMeshPro.color = Color.red;
        textMeshPro.enabled = true;
        textMeshPro.text = value.ToString();
        animator.SetTrigger("PlayerDamageTrigger");

        if (textMeshPro.text == "0" ) 
        
        { 
            textMeshPro.color = Color.white;
            textMeshPro.text = "FENDED!"; 
        }     
    }

    public void DisablePlayerDamageDisplay()

    {
        textMeshPro.enabled = false;
        animator.SetTrigger("PlayerDamageResetToDefault");
    }

   }
