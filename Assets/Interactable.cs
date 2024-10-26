using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Interactable : ToTrigger
{
    public string interactionTextToDisplay;
    public Animator animator;
    bool isCollision;
    public bool disableColliderAfterTrigger;

    public TextMeshProUGUI textMeshProUGUI;

    private void Start()
    {
        textMeshProUGUI.text = interactionTextToDisplay;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")

        {
            isCollision = true;
            animator.SetTrigger("OpenInteraction");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")

        {
            isCollision = false;
            animator.SetTrigger("CloseInteraction");
        }
    }

    public override IEnumerator DoAction()

    {
        animator.SetTrigger("CloseInteraction");
        FieldEvents.HasCompleted.Invoke(this.gameObject);

        if (disableColliderAfterTrigger)
        {
            GetComponent<Collider2D>().enabled = false;
        }

        yield return null;
    }

    private void Update()
    {
        if (isCollision)

        {
            if (Input.GetKeyDown(KeyCode.Space))

            {
                StartCoroutine(DoAction());
            }
        }
    }
}
