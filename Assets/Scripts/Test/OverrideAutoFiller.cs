using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

public class OverrideAutoFiller : EditorWindow
{
    AnimatorOverrideController controller;
    DefaultAsset animationFolder; // Folder containing character clips

    [MenuItem("Tools/Auto Fill Animator Overrides")]
    public static void ShowWindow()
    {
        GetWindow<OverrideAutoFiller>("Override Auto Filler");
    }

    void OnGUI()
    {
        controller = (AnimatorOverrideController)EditorGUILayout.ObjectField(
            "Override Controller", controller, typeof(AnimatorOverrideController), false);

        animationFolder = (DefaultAsset)EditorGUILayout.ObjectField(
            "Animation Folder", animationFolder, typeof(DefaultAsset), false);

        if (GUILayout.Button("Auto-Fill Overrides"))
        {
            FillOverrides();
        }
    }

    void FillOverrides()
    {
        if (controller == null || animationFolder == null)
        {
            Debug.LogWarning("Assign both controller and folder.");
            return;
        }

        // Load all AnimationClips in the selected folder
        string folderPath = AssetDatabase.GetAssetPath(animationFolder);
        var clipGUIDs = AssetDatabase.FindAssets("t:AnimationClip", new[] { folderPath });
        var clips = clipGUIDs
            .Select(guid => AssetDatabase.LoadAssetAtPath<AnimationClip>(AssetDatabase.GUIDToAssetPath(guid)))
            .Where(c => c != null)
            .ToList();

        if (clips.Count == 0)
        {
            Debug.LogWarning("No animation clips found in the folder!");
            return;
        }

        // Get current overrides from the controller
        var pairs = new List<KeyValuePair<AnimationClip, AnimationClip>>();
        controller.GetOverrides(pairs);

        int matchCount = 0;

        for (int i = 0; i < pairs.Count; i++)
        {
            var baseClip = pairs[i].Key;
            string baseName = baseClip.name.Trim();

            // Match any clip that contains the base clip name (handles character prefix/suffix)
            var match = clips.FirstOrDefault(c => c.name.Trim().Contains(baseName));

            if (match != null)
            { 
                pairs[i] = new KeyValuePair<AnimationClip, AnimationClip>(baseClip, match);
                matchCount++;
                //  Debug.Log($"Matched base '{baseName}' → '{match.name}'");
            }
            else
            {
                pairs[i] = new KeyValuePair<AnimationClip, AnimationClip>(baseClip, null);
                //  Debug.LogWarning($"No match found for base clip '{baseName}'");
            }
        }

        // Apply overrides
        controller.ApplyOverrides(pairs);
        EditorUtility.SetDirty(controller);
    }
}
