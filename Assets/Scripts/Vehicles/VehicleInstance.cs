using NUnit.Framework;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Unity.VisualScripting;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using static UnityEngine.RuleTile.TilingRuleOutput;
using TMPro;
using System.Security.Claims;

public class VehicleInstance : MonoBehaviour
{
    public VehiclesMovementScript movementScript;
    public string vehicleName;
    public float batteryCharge;
    public float chargeUsageFactor;
    public InventorySO batteryInventorySO;
    public wheelRotation wheelRotation;
    public GameObject collidersU, collidersD, collidersL, collidersR, bodyU, bodyD, bodyL, bodyR, wheelsU, wheelsD, wheelsL, wheelsR;
    public GameObject exitPosU, exitPosD, exitPosL, exitPosR, vehicleUI;
    public TextMeshProUGUI vehicleUIChargeTMP;
    public Animator bodyAnimator, vibrationAnimator, chargeMenuTransitionAnimator, vehicleUIChargeTMPAnimator;
    public List<GameObject> passengers = new List<GameObject>();
    public GameObject playerDriving;

    private void FixedUpdate()
    {
        if (playerDriving != null)
            UpdateBatteryCharge();
    }

    public void UpdateBatteryCharge()
    {
        EquipmentInstance battery = batteryInventorySO.gearInstanceInventory[0] as EquipmentInstance;
        float batteryDrain = (movementScript.distance * chargeUsageFactor);
        battery.RemoveCharge(batteryDrain);
        batteryCharge = battery.Charge;
        vehicleUIChargeTMP.text = battery.QuantityString();
        vehicleUIChargeTMPAnimator.SetFloat("Charge", battery.ChargePercentage());
    }

    private void Start()
    {
        //this is dumb but will prevent other bodies from showing if they were accidentally left on in edit mode
        {
            bodyAnimator.gameObject.SetActive(false);
            bodyU.SetActive(false);
            bodyD.SetActive(false);
            bodyL.SetActive(false);
            bodyR.SetActive(false);
            wheelsU.SetActive(false);
            wheelsD.SetActive(false);
            wheelsL.SetActive(false);
            wheelsR.SetActive(false);
            collidersU.SetActive(false);
            collidersD.SetActive(false);
            collidersL.SetActive(false);
            collidersR.SetActive(false);
            vehicleUI.SetActive(false);
            bodyAnimator.gameObject.SetActive(true);
        }

        Vector2 dir = movementScript.lookDirection;

        bodyAnimator.SetFloat("LookDirectionX", movementScript.lookDirection.x);
        bodyAnimator.SetFloat("LookDirectionY", movementScript.lookDirection.y);

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
        vehicleUI.SetActive(true);
        vibrationAnimator.enabled = true;
        chargeMenuTransitionAnimator.Play("OpenMenu");
        UpdateBatteryCharge();
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

        vibrationAnimator.enabled = false;
        CombatEvents.LockPlayerMovement();
        yield return new WaitForSeconds(.5f);
        CombatEvents.UnlockPlayerMovement();
        UpdateBatteryCharge();

        //disables UI GO via animation event
        chargeMenuTransitionAnimator.Play("CloseMenu");

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

        if (passengers != null && batteryCharge <= 0)
            vibrationAnimator.enabled = false;

        bodyAnimator.SetFloat("LookDirectionX", movementScript.lookDirection.x);
        bodyAnimator.SetFloat("LookDirectionY", movementScript.lookDirection.y);
    }
}
