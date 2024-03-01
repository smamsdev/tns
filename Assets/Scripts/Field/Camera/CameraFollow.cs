using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform;
    public float cameraSpeed;
    public float xOffset;
    public float yOffset;
    public bool battleModeOn;
    public Vector3 newPos;

    float floatValueFromCoRoutine;
    float testFloat;

    private void OnEnable()
    {
        CombatEvents.BattleMode += CameraBattleMode;
    }

    private void OnDisable()
    {
        CombatEvents.BattleMode -= CameraBattleMode;
    }

    private void Start()
    {
        transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, (playerTransform.position.z - 10));
       // newPos = new Vector3(playerTransform.position.x, playerTransform.position.y, (playerTransform.position.z - 10));
    }

    private void FixedUpdate()
    {
       newPos = new Vector3(playerTransform.position.x + xOffset, playerTransform.position.y + yOffset, -10f);
       transform.position = Vector3.Slerp(transform.position, newPos, cameraSpeed*Time.deltaTime);
    }


    public void CameraBattleMode(bool on)
    {

        battleModeOn = on;
        if (on) 
        
        {
            StartCoroutine(UpdateCameraXOffsetOverTimeCoRoutine(0, 2, 0.5f));
            StartCoroutine(UpdateCameraSpeedCoRoutine(3, 0, 2));
        }

        else if (!on)

        {
            StartCoroutine(UpdateCameraXOffsetOverTimeCoRoutine(2, 0, 0.5f));
            StartCoroutine(UpdateCameraSpeedCoRoutine(0, 3, 2));
        }
    }

    public IEnumerator UpdateCameraSpeedCoRoutine(float currentCameraSpeed, float finalCameraSpeed, float time)
    {
        float elapsedTime = 0;
        while (elapsedTime < time)
    
        {
            cameraSpeed = Mathf.Lerp(currentCameraSpeed, finalCameraSpeed, elapsedTime/time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    
    public IEnumerator UpdateCameraXOffsetOverTimeCoRoutine(float currentXOffset, float finalXOffset, float time)
    {
        float elapsedTime = 0;
        while (elapsedTime < time)
    
        {
            xOffset = Mathf.Lerp(currentXOffset, finalXOffset, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }




}
