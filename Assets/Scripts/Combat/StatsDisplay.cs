using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatsDisplay : MonoBehaviour
{
    public TextMeshProUGUI combatantNameTextMeshPro;
    public TextMeshProUGUI combatantHPTextMeshPro;
    [SerializeField] GameObject statsDisplayGameObject, combatantNameGO;
    public Animator HPTMPAnimator, statsDisplayContainerAnimator;
    public Combatant combatant;

    private void OnDisable()
    {
        ShowStatsDisplay(false);
    }

    public virtual void ShowStatsDisplay(bool on)
    {
        statsDisplayGameObject.SetActive(on);
        combatantNameGO.SetActive(on);
        combatantHPTextMeshPro.alpha = 1.0f;
    }

    public virtual void UpdateHPDisplay(int value)
    {
        combatantHPTextMeshPro.text = value.ToString() + " / " + combatant.MaxHP.ToString();
    }

    public virtual void ShowCombatantName(bool on)
    {
      //  combatantNameGO.SetActive(on);
    }

    public virtual void InitialiseCombatStatsDisplay(Combatant combatant)
    {
        this.combatant = combatant;

        combatantHPTextMeshPro.text = combatant.CurrentHP.ToString() + " / " + combatant.MaxHP.ToString();
        combatantNameTextMeshPro.text = combatant.combatantName;
    }
}
