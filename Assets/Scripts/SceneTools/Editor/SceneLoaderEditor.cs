using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SceneIterationUpdater))]
public class SceneIterationUpdaterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SceneIterationUpdater updater = (SceneIterationUpdater)target;

        if (updater.sceneComboToChange != null &&
            updater.sceneComboToChange.additiveScenes != null)
        {
            var scenes = updater.sceneComboToChange.additiveScenes;
            string[] options = new string[scenes.Length];

            for (int i = 0; i < scenes.Length; i++)
            {
                var scene = scenes[i];
                options[i] = scene ? scene.name : "Null";
            }

            EditorGUI.BeginChangeCheck();

            int newIndex = EditorGUILayout.Popup("Additive Scene To Change To", updater.additiveIterationToChangeTo, options);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(updater, "Change Additive Scene");
                updater.additiveIterationToChangeTo = newIndex;
                EditorUtility.SetDirty(updater);
            }
        }
    }
}
