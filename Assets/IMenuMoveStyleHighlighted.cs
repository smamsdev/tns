using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class IMenuMoveStyleHighlighted : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public TextMeshProUGUI moveDescriptionFieldTMP;
    public string textToUpdate;
    public GameObject subMenuToDisplay;

    private void OnEnable()
    {
        subMenuToDisplay.SetActive(false);
    }

    public void OnSelect(BaseEventData eventData)
    {
        moveDescriptionFieldTMP.text = textToUpdate;
        subMenuToDisplay.SetActive(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        subMenuToDisplay.SetActive(false);
    }
}
