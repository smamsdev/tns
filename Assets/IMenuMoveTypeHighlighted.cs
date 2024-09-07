using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class IMenuMoveTypeHighlighted : MonoBehaviour, ISelectHandler
{
    public TextMeshProUGUI moveDescriptionFieldTMP;
    public string textToUpdate;
    public Button button;

    public void OnSelect(BaseEventData eventData)
    {
        moveDescriptionFieldTMP.text = textToUpdate;
    }
}
