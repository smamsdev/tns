using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public static class AddScenesToBuildTool
{
    [MenuItem("Tools/Add All Scenes In Folder To Build")]
    public static void AddAllScenesInFolder()
    {
        string folderPath = "Assets/Scenes"; // Change this to your folder path
        if (!Directory.Exists(folderPath))
        {
            Debug.LogError($"Folder does not exist: {folderPath}");
            return;
        }

        // Get all .unity files in folder and subfolders
        string[] sceneFiles = Directory.GetFiles(folderPath, "*.unity", SearchOption.AllDirectories);

        List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);

        foreach (string sceneFile in sceneFiles)
        {
            string unityPath;

            try
            {
                // Convert absolute path to Unity project path
                unityPath = Path.GetRelativePath(Application.dataPath, sceneFile).Replace("\\", "/");
                unityPath = "Assets/" + unityPath;
            }
            catch
            {
                Debug.LogWarning($"Skipping invalid scene path: {sceneFile}");
                continue;
            }

            // Avoid duplicates
            if (!scenes.Exists(s => s.path == unityPath))
            {
                scenes.Add(new EditorBuildSettingsScene(unityPath, true));
                Debug.Log($"Added scene to build: {unityPath}");
            }
        }

        // Save updated build settings
        EditorBuildSettings.scenes = scenes.ToArray();
        Debug.Log("All scenes in folder (and subfolders) added to build settings.");
    }
}
