using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemySelectTargetHighlight : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public SelectEnemyMenuScript selectEnemyMenuScript;

    public void OnSelect(BaseEventData eventData)
    {
        selectEnemyMenuScript.HighlightEnemy(int.Parse(this.gameObject.name));
    }

    public void OnDeselect(BaseEventData eventData)
    {
        selectEnemyMenuScript.DeselectEnemy(int.Parse(this.gameObject.name));
    }
}
