using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class menuMain : Menu
{
    public Button firstMenuButton;
    public Animator animator;

    public GameObject[] blueUnderlines;
    public MenuButtonHighlighted[] sideButtonsHighlighted;

    [SerializeField] TextMeshProUGUI locationTMP;
    [SerializeField] TextMeshProUGUI smamsValue;
    [SerializeField] TextMeshProUGUI durationDisplay;

    public MenuSave menuSave;
    public MenuGearInventorySubPage menuGearInventorySubPage;
    public PlayerCombat playerCombat;

    public GameObject masterMenuContainer;

    private bool isMenuOn = false;

    void WireButtons()
    {
        for (int i = 0; i < sideButtonsHighlighted.Length; i++)
        {
            int index = i;

            sideButtonsHighlighted[i].onHighlighted = () =>
            {
                blueUnderlines[index].SetActive(true);
            };

            sideButtonsHighlighted[i].onUnHighlighted = () =>
            {
                blueUnderlines[index].SetActive(false);
            };
        }
    }

    private IEnumerator CaptureScreenshotAndEnter()
    {
        displayContainer.SetActive(true);
        yield return new WaitForEndOfFrame();
        menuSave.tempScreenshot = ScreenCapture.CaptureScreenshotAsTexture();

        EnterMenu();
    }

    public override void DisplayMenu(bool on)
    {
        displayContainer.SetActive(on);
    }

    public override void EnterMenu()
    {
        WireButtons();

        menuManagerUI.ClearThenDisplayMenu(this);

        if (EventSystem.current == null)
        {
            Debug.LogError("🚨 you are missing your EventSystem 🚨\nButtons will not work.");
        }

        playerCombat = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>();
        locationTMP.text = FieldEvents.sceneName;
        isMenuOn = true;

        masterMenuContainer.SetActive(true);
        animator.SetBool("Open", true);
        firstMenuButton.Select();

        menuSave.UpdateSaveSlotUI();

        smamsValue.text = $"{playerCombat.playerPermanentStats.Smams}";

        StartCoroutine(LockMovementAfterDelay());
    }

    IEnumerator CloseMenuAnimation()
    {
        animator.SetBool("Open", false);
        yield return new WaitForSeconds(0.25f);
        //menuGO.SetActive(false);
        CombatEvents.UnlockPlayerMovement();
    }

    public override void ExitMenu()
    {
        isMenuOn = false;
        StartCoroutine(CloseMenuAnimation());
    }

    void ToggleMainMenu(bool on)
    {
        if (on == isMenuOn) 
            return;

        if (on) 
            StartCoroutine(CaptureScreenshotAndEnter()) ; 
        
        else 
            ExitMenu();
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isMenuOn && FieldEvents.movementLocked || FieldEvents.isShopping) return;
            //

            ToggleMainMenu(!isMenuOn);

            if (isMenuOn)
            {
                TimeSpan timeSpan = TimeSpan.FromSeconds(Time.time);
                string playTimeDuration = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);

                durationDisplay.text = playTimeDuration;
            }
        }
    }

    IEnumerator LockMovementAfterDelay()
    {
        yield return new WaitForSeconds(.75f);
        CombatEvents.LockPlayerMovement();
    }
}