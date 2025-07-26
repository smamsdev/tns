using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatsDisplay : MonoBehaviour
{
    public TextMeshProUGUI combatantNameTextMeshPro;
    public TextMeshProUGUI combatantHPTextMeshPro;
    public GameObject statsDisplayGameObject;
    public Animator HPTMPAnimator;
    public Combatant combatant;
    public int combatantHP;

    private void OnDisable()
    {
        ShowStatsDisplay(false);
    }

    public void ShowStatsDisplay(bool on)

    {
        statsDisplayGameObject.SetActive(on);
    }

    public void UpdateHPDisplay(int value)
    {
        combatantHPTextMeshPro.text = "HP: " + value.ToString();

        if (combatant.CurrentHP <= 0)
        {
            HPTMPAnimator.SetTrigger("bump");
            combatantHPTextMeshPro.text = "DEFEATED";
        }
    }
}
