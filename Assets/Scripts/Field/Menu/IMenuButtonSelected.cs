using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class IMenuButtonSelected : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] GameObject menuToDisplay;
    [SerializeField] GameObject blueUnderLine;

    private void OnEnable()
    {
        blueUnderLine.SetActive(false);
    }

    public void OnSelect(BaseEventData eventData)
    {
        blueUnderLine.SetActive(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        blueUnderLine.SetActive(false);
    }


}