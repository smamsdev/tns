using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class IMenuSaveSlotHighlighted : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] MenuSave menuSave;
    public int saveSlot;

    public void OnSelect(BaseEventData eventData)
    {
        TextYellow();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        TextWhite();
    }

    public void TextWhite()

    {
        menuSave.saveSceneNameTMPs[saveSlot].color = Color.white;
        menuSave.saveLevelTMPs[saveSlot].color = Color.white;
        menuSave.saveDurationTMPs[saveSlot].color = Color.white;
        menuSave.saveDateTMPs[saveSlot].color = Color.white;
        menuSave.saveTimeTMPs[saveSlot].color = Color.white;
        menuSave.saveSmamsTMPs[saveSlot].color = Color.white;
    }

    public void TextYellow()

    {
        menuSave.saveSceneNameTMPs[saveSlot].color = Color.yellow;
        menuSave.saveLevelTMPs[saveSlot].color = Color.yellow;
        menuSave.saveDurationTMPs[saveSlot].color = Color.yellow;
        menuSave.saveDateTMPs[saveSlot].color = Color.yellow;
        menuSave.saveTimeTMPs[saveSlot].color = Color.yellow;
        menuSave.saveSmamsTMPs[saveSlot].color = Color.yellow;
    }
}
