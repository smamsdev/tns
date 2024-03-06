using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BattleUpdate : MonoBehaviour
{
    [SerializeField] CombatManagerV3 CombatManagerV3;
    public bool isCoolDown;
    float seconds;


    // detel this i think

    private void OnEnable()
    {
        CombatEvents.InputCoolDown += CoolDown;
    }

    private void OnDisable()
    {
        CombatEvents.InputCoolDown -= CoolDown;
    }

 //      private void LateUpdate()
 //
 //  {
 //      if (!isCoolDown)
 //          { CombatManagerV3.currentState.Update(); }
 //     
 //  }

    void CoolDown(float _seconds)

    {
        seconds = _seconds;

        StartCoroutine(CoolDownTimer());
    }

    IEnumerator CoolDownTimer()
    {
        isCoolDown = true;
        yield return new WaitForSeconds(seconds);
        isCoolDown = false;
    }

}
