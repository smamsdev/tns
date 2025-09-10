using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatsDisplay : MonoBehaviour
{
    public TextMeshProUGUI combatantNameTextMeshPro;
    public TextMeshProUGUI combatantHPTextMeshPro;
    public GameObject statsDisplayGameObject;
    public Animator HPTMPAnimator, statsDisplayContainerAnimator;
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
            statsDisplayContainerAnimator.Play("StatsDisplayOnDefeat");
        }
    }

    public virtual void InitialiseCombatStatsDisplay(Combatant combatant)
    {
        this.combatant = combatant;

        combatantHP = combatant.CurrentHP;
        combatantHPTextMeshPro.text = "HP: " + combatant.CurrentHP.ToString();
    }
}
