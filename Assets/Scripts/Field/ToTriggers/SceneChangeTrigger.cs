using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeTrigger : ToTrigger
{
    public SceneCombination sceneCombinationToEnter;

    public override IEnumerator TriggerFunction()
    {
        var sceneCombo = sceneCombinationToEnter;
        var defaultFaderAnimator = GameObject.FindGameObjectWithTag("Fader").GetComponent<Animator>();
        defaultFaderAnimator.Play("DissolveToBlack");

        yield return new WaitForSeconds(0.5f);

        yield return LoadScene(sceneCombo.baseScene.name, sceneCombo.additiveScenes[sceneCombo.activeAdditiveIteration].name);
    }

    public IEnumerator LoadScene(String newBaseSceneName, String newAdditiveSceneName)
    {
        SceneManager.LoadScene(newBaseSceneName, LoadSceneMode.Single);
        SceneManager.LoadScene(newAdditiveSceneName, LoadSceneMode.Additive);
        FieldEvents.sceneName = sceneCombinationToEnter.sceneName;
        yield return null;
    }
}
