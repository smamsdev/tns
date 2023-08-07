using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.
using UnityEngine.Events;

public class ShowBodyAttackInfo : MonoBehaviour, ISelectHandler
{

    //Do this when the selectable UI object is selected.
    public void OnSelect(BaseEventData eventData)
    {
        CombatEvents.HighlightBodypartTarget.Invoke(true, false, false);
    }
}
