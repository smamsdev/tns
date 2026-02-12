using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuStats : PauseMenu
{
    [SerializeField] PartySO partySO;
    [SerializeField] Button firstButtonToSelect;
    [SerializeField] menuMain menuMain;
    [SerializeField] GameObject arrowGO;
    [SerializeField] List<Button> characterButtons = new List<Button>();

    [SerializeField] TextMeshProUGUI hpValue;
    [SerializeField] TextMeshProUGUI potentialValue;
    [SerializeField] TextMeshProUGUI strengthValue;
    [SerializeField] TextMeshProUGUI defenceValue;
    [SerializeField] TextMeshProUGUI focusValue;

    [SerializeField] GameObject[] partyMemberHeaderGOs;
    [SerializeField] TextMeshProUGUI[] partyMemberLevelTMPs;
    [SerializeField] TextMeshProUGUI[] partyMemberNameTMPs;
    [SerializeField] RawImage[] partyMemberPortraitImages;

    [SerializeField] TextMeshProUGUI experienceValue;
    [SerializeField] TextMeshProUGUI nextLevelValue;


    public GameObject[] playerSpecificStatGOs;
    public GameObject[] spacers;
    public int partyMemberSlot = 0;

    public override void DisplayMenu(bool on)
    {
        displayContainer.SetActive(on);
    }

    public override void EnterMenu()
    {
        pauseMenuManager.ClearThenDisplayMenu(this);
        arrowGO.SetActive(false);
        partySO = menuMain.playerCombat.party;

        foreach (var gameObject in partyMemberHeaderGOs)
        {
            gameObject.SetActive(false);
        }

        foreach (var rawImage in partyMemberPortraitImages)
        {
            rawImage.enabled = false;
        }

        List<Button> characterButtonInstances = new List<Button>();

        for (int i = 0; i < partySO.partyMembers.Count; i++)
        {
            PartyMemberCombat partyMemberCombat = partySO.partyMembers[i].prefab.GetComponent<PartyMemberCombat>();

            characterButtonInstances.Add(characterButtons[i]);
            partyMemberHeaderGOs[i].SetActive(true);
            partyMemberNameTMPs[i].text = partyMemberCombat.combatantName;
            partyMemberPortraitImages[i].enabled = true;
            partyMemberPortraitImages[i].texture = partyMemberCombat.portraitImage;
            partyMemberPortraitImages[i].SetNativeSize();
            partyMemberLevelTMPs[i].text = $"Level: {partySO.partyMembers[i].Level}";
        }

        FieldEvents.SetGridNavigationWrapAround(characterButtonInstances, 1);

        InitializeStats();


        if (partySO.partyMembers.Count > 1) arrowGO.SetActive(true);

        partyMemberSlot = 0;
        firstButtonToSelect.Select();
    }

    public override void ExitMenu()
    {
        arrowGO.SetActive(false);
        pauseMenuManager.EnterMenu(pauseMenuManager.main);
        pauseMenuManager.menuUpdateMethod.lastParentButtonSelected.SetButtonNormalColor(Color.white);
        pauseMenuManager.menuUpdateMethod.lastParentButtonSelected.button.Select();
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
        PartyMemberSO partyMember = partySO.partyMembers[partyMemberSlot];

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
