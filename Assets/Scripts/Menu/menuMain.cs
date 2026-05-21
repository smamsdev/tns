using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.UI.Image;

public class menuMain : PauseMenu
{
    public Animator animator;

    public MenuButtonHighlighted[] menuButtonHighlighteds;

    [SerializeField] TextMeshProUGUI locationTMP;
    [SerializeField] TextMeshProUGUI smamsValue;
    [SerializeField] TextMeshProUGUI durationDisplay;

    public MenuSave menuSave;
    public MenuGearInventorySubPage menuGearInventorySubPage;
    public PlayerCombat playerCombat;
    public List<GameObject> extraSpacerGOs;
    public List<Button> activeButtons = new();

    public GameObject masterMenuContainer, firstSpacerGO;

    public bool isMenuOn = false;

    private void Start()
    {
        masterMenuContainer.SetActive(false);
        animator.enabled = false;
    }

    void HideUnavailableSideButtons()
    {
        foreach (MenuButtonHighlighted menuButtonHighlighted in menuButtonHighlighteds) 
            menuButtonHighlighted.gameObject.SetActive(true);

        if (!playerCombat.playerPermanentStats.IsStatsMenuAvailable)
            DisableButton(menuButtonHighlighteds[0].gameObject);

        if (!playerCombat.playerInventorySO.IsGearMenuAvailable)
            DisableButton(menuButtonHighlighteds[1].gameObject);

        if (!playerCombat.playerMoveInventorySO.IsMoveMenuAvailable)
            DisableButton(menuButtonHighlighteds[2].gameObject);
    }

    void DisableButton(GameObject GOTodisable)
    {
        GOTodisable.SetActive(false);
        GameObject newSpacer = Instantiate(firstSpacerGO, firstSpacerGO.transform.parent, true);
        newSpacer.transform.SetSiblingIndex(firstSpacerGO.transform.GetSiblingIndex()+1);
        newSpacer.name = "spacer filler for disabled " + GOTodisable.name;
        extraSpacerGOs.Add(newSpacer);
    }
       
    void WireButtons()
    {
        for (int i = 0; i < menuButtonHighlighteds.Length; i++)
        {
            int index = i;

            menuButtonHighlighteds[i].onHighlighted = () =>
            {
              //  blueUnderlines[index].SetActive(true);
            };

            menuButtonHighlighteds[i].onUnHighlighted = () =>
            {
              //  blueUnderlines[index].SetActive(false);
            };

            foreach (MenuButtonHighlighted menuButtonHighlighted in menuButtonHighlighteds)
            {
                if (menuButtonHighlighted.gameObject.activeSelf)
                    activeButtons.Add(menuButtonHighlighted.button);
            }

            FieldEvents.SetGridNavigationWrapAround(activeButtons, activeButtons.Count);
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
        playerCombat = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>();

        HideUnavailableSideButtons();
        WireButtons();
        pauseMenuManager.ClearThenDisplayMenu(this);

        if (EventSystem.current == null)
            Debug.LogError("🚨 you are missing your EventSystem 🚨\nButtons will not work.");

        locationTMP.text = FieldEvents.sceneName;
        isMenuOn = true;
        masterMenuContainer.SetActive(true);
        menuSave.UpdateSaveSlotUI();
        smamsValue.text = $"{playerCombat.playerPermanentStats.Smams}";
        CombatEvents.LockPlayerMovement();
        animator.Play("OpenMenu");
        animator.enabled = true;

        activeButtons[0].Select();
    }

    public override void ExitMenu() //triggered via animation transition event
    {
        animator.enabled = false;
        masterMenuContainer.SetActive(false);

        foreach (GameObject spacer in extraSpacerGOs)
            GameObject.Destroy(spacer);

        extraSpacerGOs.Clear();
        activeButtons.Clear();
        CombatEvents.UnlockPlayerMovement();
    }

    void ToggleMainMenu(bool open)
    {
        if (open)
        {
            StartCoroutine(CaptureScreenshotAndEnter());
        }
        else
        {
            animator.Play("CloseMenu");
            CombatEvents.UnlockPlayerMovement();
        }
    }

    bool CanOpenMenu()
    {
        return !FieldEvents.isCoolDownBool && !FieldEvents.movementLocked;
    }


    bool CanCloseMenu()
    {
        return !FieldEvents.isCoolDownBool;
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isMenuOn)
            {
                if (!CanOpenMenu())
                    return;

                StartCoroutine(FieldEvents.CoolDown(.1f));

                isMenuOn = true;
                ToggleMainMenu(true);
            }
            else
            {
                if (!CanCloseMenu())
                    return;

                StartCoroutine(FieldEvents.CoolDown(.1f));

                isMenuOn = false;
                ToggleMainMenu(false);
            }
        }

        if (isMenuOn)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(Time.time);
            string playTimeDuration = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);

            durationDisplay.text = playTimeDuration;
        }
    }
}