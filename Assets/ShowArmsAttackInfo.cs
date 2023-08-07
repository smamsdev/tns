using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.

public class ShowArmsAttackInfo : MonoBehaviour, ISelectHandler// required interface when using the OnSelect method.
{

        //Do this when the selectable UI object is selected.
    public void OnSelect(BaseEventData eventData)
    {
        CombatEvents.HighlightBodypartTarget.Invoke(false, true, false);
    }

    
}