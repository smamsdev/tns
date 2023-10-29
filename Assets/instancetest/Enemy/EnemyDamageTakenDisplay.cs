using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyDamageTakenDisplay : MonoBehaviour
{

    [SerializeField] Animator animator;
    public TextMeshPro textMeshPro;

    private void OnEnable()
    {
        CombatEvents.UpdateenemyHPUI += ShowEnemyDamageDisplay;
    }

    private void OnDisable()
    {
        CombatEvents.UpdateenemyHPUI -= ShowEnemyDamageDisplay;
    }

    void Start()
    {
        textMeshPro.enabled = false;
    }

     public void ShowEnemyDamageDisplay(int value)
    { 
    
        if (value > 0)
        {
            textMeshPro.enabled = true;
            textMeshPro.text = value.ToString();
            animator.SetTrigger("EnemyDamageTrigger");
        }   
    }
     
    public void DisableEnemyDamageDisplay()

    {
        textMeshPro.enabled = false;
    }

   }
