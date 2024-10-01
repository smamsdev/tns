using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AscendStairs : MonoBehaviour
{
    public DescendStairs descendStairs;
    GameObject actor;

    private void Start()
    {
        this.GetComponent<BoxCollider2D>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collisionWith)
    {
        collisionWith.GetComponent<LayerChanger>().ChangeLayer("Actors and Buildings");
        collisionWith.GetComponent<MovementScript>().isAscending = new Vector2(1, 1);

        actor = collisionWith.gameObject;

        StartCoroutine(EscapeFromStairs(actor.transform.position,new Vector2 (actor.transform.position.x, actor.transform.position.y-0.04f), 0.04f));
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

        descendStairs.bottomOfStairs.SetActive(false);
        this.GetComponent<BoxCollider2D>().enabled = false;
        descendStairs.GetComponent<BoxCollider2D>().enabled = true;
    }
}
