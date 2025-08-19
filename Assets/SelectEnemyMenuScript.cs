using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectEnemyMenuScript : MonoBehaviour
{
    [SerializeField] CombatManager combatManager;
    public Combatant enemyhighlighted;
    [SerializeField] GameObject enemySelectButtonPrefab;
    [SerializeField] GameObject selectEnemyMenuContainer;
    public Button lastEnemySelected;
    public List<EnemySelectButtonScript> enemySelectButtons = new List<EnemySelectButtonScript>();

    public void InitializeButtonSlots()
    {
        enemySelectButtons.Clear();

        for (int i = 0; i < combatManager.enemies.Count; i++)
        {
            GameObject enemySelectButtonGO = Instantiate(enemySelectButtonPrefab, selectEnemyMenuContainer.transform);
            EnemySelectButtonScript enemySelectButtonScript = enemySelectButtonGO.GetComponent<EnemySelectButtonScript>();
            enemySelectButtonScript.selectEnemyMenuScript = this;
            enemySelectButtonScript.combatant = combatManager.enemies[i];
            enemySelectButtonScript.buttonText.text = enemySelectButtonScript.combatant.combatantName;
            enemySelectButtonGO.name = enemySelectButtonScript.combatant.combatantName;
            enemySelectButtons.Add(enemySelectButtonScript);
        }
        DefaultButtonSelected();


        SetGridNavigationWrapAround(enemySelectButtons,4);
    }

    void SetGridNavigationWrapAround(List<EnemySelectButtonScript> enemySelectButtons, int maxRows)
    {
        int buttonCount = enemySelectButtons.Count;
        int rows = Mathf.Min(buttonCount, maxRows); // vertical length of a column
        int columns = Mathf.CeilToInt((float)buttonCount / rows);

        for (int i = 0; i < buttonCount; i++)
        {
            Button button = enemySelectButtons[i].button;
            Navigation nav = button.navigation;
            nav.mode = Navigation.Mode.Explicit;

            int col = i / rows; // column-major
            int row = i % rows;

            int WrapIndex(int r, int c)
            {
                int index = c * rows + r;
                if (index >= buttonCount)
                    index = buttonCount - 1;
                return index;
            }

            nav.selectOnUp = enemySelectButtons[WrapIndex((row - 1 + rows) % rows, col)].button;
            nav.selectOnDown = enemySelectButtons[WrapIndex((row + 1) % rows, col)].button;
            nav.selectOnLeft = enemySelectButtons[WrapIndex(row, (col - 1 + columns) % columns)].button;
            nav.selectOnRight = enemySelectButtons[WrapIndex(row, (col + 1) % columns)].button;

            button.navigation = nav;
        }
    }

    public void DefaultButtonSelected()
    {
        if (lastEnemySelected == null)
        {
            lastEnemySelected = enemySelectButtons[0].button;
        }
    }

    public void HighlightEnemy(Combatant combatant)
    {
        combatManager.cameraFollow.transformToFollow = combatant.transform;
        enemyhighlighted = combatant;
        var combatantUI = combatant.combatantUI;

        combatantUI.selectedAnimator.SetBool("Flash", true);

        Vector2 direction = (combatant.transform.position - combatManager.player.transform.position).normalized;
        float attackDirX = Mathf.Sign(direction.x);

        combatManager.playerCombat.movementScript.lookDirection = new Vector2 (attackDirX, 0);
    }

    public void DeselectEnemy(Combatant combatant)
    {
        var combatantUI = combatant.combatantUI;
        combatantUI.selectedAnimator.SetBool("Flash", false);
    }

    public void SelectedEnemyToRevertToOnBack(Button button)  //prob dont eneed this
    { 
        combatManager.combatMenuManager.thirdMenuFirstButton = button;
    }
}
