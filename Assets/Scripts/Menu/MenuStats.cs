using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuStats : Menu
{
    [SerializeField] PartySO partySO;
    [SerializeField] Button firstButtonToSelect;
    [SerializeField] menuMain menuMain;
    [SerializeField] GameObject arrowGO;

    [Header("Combat Stats UI Elements")]
    [SerializeField] private TextMeshProUGUI hpValue;
    [SerializeField] private TextMeshProUGUI potentialValue;
    [SerializeField] private TextMeshProUGUI strengthValue;
    [SerializeField] private TextMeshProUGUI defenceValue;
    [SerializeField] private TextMeshProUGUI focusValue;

    [Header("XP Stats UI Elements")]
    [SerializeField] private TextMeshProUGUI[] levelValues;
    [SerializeField] private TextMeshProUGUI experienceValue;
    [SerializeField] private TextMeshProUGUI nextLevelValue;

    public GameObject[] playerSpecificStatGOs;
    public GameObject[] spacers;
    public int partyMemberSlot = 0;

    public override void DisplayMenu(bool on)
    {
        arrowGO.SetActive(false);
        partySO = menuMain.playerCombat.party;

        InitializeStats();
        levelValues[0].text = $"Level: {partySO.partyMembers[0].Level}";
        levelValues[1].text = $"Level: {partySO.partyMembers[1].Level}";
        levelValues[2].text = $"Level: {partySO.partyMembers[2].Level}";
        displayContainer.SetActive(on);
    }

    public override void EnterMenu()
    {
        arrowGO.SetActive(true);
        partyMemberSlot = 0;
        menuButtonHighlighted.SetButtonColor(menuButtonHighlighted.highlightedColor);
        menuButtonHighlighted.enabled = false; //this removes the blue underline
        firstButtonToSelect.Select();
    }

    public override void ExitMenu()
    {
        arrowGO.SetActive(false);
        menuButtonHighlighted.enabled = true;
        menuButtonHighlighted.SetButtonColor(Color.white);
        mainButtonToRevert.Select();
        menuManagerUI.menuUpdateMethod = menuManagerUI.main;
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
