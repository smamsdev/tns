using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    public PlayerPermanentStats permanentStatsSO;
    public RawImage saveSlotOneImage;

    private void Start()
    {
        permanentStatsSO = GameObject.FindGameObjectWithTag("CombatManager").GetComponent<PlayerCombatStats>().playerPermanentStats;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            SaveData loadedData = LoadGame(1);

            if (loadedData != null)
            {
                Debug.Log("Game Loaded!");
            }
            else
            {
                Debug.Log("No save file found.");
            }
        }

        if (Input.GetKeyDown(KeyCode.I))

        { 
            SaveGame(1);
        }
    }

    public void SaveGame(int slot)
    {
        SaveData saveData = new SaveData();

        saveData.fendBase = permanentStatsSO.fendBase;
        saveData.attackPowerBase = permanentStatsSO.attackPowerBase;
        saveData.playerFocusbase = permanentStatsSO.playerFocusbase;
        saveData.maxPotential = permanentStatsSO.maxPotential;
        saveData.currentPotential = permanentStatsSO.currentPotential;
        saveData.maxHP = permanentStatsSO.maxHP;
        saveData.currentHP = permanentStatsSO.currentHP;
        saveData.level = permanentStatsSO.level;
        saveData.XP = permanentStatsSO.XP;
        saveData.XPThreshold = permanentStatsSO.XPThreshold;
        saveData.XPremainder = permanentStatsSO.XPremainder;
        saveData.defaultXPThreshold = permanentStatsSO.defaultXPThreshold;

        // Convert to JSON
        string json = JsonUtility.ToJson(saveData, true);  // `true` makes it pretty-print for easier reading

        // Get save path for JSON
        string path = Path.Combine(Application.persistentDataPath, $"saveSlot{slot}.json");

        // Write to file
        File.WriteAllText(path, json);
        Debug.Log($"Game saved to: {path}");

        // Define screenshot path
        string screenshotPath = Path.Combine(Application.persistentDataPath, $"saveSlot{slot}_screenshot.png");

        // Take a screenshot
        StartCoroutine(CaptureAndSaveScreenshot(screenshotPath));
    }

    private IEnumerator CaptureAndSaveScreenshot(string screenshotPath)
    {
        // Wait for 1 second before capturing the screenshot
        yield return new WaitForSeconds(.1f);

        // Capture the screenshot
        ScreenCapture.CaptureScreenshot(screenshotPath);
        Debug.Log($"Screenshot requested, saving to: {screenshotPath}");

        // Optional: Wait until the screenshot file is written to disk (this may take some time)
        yield return new WaitUntil(() => File.Exists(screenshotPath));

        // Check if the file exists
        if (File.Exists(screenshotPath))
        {
            Debug.Log("Screenshot successfully saved.");
            byte[] imageBytes = File.ReadAllBytes(screenshotPath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageBytes);  // Load the image into the texture

            // Optionally, display the texture (for example, with a RawImage in the UI)
            // screenshotDisplay.texture = texture;
        }
        else
        {
            Debug.LogWarning("Screenshot file not found after capture.");
        }

        LoadScreenshot(screenshotPath);
    }

    public SaveData LoadGame(int slot)
    {
        string path = Path.Combine(Application.persistentDataPath, $"saveSlot{slot}.json");

        if (File.Exists(path))
        {
            // Read the saved JSON string from the file
            string json = File.ReadAllText(path);

            // Convert the JSON string back into a SaveData object
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            // Apply the data to your game (e.g., player stats, inventory, etc.)
            ApplyLoadedData(data);

            Debug.Log($"Game loaded from: {path}");
            return data;
        }
        else
        {
            Debug.LogWarning($"No save file found at: {path}");
            return null;
        }
    }

    // Apply the loaded data to your game objects (example for player stats)
    private void ApplyLoadedData(SaveData saveData)
    {
        if (saveData != null)
        {
            permanentStatsSO.attackPowerBase = saveData.attackPowerBase;
            permanentStatsSO.playerFocusbase = saveData.playerFocusbase;
            permanentStatsSO.maxPotential = saveData.maxPotential;
            permanentStatsSO.currentPotential = saveData.currentPotential;
            permanentStatsSO.maxHP = saveData.maxHP;
            permanentStatsSO.currentHP = saveData.currentHP;
            permanentStatsSO.level = saveData.level;
            permanentStatsSO.XP = saveData.XP;
            permanentStatsSO.XPThreshold = saveData.XPThreshold;
            permanentStatsSO.XPremainder = saveData.XPremainder;
            permanentStatsSO.defaultXPThreshold = saveData.defaultXPThreshold;
        }
    }

    public void LoadScreenshot(string screenshotPath)
    {
        if (File.Exists(screenshotPath))
        {
            // Read the image file into a byte array
            byte[] imageBytes = File.ReadAllBytes(screenshotPath);

            // Create a new texture and load the image data into it
            Texture2D texture = new Texture2D(2, 2);  // Temporary 2x2 texture, will be resized when loading
            texture.LoadImage(imageBytes);  // Load the image data

            // Assign the texture to the RawImage UI element to display it
            saveSlotOneImage.texture = texture;

            Debug.Log("Screenshot successfully loaded and displayed.");
        }
        else
        {
            Debug.LogWarning("Screenshot file not found.");
        }
    }


}

[System.Serializable]
public class SaveData
{
    public int fendBase;
    public int attackPowerBase;
    public int playerFocusbase;
    public int maxPotential;
    public int currentPotential;
    public int maxHP;
    public int currentHP;
    public int level;
    public int XP;
    public int XPThreshold;
    public int XPremainder;
    public int defaultXPThreshold;
}