using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AscendStairs : MonoBehaviour
{
    public DescendStairs descendStairs;
    GameObject actor;

    int ignoreLayer = 10;

    private void OnTriggerEnter2D(Collider2D collisionWith)
    {
        if (collisionWith.gameObject.layer != ignoreLayer)
        { 
            collisionWith.GetComponent<LayerChanger>().ChangeLayer("Actors");
            collisionWith.GetComponent<MovementScript>().isReversing = new Vector2(1, 1);

            actor = collisionWith.gameObject;
            actor.layer = LayerMask.NameToLayer("Actors");

            StartCoroutine(EscapeFromStairs(actor.transform.position, new Vector2(actor.transform.position.x, actor.transform.position.y - 0.04f), 0.04f));
        }
    }

    private IEnumerator EscapeFromStairs(Vector2 start, Vector2 end, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            actor.transform.position = Vector2.Lerp(start, end, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        actor.transform.position = end;
    }
}
