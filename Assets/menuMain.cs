using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class menuMain : Menu
{
    public GameObject menuGO;
    public bool isMenuOn;

    public Button firstMenuButton;

    [SerializeField] private PlayerPermanentStats playerPermanentStats;

    [SerializeField] TextMeshProUGUI smamsValue;
    [SerializeField] TextMeshProUGUI timeValue;

    public override void DisplayMenu(bool on)
    {
        throw new NotImplementedException();
    }

    public override void EnterMenu()
    {
        CombatEvents.LockPlayerMovement();
    }

    public override void ExitMenu()
    {
        CombatEvents.UnlockPlayerMovement();
    }

    private void Start()
    {
        menuGO.SetActive(false);
        isMenuOn = false;
    }

    void Update()
    {
        if (isMenuOn)
        {
            UpdateTime();
        }
    }

    void ToggleMainMenu(bool on)

    {
        if (!isMenuOn)
        {
            isMenuOn = true;
            menuGO.SetActive(true);
            firstMenuButton.Select();
            CombatEvents.LockPlayerMovement();
            smamsValue.text = $"{playerPermanentStats.smams}";
            return;
        }

        if (isMenuOn) 
        {
            CombatEvents.UnlockPlayerMovement();
            menuGO.SetActive(false);
            isMenuOn = false;
        }
    }

    void UpdateTime()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(Time.time);

        string formattedTime = string.Format("{0:D2}:{1:D2}:{2:D2}",
                                             timeSpan.Hours,
                                             timeSpan.Minutes,
                                             timeSpan.Seconds);

        timeValue.text = formattedTime;
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMainMenu(isMenuOn);
        }
    }
}