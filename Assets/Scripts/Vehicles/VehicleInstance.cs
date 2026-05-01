using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class VehicleInstance : MonoBehaviour
{
    public VehiclesMovementScript movementScript;
    public wheelRotation wheelRotation;
    public GameObject horizontalWheelsGO, verticalWheelsGO, bodySpriteU, bodySpriteD, bodySpriteL, bodySpriteR, collidersU, collidersD, collidersL, collidersR;
    public GameObject exitPosU, exitPosD, exitPosL, exitPosR;
    public List<GameObject> passengers = new List<GameObject>();
    [SerializeField] GameObject playerDriving;

    public void EnterVehicle(GameObject GOToEnter)
    { 
        GOToEnter.SetActive(false);
        passengers.Add(GOToEnter);

        if (GOToEnter.tag == "Player")
        { 
            movementScript.enabled = true;
            CameraFollow cameraFoller = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
            cameraFoller.transformToFollow = transform;
            playerDriving = GOToEnter;
            StartCoroutine(FieldEvents.CoolDown(0.3f));
        }
    }

    public void ExitVehicle(GameObject GoToExit)
    {
        Vector2 dir = movementScript.lookDirection;

        if (dir == Vector2.up)
            GoToExit.transform.position = exitPosU.transform.position;

        else if (dir == Vector2.down)
            GoToExit.transform.position = exitPosD.transform.position;

        else if(dir == Vector2.left)
            GoToExit.transform.position = exitPosL.transform.position;

        else
            GoToExit.transform.position = exitPosR.transform.position;

        GoToExit.SetActive(true);
        passengers.Remove(GoToExit);

        if (GoToExit.tag == "Player")
        {
            playerDriving = null;
            movementScript.enabled = false;
            CameraFollow cameraFoller = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
            cameraFoller.transformToFollow = GoToExit.transform;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && playerDriving != null && !FieldEvents.isCoolDownBool)
            ExitVehicle(playerDriving);
    }

}
