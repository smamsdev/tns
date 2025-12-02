using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class menuMain : Menu
{
    public GameObject menuGO;
    public Button firstMenuButton;
    public Animator animator;

    [SerializeField] TextMeshProUGUI smamsValue;
    [SerializeField] TextMeshProUGUI durationDisplay;
    
    public MenuSave menuSave;
    public MenuGear menuGear;
    public PlayerCombat playerCombat;

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
        playerCombat = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>();

        isMenuOn = true;
        menuGO.SetActive(true);
        animator.SetBool("Open", true);
        firstMenuButton.Select(); // Ihandler uses this to trigger DisplayMenu method

        menuSave.UpdateSaveSlotUI();
        menuGear.InstantiateUIGearSlots();

        smamsValue.text = $"{playerCombat.playerPermanentStats.Smams}";

        StartCoroutine(LockMovementAfterDelay());
    }

    IEnumerator CloseMenuAnimation()
    {
        animator.SetBool("Open", false);
        yield return new WaitForSeconds(0.25f);
        menuGO.SetActive(false);
        CombatEvents.UnlockPlayerMovement();
    }

    public override void ExitMenu()
    {
        isMenuOn = false;
        menuGear.DeleteAllInventoryUI();
        StartCoroutine(CloseMenuAnimation());
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

    IEnumerator LockMovementAfterDelay()
    {
        yield return new WaitForSeconds(.75f);
        CombatEvents.LockPlayerMovement();
    }
}