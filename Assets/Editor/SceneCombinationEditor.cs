using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(SceneCombination))]
public class SceneCombinationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        SceneCombination combo = (SceneCombination)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Load Scene Combination"))
        {
            LoadSceneCombo(combo);
        }
    }

    private void LoadSceneCombo(SceneCombination combo)
    {
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            return;
        }

        EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(combo.baseScene),OpenSceneMode.Single);
        EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(combo.additiveScenes[combo.activeAdditiveIteration]), OpenSceneMode.Additive);
    }
}
