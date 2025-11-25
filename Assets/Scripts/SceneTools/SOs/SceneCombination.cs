using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;

[CreateAssetMenu]
public class SceneCombination : ScriptableObject
{//
    public SceneAsset baseScene;
    public SceneAsset[] additiveScenes;
    public int activeAdditiveIteration;
}