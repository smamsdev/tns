using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class IMenuPartyMemberHighlighted : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public Button button;
    public TextMeshProUGUI levelTMP;
    public GameObject selectionArrowGO;
    public MenuStats menuStats;
    [SerializeField] int partyMemberSlot;
    [SerializeField] int arrowXPos;

    public void OnSelect(BaseEventData eventData)
    {
        levelTMP.color = Color.yellow;
        selectionArrowGO.transform.localPosition = new Vector2(arrowXPos, 35);
        menuStats.partyMemberSlot = partyMemberSlot;
        menuStats.InitializeStats();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        levelTMP.color = Color.white;
    }
}
