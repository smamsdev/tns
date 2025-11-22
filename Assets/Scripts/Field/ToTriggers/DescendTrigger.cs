using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescendTrigger : ToTrigger
{
    public GameObject GOToDescend;
    public DescendStairs descendStairsScript;
    public float stairLength;
    public float duration;
    MovementScript movementScript;

        public override IEnumerator TriggerFunction()
        {
            CombatEvents.LockPlayerMovement();

            movementScript = GOToDescend.GetComponent<MovementScript>();
            movementScript.lookDirection = Vector2.up;
            GOToDescend.GetComponent<LayerChanger>().ChangeLayer("OnStairs");

            var GOTransform = GOToDescend.transform.position;

            StartCoroutine(EnterStairs(GOTransform, new Vector2(GOTransform.x, GOTransform.y - stairLength), duration));
            yield return null;
        }

        private IEnumerator EnterStairs(Vector2 start, Vector2 end, float duration)
        {
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
            movementScript.animator.SetFloat("verticalInput", 1);
            movementScript.animator.SetBool("isMoving", true);
            float t = elapsedTime / duration;
            GOToDescend.transform.position = Vector2.Lerp(start, end, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            GOToDescend.transform.position = end;
            CombatEvents.UnlockPlayerMovement();
            FieldEvents.HasCompleted.Invoke(this.gameObject);
        }
    }
