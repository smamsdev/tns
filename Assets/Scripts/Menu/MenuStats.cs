using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuStats : PauseMenu
{
    [SerializeField] PartySO partySO;
    [SerializeField] menuMain menuMain;
    [SerializeField] GameObject arrowGO;
    [SerializeField] TextMeshProUGUI hpValueTMP, potentialValueTMP, focusValueTMP, strengthValueTMP, defenceValueTMP, experienceValueTMP, nextLevelValueTMP, potentialLabelTMP, focusLabelTMP;
    [SerializeField] PartyMemberPortrait[] partyMemberPortraits;

    public GameObject partyMemberUIPrefabGO, partyMemberUIParentGO, allStatsContainerGO;
    public int highlightedIndex = 0;
    public List<PartyMemberUI> partyMemberUIs = new();

    public override void DisplayMenu(bool on)
    {
        displayContainer.SetActive(on);
    }

    public void InitBandPage()
    {
        List<Button> buttons = new List<Button>();

        foreach (PartyMemberPortrait partyMemberPortrait in partyMemberPortraits)
            partyMemberPortrait.gameObject.SetActive(false);

        partySO = menuMain.playerCombat.partySO;

        for (int i = 0; i < partySO.partyMembers.Count; i++)
        {
            GameObject partyMemberUIGO = Instantiate(partyMemberUIPrefabGO, partyMemberUIParentGO.transform);
            PartyMemberUI partyMemberUI = partyMemberUIGO.GetComponent<PartyMemberUI>();
            partyMemberUIs.Add(partyMemberUI);

            partyMemberUI.onHighlighted = () => PartyMemberHighlighted(partyMemberUI);
            partyMemberUI.onUnHighlighted = () =>
            {
                if (partyMemberUI.partyMemberCombat != null)
                    partyMemberUI.partyMemberPortrait.arrowGO.SetActive(false);
            };

            if (partySO.partyMembers[i] == null)
            {
                partyMemberUIGO.gameObject.name = "Party Slot " + i + "Vacant" ;
                partyMemberUI.nameTMP.text = "Vacant";
                partyMemberUI.levelTMP.text = "Level: --";
                //partyMemberPortraitImages[i].enabled = false;
                //partyMemberPortraitImages[i].texture = null;
                //partyMemberPortraitImages[i].SetNativeSize();
                partyMemberUI.partyMemberPortrait = null;
            }

            else
            {
                PartyMemberCombat partyMemberCombat = partySO.partyMembers[i].prefab.GetComponent<PartyMemberCombat>();

                partyMemberUIGO.gameObject.name = "Party Slot " + i + " " + partyMemberCombat.combatantName;
                partyMemberUI.partyMemberCombat = partyMemberCombat;
                partyMemberUI.nameTMP.text = partyMemberCombat.combatantName;
                partyMemberUI.levelTMP.text = $"Level: {partySO.partyMembers[i].Level}";
                partyMemberUI.partyMemberPortrait = partyMemberPortraits[i];
                partyMemberUI.partyMemberPortrait.portrait.sprite = partyMemberCombat.portraitImage;
                partyMemberPortraits[i].gameObject.SetActive(true);
                partyMemberPortraits[i].arrowGO.SetActive(false);

                //partyMemberPortraitImages[i].enabled = true;
                //partyMemberPortraitImages[i].texture = partyMemberCombat.portraitImage;
                //partyMemberPortraitImages[i].SetNativeSize();

            }

            buttons.Add(partyMemberUIs[i].button);
        }

        FieldEvents.SetGridNavigationWrapAround(buttons, 1);

        if (partySO.partyMembers.Count > 1) 
            arrowGO.SetActive(true);

        highlightedIndex = 0;
    }

    public void PartyMemberHighlighted(PartyMemberUI partyMemberUI)
    { 
        PartyMemberCombat partyMemberCombat = partyMemberUI.partyMemberCombat;

        highlightedIndex = partyMemberUIs.IndexOf(partyMemberUI);
        UpdateStatTMPS(partyMemberCombat);

        if (partyMemberUI.partyMemberCombat != null)
        partyMemberUI.partyMemberPortrait.arrowGO.SetActive(true);
    }

    public override void EnterMenu()
    {
        InitBandPage();
        pauseMenuManager.ClearThenDisplayMenu(this);
        arrowGO.SetActive(false);
        partyMemberUIs[highlightedIndex].button.Select();
    }

    public override void ExitMenu()
    {
        arrowGO.SetActive(false);
        pauseMenuManager.EnterMenu(pauseMenuManager.menuMain);
        pauseMenuManager.menuMain.menuButtonHighlighteds[0].SetButtonNormalColor(Color.white);
        pauseMenuManager.menuMain.menuButtonHighlighteds[0].button.Select();
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitMenu();
        }
    }
        
    public void UpdateStatTMPS(PartyMemberCombat partyMemberCombat)
    {
        if (partyMemberCombat == null)
        {
            allStatsContainerGO.SetActive(false);
            return;
        }

        allStatsContainerGO.SetActive(true);

        PartyMemberPermanentStats stats = partyMemberCombat.partyMemberPermanentStats;

        hpValueTMP.text = $"{stats.CurrentHP} / {stats.MaxHP}";
        strengthValueTMP.text = $"{stats.AttackBase}";
        defenceValueTMP.text = $"{stats.FendBase}";
        experienceValueTMP.text = $"{stats.XP}";
        nextLevelValueTMP.text = $"{stats.XPThreshold}";

        if (partyMemberCombat is PlayerCombat playerCombat)
        {
            var playerPermanentstats = playerCombat.playerPermanentStats;
            potentialLabelTMP.text = "Potential:";
            potentialValueTMP.text = $"{playerPermanentstats.CurrentPotential} / {playerPermanentstats.MaxPotential}";
            focusLabelTMP.text = "Focus:";
            focusValueTMP.text = $"{playerPermanentstats.FocusBase}";
        }

        else
        {
            potentialLabelTMP.text = "";
            potentialValueTMP.text = "";
            focusLabelTMP.text = "";
            focusValueTMP.text = "";
        }
    }
}
