using System.Collections;
using UnityEngine;

public class SubversivenessMoveBehaviour : MoveBehaviour
{
    public override IEnumerator ApplyCustomMoveFunction()
    {
        targetCombatant.combatantUI.fendScript.ShowFendDisplay(targetCombatant, false);

        yield return new WaitForSeconds(.5f);

        targetCombatant.FendTotal = 0;
        int previousFendBase = targetCombatant.FendBase;
        int newFendBase = Mathf.RoundToInt(previousFendBase - (previousFendBase * 0.25f));
        targetCombatant.FendBase = newFendBase;
        yield return null;
    }

    public override IEnumerator TriggerMoveAnimation()
    {
        combatantToActAnimator.SetFloat("MoveAnimationFloat", moveSO.MoveAnimationFloat);

        CameraFollow cameraFollow = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
        int currentPPU = cameraFollow.pixelPerfectCamera.assetsPPU;
        int newPPU = currentPPU * 3;

        combatantToActAnimator.speed = 0;
        combatantToActAnimator.Play("Attack", 0, 0.2f);
        yield return FieldEvents.LerpValuesCoRo(currentPPU, newPPU, 0.2f, (value) =>
        {
            cameraFollow.pixelPerfectCamera.assetsPPU = Mathf.RoundToInt(value);
        });

        yield return new WaitForSeconds(0.3f);

        combatantToActAnimator.speed = 1;

        yield return new WaitForSeconds(1f);

        StartCoroutine(FieldEvents.LerpValuesCoRo(newPPU, currentPPU, 0.2f, (value) =>
        {
            cameraFollow.pixelPerfectCamera.assetsPPU = Mathf.RoundToInt(value);
        }));
    }
}
