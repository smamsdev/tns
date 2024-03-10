using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyFendScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI fendTextMeshProUGUI;
    [SerializeField] CombatManager combatManager;
    [SerializeField] GameObject enemyFendContainer;
    [SerializeField] Animator animator;
    [SerializeField] Animator iconAnimator;

    public int attackRemainder;

    public int fend = 0;

    private void OnEnable()
    {
        CombatEvents.BattleMode += Init;
    }

    private void OnDisable()
    {
        CombatEvents.BattleMode -= Init;
    }

    void Init(bool on)
    {
        enemyFendContainer.transform.position = new Vector2(combatManager.battleScheme.enemyFightingPosition.transform.position.x, combatManager.battleScheme.enemyFightingPosition.transform.position.y + 0.8f);
    }

    public void ShowHideFendDisplay(bool on)

    {
        if (on)

        {
            enemyFendContainer.SetActive(true);
        }

        if (!on)
        {
            enemyFendContainer.SetActive(false);
        }
    }
}
