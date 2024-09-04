using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class IMenuButtonSelected : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] Menu menuToDisplay;
    [SerializeField] GameObject blueUnderLine;

    [SerializeField] MenuManagerUI menuManager;

    private void OnEnable()
    {
        blueUnderLine.SetActive(false);
    }

    public void OnSelect(BaseEventData eventData)
    {
        blueUnderLine.SetActive(true);
        menuManager.DisplayMenu(menuToDisplay);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        blueUnderLine.SetActive(false);
    }


}