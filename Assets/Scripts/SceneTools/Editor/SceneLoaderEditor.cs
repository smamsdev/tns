using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SceneLoader))]
public class SceneLoaderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SceneLoader loader = (SceneLoader)target;

        // Draw default field for the ScriptableObject
        loader.sceneCombination = (SceneCombination)EditorGUILayout.ObjectField("Scene Combo",
            loader.sceneCombination, typeof(SceneCombination), false);

        // If a SO is assigned, make a dropdown of its additive scenes
        if (loader.sceneCombination != null && loader.sceneCombination.additiveScenes != null)
        {
            string[] options = new string[loader.sceneCombination.additiveScenes.Length];
            for (int i = 0; i < options.Length; i++)
                options[i] = loader.sceneCombination.additiveScenes[i] != null
                    ? loader.sceneCombination.additiveScenes[i].name
                    : "Null";

            loader.additiveSceneIteration = EditorGUILayout.Popup("Current Stage", loader.additiveSceneIteration, options);
        }

        // Optional: Draw the rest of the inspector
        DrawDefaultInspector();

        if (GUI.changed)
            EditorUtility.SetDirty(loader);
    }
}
