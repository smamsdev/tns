using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AttackDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI allyAttackDamageTextMeshProUI;
    [SerializeField] GameObject attackDisplayTextGO;

    private void Start()
    {
        ShowAttackDisplay(false);
    }

    public void UpdateAttackDisplay(int value)

    {
        if (value >= 0)
        {
            attackDisplayTextGO.SetActive(false);
        }

        attackDisplayTextGO.SetActive(true);
        allyAttackDamageTextMeshProUI.text = value.ToString();
    }

    public void ShowAttackDisplay(bool on)

    {
        attackDisplayTextGO.SetActive(on);
    }
}
