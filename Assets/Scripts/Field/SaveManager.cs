using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;

public class SaveManager : MonoBehaviour
{
    public PlayerPermanentStats permanentStatsSO;
    public SaveData[] saveDataSlots;
    public MenuSave menuSave;

    private void OnEnable()
    {
        saveDataSlots = new SaveData[]
        {
            new SaveData { slotNumber = 0 }, new SaveData { slotNumber = 1 }, new SaveData { slotNumber = 2 }
        };
    }

    private void Start()
    {
        permanentStatsSO = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>().playerPermanentStats;
    }

    IEnumerator SelectSlotToSaveCoRo(int slotNumber)
    {
        SaveGame(saveDataSlots[slotNumber]);
        yield return new WaitForSeconds(.1f);
        menuSave.UpdateSaveSlotUI();
    }

    public void SaveGame(SaveData saveDataSlot)
    {
        //save playtime somehow
        Debug.Log("good luck w this");

        //saveDataSlot.fendBase = permanentStatsSO.fendBase;
        //saveDataSlot.attackBase = permanentStatsSO.attackBase;
        //saveDataSlot.focusBase = permanentStatsSO.focusBase;
        //saveDataSlot.maxPotential = permanentStatsSO.maxPotential;
        //saveDataSlot.currentPotential = permanentStatsSO.currentPotential;
        //saveDataSlot.maxHP = permanentStatsSO.maxHP;
        //saveDataSlot.currentHP = permanentStatsSO.currentHP;
        //saveDataSlot.level = permanentStatsSO.level;
        //saveDataSlot.XP = permanentStatsSO.XP;
        //saveDataSlot.XPThreshold = permanentStatsSO.XPThreshold;
        ////saveDataSlot.XPremainder = permanentStatsSO.XPremainder;
        ////saveDataSlot.defaultXPThreshold = permanentStatsSO.defaultXPThreshold;
        //saveDataSlot.smams = permanentStatsSO.smams;
        saveDataSlot.date = System.DateTime.Now.ToString("yyyy/MM/dd");
        saveDataSlot.time = System.DateTime.Now.ToString("HH:mm:ss");
        saveDataSlot.screenshotPath = Path.Combine(Application.persistentDataPath, $"save-data-slot-{saveDataSlot.slotNumber}-screenshot.png");

        string json = JsonUtility.ToJson(saveDataSlot, true);  // `true` makes it pretty-print for easier reading

        string path = Path.Combine(Application.persistentDataPath, $"save-data-slot-{saveDataSlot.slotNumber}.json");

        File.WriteAllText(path, json);
        //Debug.Log($"Game saved to: {path}");

        SaveTempScreenShotToDisk(saveDataSlot);
    }

    public void SaveTempScreenShotToDisk(SaveData saveDataSlot)
    {
        if (menuSave.tempScreenshot == null)
        {
            Debug.LogError("Temp Screenshot not captured");
            return;
        }

        byte[] pngData = menuSave.tempScreenshot.EncodeToPNG();
        string filePath = Path.Combine(Application.persistentDataPath, saveDataSlot.screenshotPath);
        File.WriteAllBytes(filePath, pngData);
    }

    public bool ReadFromJson(SaveData saveData)
    {
        string path = Path.Combine(Application.persistentDataPath, $"save-data-slot-{saveData.slotNumber}.json");

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData loadedData = JsonUtility.FromJson<SaveData>(json);

            saveData.sceneName = loadedData.sceneName;
            saveData.fendBase = loadedData.fendBase;
            saveData.attackBase = loadedData.attackBase;
            saveData.focusBase = loadedData.focusBase;
            saveData.maxPotential = loadedData.maxPotential;
            saveData.currentPotential = loadedData.currentPotential;
            saveData.maxHP = loadedData.maxHP;
            saveData.currentHP = loadedData.currentHP;
            saveData.level = loadedData.level;
            saveData.XP = loadedData.XP;
            saveData.XPThreshold = loadedData.XPThreshold;
            saveData.XPremainder = loadedData.XPremainder;
            saveData.defaultXPThreshold = loadedData.defaultXPThreshold;
            saveData.smams = loadedData.smams;
            saveData.duration = loadedData.duration;
            saveData.date = loadedData.date;
            saveData.time = loadedData.time;
            saveData.screenshotPath = loadedData.screenshotPath;
            saveData.screenshot = loadedData.screenshot;
            LoadScreenshot(saveData);

            //Debug.Log($"Game loaded from: {path}");
            return true;
        }
        else
        {
            //Debug.LogWarning($"No save file found at: {path}");
            return false;
        }
    }

    public void LoadScreenshot(SaveData saveDataSlot)
    {
        if (File.Exists(saveDataSlot.screenshotPath))
        {
            byte[] imageBytes = File.ReadAllBytes(saveDataSlot.screenshotPath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageBytes);
            saveDataSlot.screenshot = texture;

            //Debug.Log("Screenshot loaded");
        }
        else
        {
            //Debug.LogWarning("Screenshot file not found.");
        }
    }

    private void ApplyLoadedData(SaveData saveData)
    {
        if (saveData != null)
        {
            Debug.Log("enjoy this take a deep breath");
           // permanentStatsSO.attackBase = saveData.attackBase;
           // permanentStatsSO.focusBase = saveData.focusBase;
           // permanentStatsSO.maxPotential = saveData.maxPotential;
           // permanentStatsSO.currentPotential = saveData.currentPotential;
           // permanentStatsSO.maxHP = saveData.maxHP;
           // permanentStatsSO.currentHP = saveData.currentHP;
           // permanentStatsSO.level = saveData.level;
           // permanentStatsSO.XP = saveData.XP;
           // permanentStatsSO.XPThreshold = saveData.XPThreshold;
           // //permanentStatsSO.XPremainder = saveData.XPremainder;
           // // permanentStatsSO.defaultXPThreshold = saveData.defaultXPThreshold;
        }
    }
}

[System.Serializable]
public class SaveData
{
    public string sceneName;
    public int slotNumber;
    public int fendBase;
    public int attackBase;
    public int focusBase;
    public int maxPotential;
    public int currentPotential;
    public int maxHP;
    public int currentHP;
    public int level;
    public int XP;
    public int XPThreshold;
    public int XPremainder;
    public int defaultXPThreshold;
    public int smams;
    public string duration;
    public string date;
    public string time;
    public string screenshotPath;
    public Texture screenshot;


    //saveLevel.text =  saveData.level.ToString();
    //    saveDate.text = saveData.saveDate
    //    saveTime.text = 
    //    saveDuration.text =
    //    saveSmams.text =
}