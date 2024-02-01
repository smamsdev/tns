using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableObject : ToTrigger
{
    [SerializeField] GameObject GoToEnable;


    private void OnEnable()
    {
        FieldEvents.PlayerRayCastHit += PlayerRayCastHit;
    }

    private void OnDisable()
    {
        FieldEvents.PlayerRayCastHit -= PlayerRayCastHit;
    }

    public override IEnumerator DoAction()

    {
        CombatEvents.LockPlayerMovement();
        GoToEnable.SetActive(true);

        FieldEvents.HasCompleted.Invoke(this.gameObject);
        yield return null;
    }

    void PlayerRayCastHit(RaycastHit2D raycastHit2D)

    {
        if (raycastHit2D.collider.gameObject == this.gameObject && !FieldEvents.isCooldown())
        {
            StartCoroutine(FieldEvents.CoolDown(0.3f));
           StartCoroutine(DoAction());
        }
    }

}