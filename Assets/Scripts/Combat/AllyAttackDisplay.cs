using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AllyAttackDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI allyAttackDamageTextMeshProUI;
    [SerializeField] GameObject attackDisplayTextGO;

    private void Start()
    {
        attackDisplayTextGO.SetActive(false);
    }

    public void UpdateAllyAttackDisplay(int value)

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
        if (on) { attackDisplayTextGO.SetActive(true); }
        if (!on) { attackDisplayTextGO.SetActive(false); }
    }
}
