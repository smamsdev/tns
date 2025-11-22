using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AscendTrigger : ToTrigger
{
    public GameObject GOToAscend;
    public float stairLength;
    public float duration;
    MovementScript movementScript;

        public override IEnumerator TriggerFunction()
        {
            CombatEvents.LockPlayerMovement();

            movementScript = GOToAscend.GetComponent<MovementScript>();
            movementScript.lookDirection = Vector2.down;

            var GOTransform = GOToAscend.transform.position;

            StartCoroutine(ExitStairs(GOTransform, new Vector2(GOTransform.x, GOTransform.y + stairLength), duration));
            yield return null;
        }

        private IEnumerator ExitStairs(Vector2 start, Vector2 end, float duration)
        {
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
            movementScript.animator.SetFloat("verticalInput", -1);
            movementScript.animator.SetBool("isMoving", true);
            float t = elapsedTime / duration;
            GOToAscend.transform.position = Vector2.Lerp(start, end, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            GOToAscend.transform.position = end;
            GOToAscend.GetComponent<LayerChanger>().ChangeLayer("Actors");
            CombatEvents.UnlockPlayerMovement();
        }
    }
