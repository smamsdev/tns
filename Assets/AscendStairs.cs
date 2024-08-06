using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AscendStairs : MonoBehaviour
{
    public DescendStairs descendStairs;
    GameObject player;

    private void Start()
    {
        this.GetComponent<BoxCollider2D>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collisionWith)
    {
        Debug.Log("ascended");
        collisionWith.GetComponent<LayerChanger>().ChangeLayer("Actors and Buildings");
        collisionWith.GetComponent<PlayerMovementScript>().isAscending = new Vector2(1, 1);

        player = collisionWith.gameObject;

        StartCoroutine(EscapeFromStairs(player.transform.position,new Vector2 (player.transform.position.x, player.transform.position.y-0.03f), 0.05f));
    }

    private IEnumerator EscapeFromStairs(Vector2 start, Vector2 end, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            CombatEvents.LockPlayerMovement();

            float t = elapsedTime / duration;
            player.transform.position = Vector2.Lerp(start, end, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        player.transform.position = end;

        descendStairs.bottomOfStairs.SetActive(false);
        this.GetComponent<BoxCollider2D>().enabled = false;
        descendStairs.GetComponent<BoxCollider2D>().enabled = true;
        CombatEvents.UnlockPlayerMovement();
    }
}
