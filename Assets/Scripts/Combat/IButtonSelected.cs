using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class IButtonSelected : MonoBehaviour, ISelectHandler
{

    public void OnSelect(BaseEventData eventData)
    {
        CombatEvents.ButtonHighlighted?.Invoke(this.gameObject);
    }


}