using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemySelectButtonScript : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public SelectEnemyMenuScript selectEnemyMenuScript;
    public TextMeshProUGUI buttonText;
    public Enemy enemy;

    public void OnSelect(BaseEventData eventData)
    {
        selectEnemyMenuScript.HighlightEnemy(enemy);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        selectEnemyMenuScript.DeselectEnemy(enemy);
    }
}
