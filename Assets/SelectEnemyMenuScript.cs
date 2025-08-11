using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectEnemyMenuScript : MonoBehaviour
{
    [SerializeField] CombatManager combatManager;
    public GameObject[] buttonSlotGOs;
    public Combatant enemyhighlighted;

    public void InitializeButtonSlots()
    {
        foreach (GameObject buttonGO in buttonSlotGOs)
        {
            buttonGO.SetActive(false);
        };

        for (int i = 0; i < combatManager.enemies.Count; i++)
        {
            buttonSlotGOs[i].SetActive(true);

            Button button = buttonSlotGOs[i].GetComponent<Button>();
            EnemySelectButtonScript enemySelectButtonScript = button.GetComponent<EnemySelectButtonScript>();
            enemySelectButtonScript.combatant = combatManager.enemies[i];
            enemySelectButtonScript.buttonText.text = enemySelectButtonScript.combatant.combatantName;
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
