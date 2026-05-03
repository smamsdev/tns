using NUnit.Framework;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class VehicleInstance : MonoBehaviour
{
    public VehiclesMovementScript movementScript;
    public wheelRotation wheelRotation;
    public GameObject collidersU, collidersD, collidersL, collidersR, optionalPlaceholderSprite;
    public GameObject exitPosU, exitPosD, exitPosL, exitPosR;
    public Animator bodyAnimator;
    public List<GameObject> passengers = new List<GameObject>();
    [SerializeField] GameObject playerDriving;

    private void Start()
    {
        Vector2 dir = movementScript.lookDirection;

        bodyAnimator.SetFloat("LookDirectionX", movementScript.lookDirection.x);
        bodyAnimator.SetFloat("LookDirectionY", movementScript.lookDirection.y);

        optionalPlaceholderSprite.SetActive(false);

        if (dir == Vector2.up)
        {
            collidersU.SetActive(true);
        }

        else if (dir == Vector2.down)

        {
            collidersD.SetActive(true);
        }


        else if (dir == Vector2.left)
        {
            collidersL.SetActive(true);
        }

        else
        {
            collidersR.SetActive(true);
        }
    }

    public void EnterVehicle(GameObject GOToEnter)
    { 
        GOToEnter.SetActive(false);
        passengers.Add(GOToEnter);

        collidersU.SetActive(false);
        collidersD.SetActive(false);
        collidersL.SetActive(false);
        collidersR.SetActive(false);

        if (GOToEnter.tag == "Player")
        { 
            movementScript.enabled = true;
            CameraFollow cameraFoller = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
            cameraFoller.transformToFollow = transform;
            playerDriving = GOToEnter;
            StartCoroutine(FieldEvents.CoolDown(0.3f));
        }
    }

    private IEnumerator ExitVehicle(GameObject GoToExit)
    {
        Vector2 dir = movementScript.lookDirection;

        CombatEvents.LockPlayerMovement();
        yield return new WaitForSeconds(.5f);
        CombatEvents.UnlockPlayerMovement();

        movementScript.rigidBody2d.bodyType = RigidbodyType2D.Kinematic;

        if (dir == Vector2.up)
        {
            collidersU.SetActive(true);
            GoToExit.transform.position = exitPosU.transform.position;
        }

        else if (dir == Vector2.down)

        {
            collidersD.SetActive(true);
            GoToExit.transform.position = exitPosD.transform.position;
        }


        else if (dir == Vector2.left)
        {
            collidersL.SetActive(true);
            GoToExit.transform.position = exitPosL.transform.position;
        }
           
        else
        {
            collidersR.SetActive(true);
            GoToExit.transform.position = exitPosR.transform.position;
        }

        GoToExit.SetActive(true);
        passengers.Remove(GoToExit);

        if (GoToExit.tag == "Player")
        {
            playerDriving = null;
            movementScript.enabled = false;
            CameraFollow cameraFoller = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
            cameraFoller.transformToFollow = GoToExit.transform;
        }

        yield return null;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && playerDriving != null && !FieldEvents.isCoolDownBool)
            StartCoroutine(ExitVehicle(playerDriving));

        bodyAnimator.SetFloat("LookDirectionX", movementScript.lookDirection.x);
        bodyAnimator.SetFloat("LookDirectionY", movementScript.lookDirection.y);
    }
}
