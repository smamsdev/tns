using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class menuMain : Menu
{
    public GameObject menuGO;
    public Button firstMenuButton;
    public PlayerPermanentStats playerPermanentStats;

    [SerializeField] TextMeshProUGUI smamsValue;
    [SerializeField] TextMeshProUGUI timeValue;

    private bool isMenuOn = false;

    public override void DisplayMenu(bool on)
    {
        return;
    }

    public override void EnterMenu()
    {
        isMenuOn = true;
        menuGO.SetActive(true);
        firstMenuButton.Select(); // Ihandler uses this to trigger DisplayMenu method
        CombatEvents.LockPlayerMovement();
        smamsValue.text = $"{playerPermanentStats.smams}";
    }

    public override void ExitMenu()
    {
        isMenuOn = false;
        menuGO.SetActive(false);
        CombatEvents.UnlockPlayerMovement();
    }

    private void Start()
    {
        menuGO.SetActive(false);
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
        if (on == isMenuOn) return;
        if (on) EnterMenu(); else ExitMenu();
    }

    void UpdateTime()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(Time.time);
        timeValue.text = string.Format("{0:D2}:{1:D2}:{2:D2}",
                                       timeSpan.Hours,
                                       timeSpan.Minutes,
                                       timeSpan.Seconds);
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isMenuOn && FieldEvents.movementLocked) return;
            ToggleMainMenu(!isMenuOn);
        }
    }
}