using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SelectEnemyMenuScript : MonoBehaviour
{
    public CombatManager combatManager;
    public EnemySelectButtonScript enemySelectButtonScriptHighlighted;
    [SerializeField] GameObject enemySelectButtonPrefab;
    [SerializeField] GameObject selectEnemyMenuContainer;
    public Button defaultButton;
    public List<EnemySelectButtonScript> enemySelectButtons = new List<EnemySelectButtonScript>();
    public GameObject selectEnemyLabelGO;
    public bool isEnemySlotsInitialized;


    public void InitializeButtonSlots()
    {
        if (isEnemySlotsInitialized)
        { return; }

        isEnemySlotsInitialized = true;

        foreach (Transform child in selectEnemyMenuContainer.transform)
        {
            if (child != selectEnemyLabelGO.transform)
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

            enemySelectButtonScript.button.onClick.AddListener(enemySelectButtonScript.OnButtonSelected);

            buttons.Add(enemySelectButtonScript.button);
        }
        FieldEvents.SetGridNavigationWrapAround(buttons, 4);
        combatManager.combatMenuManager.selectEnemyMenuDefaultButton = enemySelectButtons[0].button;
    }

    public void HighlightEnemy(EnemySelectButtonScript enemySelectScript)
    {
        combatManager.cameraFollow.transformToFollow = enemySelectScript.combatant.transform;
        combatManager.combatMenuManager.selectEnemyMenuDefaultButton = enemySelectScript.button;
        enemySelectButtonScriptHighlighted = enemySelectScript;
        var combatantUI = enemySelectScript.combatant.combatantUI;
        combatantUI.statsDisplay.ShowStatsDisplay(true);

        combatantUI.selectedAnimator.SetBool("Flash", true);
        combatManager.SelectAndDisplayCombatantMove(enemySelectScript.combatant);

        Vector2 direction = (enemySelectScript.combatant.transform.position - combatManager.player.transform.position).normalized;
        float attackDirX = Mathf.Sign(direction.x);

        combatManager.playerCombat.movementScript.lookDirection = new Vector2 (attackDirX, 0);
    }

    public void DeselectEnemy(EnemySelectButtonScript enemySelectScript)
    {
        var combatantUI = enemySelectScript.combatant.combatantUI;
        combatantUI.selectedAnimator.SetBool("Flash", false);
        combatantUI.statsDisplay.ShowStatsDisplay(false);
        combatantUI.attackDisplay.attackDisplayAnimator.Play("CombatantAttackDamageFadeUpReverse");
        if (enemySelectScript.combatant.fendTotal > 0)
        {
            enemySelectScript.combatant.combatantUI.fendScript.fendAnimator.Play("FendAppearReverse");
        }
    }

}
