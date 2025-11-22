using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]

public abstract class ColliderInteractableAbstract : ToTrigger
{
    public Animator animator;
    public Collider2D playerInTrigger;
    public TextMeshProUGUI signPostTMP;
    public Image bGImage;

    void Reset()
    {
        animator = GetComponent<Animator>();
        playerInTrigger = GetComponent<Collider2D>();
        playerInTrigger.isTrigger = true;
    }

    private void Start()
    {
        bGImage.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInTrigger = collision;
            animator.Play("Open");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (playerInTrigger != null)
        {
            if (collision.CompareTag("Player"))
            {
                playerInTrigger = null;
                animator.Play("Close");
            }
        }
    }

    private void Update()
    {
        if (playerInTrigger && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Triggered());
        }
    }
}
