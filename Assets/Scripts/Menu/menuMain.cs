using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class menuMain : Menu
{
    public GameObject menuGO;
    public Button firstMenuButton;
    public PlayerPermanentStats playerPermanentStats;

    [SerializeField] TextMeshProUGUI smamsValue;
    [SerializeField] TextMeshProUGUI durationDisplay;
    public MenuSave menuSave;

    private bool isMenuOn = false;

    private IEnumerator CaptureScreenshotAndEnter()
    {
        yield return new WaitForEndOfFrame();
        menuSave.tempScreenshot = ScreenCapture.CaptureScreenshotAsTexture();

        EnterMenu();
    }

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
        menuSave.UpdateSaveSlotUI();
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
            FieldEvents.UpdateTime();
            durationDisplay.text = FieldEvents.duration;
        }
    }

    void ToggleMainMenu(bool on)
    {
        if (on == isMenuOn) return;
        if (on) StartCoroutine(CaptureScreenshotAndEnter()) ; else ExitMenu();
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