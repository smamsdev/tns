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

    public bool isMenuOn = false;

    private void Start()
    {
        masterMenuContainer.SetActive(false);
        animator.enabled = false;
    }

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

        menuSave.UpdateSaveSlotUI();

        smamsValue.text = $"{playerCombat.playerPermanentStats.Smams}";

        CombatEvents.LockPlayerMovement();

        animator.Play("MenuOpen");
        animator.enabled = true;

        firstMenuButton.Select();
    }

    public override void ExitMenu() //triggered via animation transition event
    {
        animator.enabled = false;
    }

    void ToggleMainMenu(bool on)
    {
        StartCoroutine(FieldEvents.CoolDown(.1f));

        if (on)
            StartCoroutine(CaptureScreenshotAndEnter());

        else
        {
            animator.Play("MenuClose"); //this will trigger close state via animation event
            CombatEvents.UnlockPlayerMovement();
        }
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !FieldEvents.isCoolDownBool && FieldEvents.menuAvailable)
        {
            isMenuOn = !isMenuOn;
            ToggleMainMenu(isMenuOn);
        }

        if (isMenuOn)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(Time.time);
            string playTimeDuration = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);

            durationDisplay.text = playTimeDuration;
        }
    }
}