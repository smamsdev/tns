using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class menuUI : MonoBehaviour
{
    public GameObject menuGO;
    public bool isMenuOn;
    public Button firstMenuItem;

    public MenuStats firstMenuStats;
    [SerializeField] private PlayerPermanentStats playerPermanentStats;

    [SerializeField] TextMeshProUGUI smamsValue;
    [SerializeField] TextMeshProUGUI timeValue;

    private void Start()
    {
        menuGO.SetActive(false);
        isMenuOn = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu(isMenuOn);
        }

        if (isMenuOn)
        {
            UpdateTime();
        }
    }

    void ToggleMenu(bool on)

    {
        isMenuOn = !isMenuOn;
        firstMenuStats.InitializeStats();
        menuGO.SetActive(!on);
        firstMenuItem.Select();
        CombatEvents.LockPlayerMovement();
        smamsValue.text = $"{playerPermanentStats.smams}";
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


}
