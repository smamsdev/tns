using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class IMenuMoveTypeHighlighted : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public TextMeshProUGUI moveDescriptionFieldTMP;
    public string textToUpdate;
    public Button button;
    public GameObject symbol;

    private void Start()
    {
        symbol.SetActive(false);
    }

    public void OnSelect(BaseEventData eventData)
    {
        moveDescriptionFieldTMP.text = textToUpdate;
        symbol.SetActive(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        symbol.SetActive(false);
    }
}
