using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuSave : Menu
{
    [SerializeField] Button firstButtonToSelect;

    public TextMeshProUGUI[] saveSceneNameTMPs;
    public TextMeshProUGUI[] saveLevelTMPs;
    public TextMeshProUGUI[] saveDurationTMPs;
    public TextMeshProUGUI[] saveDateTMPs;
    public TextMeshProUGUI[] saveTimeTMPs;
    public TextMeshProUGUI[] saveSmamsTMPs;
    public RawImage[] screenshotDisplays;
    [SerializeField] IMenuSaveSlotHighlighted[] menuSaveSlotHighlighteds;
    public Texture2D tempScreenshot;

    public SaveManager saveManager;

    [SerializeField] GameObject areYouSureGO;

    public Button yes, no;
    int slotNumberToSave;

    public override void DisplayMenu(bool on)
    {
        areYouSureGO.SetActive(false);
        displayContainer.SetActive(on);
        return;
    }

    public void SelectSlotToSave(int slotNumber)
    {
        areYouSureGO.SetActive(true);
        no.Select();
        slotNumberToSave = slotNumber;
        menuSaveSlotHighlighteds[slotNumberToSave].TextYellow();
    }

    public void ConfirmSlotToSave()
    {
        StartCoroutine(SelectSlotToSaveCoRo(slotNumberToSave));
        areYouSureGO.SetActive(false);
        menuSaveSlotHighlighteds[slotNumberToSave].GetComponent<Button>().Select();
    }

    public void DeclineSave()
    {
        areYouSureGO.SetActive(false);
        menuSaveSlotHighlighteds[slotNumberToSave].GetComponent<Button>().Select();
    }

    IEnumerator SelectSlotToSaveCoRo(int slotNumber)
    {
        saveManager.SaveGame(saveManager.saveDataSlots[slotNumber]);
        yield return new WaitForSeconds(.01f);
        UpdateSaveSlotUI();
        areYouSureGO.SetActive(false);
    }

    public void UpdateSaveSlotUI()
    {
        foreach (SaveData saveData in saveManager.saveDataSlots)
        {
            if (saveManager.ReadFromJson(saveData))
            {
                saveSceneNameTMPs[saveData.slotNumber].text = saveData.sceneName;
                saveLevelTMPs[saveData.slotNumber].text = $"LEVEL: {saveData.level}";
                saveDurationTMPs[saveData.slotNumber].text = $"DURATION: {saveData.duration}";
                saveDateTMPs[saveData.slotNumber].text = $"DATE: {saveData.date}";
                saveTimeTMPs[saveData.slotNumber].text = $"TIME: {saveData.time}";
                saveSmamsTMPs[saveData.slotNumber].text = $"$MAMS {saveData.smams}";
                screenshotDisplays[saveData.slotNumber].texture = saveData.screenshot;
            }
        }
    }

    public override void EnterMenu()
    {
        menuManagerUI.ClearThenDisplayMenu(this);

        firstButtonToSelect.Select();
    }

    public override void ExitMenu()
    {
        menuManagerUI.EnterMenu(menuManagerUI.main);
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (areYouSureGO.activeSelf)

            {
                DeclineSave();
            }

            else
            {
                ExitMenu();
            }
        }
    }
}
