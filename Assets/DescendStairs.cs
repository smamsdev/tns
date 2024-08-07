using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescendStairs : MonoBehaviour
{
    public GameObject bottomOfStairs;
    public GameObject ascendStairs;
    GameObject player;

    private void Start()
    {
        bottomOfStairs.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collisionWith)
    {
        this.GetComponent<Collider2D>().enabled = false;
        Debug.Log("descending");
        player = collisionWith.gameObject;
        player.GetComponent<PlayerMovementScript>().isAscending = new Vector2(-1, -1);
        player.GetComponent<LayerChanger>().ChangeLayer("SubGround");

        StartCoroutine(EnterStairs(player.transform.position, new Vector2(player.transform.position.x, player.transform.position.y - 0.04f), 0.04f));
    }

    private IEnumerator EnterStairs(Vector2 start, Vector2 end, float duration)
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
        ascendStairs.GetComponent<BoxCollider2D>().enabled = true;
        bottomOfStairs.SetActive(true);
        CombatEvents.UnlockPlayerMovement();
    }
}