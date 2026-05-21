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
    [SerializeField] TextMeshProUGUI hpValue;
    [SerializeField] TextMeshProUGUI potentialValue;
    [SerializeField] TextMeshProUGUI strengthValue;
    [SerializeField] TextMeshProUGUI defenceValue;
    [SerializeField] TextMeshProUGUI focusValue;
    [SerializeField] TextMeshProUGUI[] partyMemberLevelTMPs;
    [SerializeField] TextMeshProUGUI[] partyMemberNameTMPs;
    [SerializeField] RawImage[] partyMemberPortraitImages;
    [SerializeField] TextMeshProUGUI experienceValue;
    [SerializeField] TextMeshProUGUI nextLevelValue;
    public GameObject[] playerSpecificStatGOs;
    public GameObject[] spacers;
    public GameObject partyMemberUIPrefabGO, partyMemberUIParentGO;
    public int highlightedIndex = 0;
    public List<PartyMemberHighlightedIU> partyMemberHighlightedIUs = new();

    public override void DisplayMenu(bool on)
    {
        displayContainer.SetActive(on);
    }

    public void initBandPage()
    {
        List<Button> buttons = new List<Button>();

        for (int i = 0; i < partySO.partyMembers.Count; i++)
        {
            GameObject partyMemberUIGO = Instantiate(partyMemberUIPrefabGO, partyMemberUIParentGO.transform);
            PartyMemberHighlightedIU partyMemberHighlightedIU = partyMemberUIGO.GetComponent<PartyMemberHighlightedIU>();

            partyMemberHighlightedIU.onHighlighted = () => PartyMemberHighlighted(partyMemberHighlightedIU.partyMemberCombat);

            if (partySO.partyMembers[i] == null)
            {
                partyMemberNameTMPs[i].text = "Space Available";
                partyMemberPortraitImages[i].enabled = false;
                partyMemberPortraitImages[i].texture = null;
                partyMemberPortraitImages[i].SetNativeSize();
                partyMemberLevelTMPs[i].text = "";
            }

            else
            {
                PartyMemberCombat partyMemberCombat = partySO.partyMembers[i].prefab.GetComponent<PartyMemberCombat>();

                partyMemberUIGO.gameObject.name = "Party Slot " + i + " " + partyMemberCombat.combatantName;
                partyMemberHighlightedIU.partyMemberCombat = partyMemberCombat;
                partyMemberNameTMPs[i].text = partyMemberCombat.combatantName;
                partyMemberPortraitImages[i].enabled = true;
                partyMemberPortraitImages[i].texture = partyMemberCombat.portraitImage;
                partyMemberPortraitImages[i].SetNativeSize();
                partyMemberLevelTMPs[i].text = $"Level: {partySO.partyMembers[i].Level}";
            }

            buttons.Add(partyMemberHighlightedIU.button);
        }

        FieldEvents.SetGridNavigationWrapAround(buttons, 1);
        InitializeStats();

        if (partySO.partyMembers.Count > 1) 
            arrowGO.SetActive(true);

        highlightedIndex = 0;
    }

    public void PartyMemberHighlighted(PartyMemberCombat partyMemberCombat)
    { 
    
    }

    public override void EnterMenu()
    {
        pauseMenuManager.ClearThenDisplayMenu(this);
        arrowGO.SetActive(false);
        partySO = menuMain.playerCombat.partySO;
        partyMemberHighlightedIUs[highlightedIndex].button.Select();
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
        
    public void InitializeStats()
    {
        PartyMemberPermanentStats partyMember = partySO.partyMembers[highlightedIndex];

        if (partyMember is PlayerPermanentStats)
        {
            foreach (GameObject gameObject in playerSpecificStatGOs)
            {
                gameObject.SetActive(true);
            }

            foreach (GameObject gameObject in spacers)
            {
                gameObject.SetActive(false);
            }

            var playerPermanentstats = (PlayerPermanentStats)partyMember;
            focusValue.text = $"{playerPermanentstats.FocusBase}";
            potentialValue.text = $"{playerPermanentstats.CurrentPotential} / {playerPermanentstats.MaxPotential}";
        }

        else
        {
            foreach (GameObject gameObject in playerSpecificStatGOs)
            {
                gameObject.SetActive(false);
            }

            foreach (GameObject gameObject in spacers)
            {
                gameObject.SetActive(true);
            }
        }

        hpValue.text = $"{partyMember.CurrentHP} / {partyMember.MaxHP}";
        strengthValue.text = $"{partyMember.AttackBase}";
        defenceValue.text = $"{partyMember.FendBase}";
        experienceValue.text = $"{partyMember.XP}";
        nextLevelValue.text = $"{partyMember.XPThreshold}";
    }
}
