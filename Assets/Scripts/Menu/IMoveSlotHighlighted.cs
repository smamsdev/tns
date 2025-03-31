using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class IMoveSlotHighlighted : MonoBehaviour, ISelectHandler
{
    public MenuSlotSelect menuSlotSelect;
    MoveSlot moveSlot;

    private void OnEnable() //there are better ways to do this but life is too short
    {
        moveSlot = GetComponent<MoveSlot>();
    }

    public void OnSelect(BaseEventData eventData)
    {
        menuSlotSelect.moveSlotHighlighted = moveSlot;
    }
}
