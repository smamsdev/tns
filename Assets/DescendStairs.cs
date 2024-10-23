using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescendStairs : MonoBehaviour
{
    public GameObject bottomOfStairs;
    public GameObject ascendStairs;
    GameObject actor;

    private void OnTriggerEnter2D(Collider2D collisionWith)
    {
        actor = collisionWith.gameObject;

        actor.layer = LayerMask.NameToLayer("OnStairs");
        actor.GetComponent<MovementScript>().isAscending = new Vector2(-1, -1);
        actor.GetComponent<LayerChanger>().ChangeLayer("OnStairs");

        StartCoroutine(EnterStairs(actor.transform.position, new Vector2(actor.transform.position.x, actor.transform.position.y - 0.04f), 0.04f));
    }

    private IEnumerator EnterStairs(Vector2 start, Vector2 end, float duration)
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
         ascendStairs.GetComponent<BoxCollider2D>().enabled = true;
    }
}