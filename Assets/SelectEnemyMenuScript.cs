using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectEnemyMenuScript : MonoBehaviour
{
    public CombatManager combatManager;
    public Combatant enemyhighlighted;
    [SerializeField] GameObject enemySelectButtonPrefab;
    [SerializeField] GameObject selectEnemyMenuContainer;
    public Button defaultButton;
    public List<EnemySelectButtonScript> enemySelectButtons = new List<EnemySelectButtonScript>();

    public void InitializeButtonSlots()
    {
        foreach (Transform child in selectEnemyMenuContainer.transform)
        {
            Destroy(child.gameObject);
        }

        enemySelectButtons.Clear();
        List<Button> buttons = new List<Button>();

        for (int i = 0; i < combatManager.enemies.Count; i++)
        {
            GameObject enemySelectButtonGO = Instantiate(enemySelectButtonPrefab, selectEnemyMenuContainer.transform);
            EnemySelectButtonScript enemySelectButtonScript = enemySelectButtonGO.GetComponent<EnemySelectButtonScript>();
            enemySelectButtonScript.selectEnemyMenuScript = this;
            enemySelectButtonScript.combatant = combatManager.enemies[i];
            enemySelectButtonScript.buttonText.text = enemySelectButtonScript.combatant.combatantName;
            enemySelectButtonGO.name = enemySelectButtonScript.combatant.combatantName;
            enemySelectButtons.Add(enemySelectButtonScript);
            buttons.Add(enemySelectButtonScript.button);
        }
        FieldEvents.SetGridNavigationWrapAround(buttons, 4);
        defaultButton = enemySelectButtons[0].button;
    }

    public void HighlightEnemy(Combatant combatant)
    {
        combatManager.cameraFollow.transformToFollow = combatant.transform;
        enemyhighlighted = combatant;
        var combatantUI = combatant.combatantUI;
        combatantUI.statsDisplay.ShowStatsDisplay(true);

        combatantUI.selectedAnimator.SetBool("Flash", true);
        combatManager.SelectAndDisplayCombatantMove(combatant);

        Vector2 direction = (combatant.transform.position - combatManager.player.transform.position).normalized;
        float attackDirX = Mathf.Sign(direction.x);

        combatManager.playerCombat.movementScript.lookDirection = new Vector2 (attackDirX, 0);
    }

    public void DeselectEnemy(Combatant combatant)
    {
        var combatantUI = combatant.combatantUI;
        combatantUI.selectedAnimator.SetBool("Flash", false);
        combatantUI.statsDisplay.ShowStatsDisplay(false);
        combatantUI.attackDisplay.attackDisplayAnimator.Play("CombatantAttackDamageFadeUpReverse");
        if (combatant.fendTotal > 0)
        {
            combatant.combatantUI.fendScript.fendAnimator.Play("FendAppearReverse");
        }
    }

    public void CombatantSelected(Combatant combatant)
    {
        Combatant combatant1 = combatant;
        Debug.Log("is this on");
    }

}
