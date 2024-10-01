using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescendStairs : MonoBehaviour
{
    public GameObject bottomOfStairs;
    public GameObject ascendStairs;
    GameObject actor;

    private void Start()
    {
        bottomOfStairs.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collisionWith)
    {
        this.GetComponent<Collider2D>().enabled = false;
        actor = collisionWith.gameObject;
        actor.GetComponent<MovementScript>().isAscending = new Vector2(-1, -1);
        actor.GetComponent<LayerChanger>().ChangeLayer("SubGround");

        StartCoroutine(EnterStairs(actor.transform.position, new Vector2(actor.transform.position.x, actor.transform.position.y - 0.04f), 0.04f));
    }

    private IEnumerator EnterStairs(Vector2 start, Vector2 end, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            //CombatEvents.LockPlayerMovement();

            float t = elapsedTime / duration;
            actor.transform.position = Vector2.Lerp(start, end, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        actor.transform.position = end;
        ascendStairs.GetComponent<BoxCollider2D>().enabled = true;
        bottomOfStairs.SetActive(true);
        //CombatEvents.UnlockPlayerMovement();
    }
}