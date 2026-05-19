using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEditor;
using static PlayerMoveInventorySO;

public class MenuMoves : PauseMenu
{
    public bool isSelectingMove;
    public MenuMoveInventory menuMoveInventory;
    public PlayerMoveInventorySO playerMoveInventorySO;
    public int highlightedButtonIndex;
    public TextMeshProUGUI headerTMP, moveNameTMP, moveDescriptionTMP, probabilityTMP, movePotentialChangeTMP, moveEquipStatusTMP;
    public TextMeshProUGUI violentHeaderTMP, cautiousHeaderTMP, preciseHeaderTMP;
    public TextMeshProUGUI[] headerButtonsTMP;
    public List<MenuButtonHighlighted> allMenuButtonHighlighteds;
    public MenuMoveEquipSlotSelect[] menuMoveEquipSlotSelects;

    private void Start()
    {
        playerMoveInventorySO = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>().playerMoveInventorySO;
    }

    public override void DisplayMenu(bool on)
    {
        ClearAllDescriptionTMPs();
        displayContainer.SetActive(on);
    }

    public void ClearAllDescriptionTMPs()
    {
        headerTMP.text = "";
        moveNameTMP.text = "";
        probabilityTMP.text = "";
        moveDescriptionTMP.text = "";
        movePotentialChangeTMP.text = "";
        moveEquipStatusTMP.text = "";
    }

    public override void EnterMenu()
    {
        pauseMenuManager.ClearThenDisplayMenu(this);
        InitAllEquippedMovesToUISlots();
        InitializeMainButtons();

        ClearAllDescriptionTMPs();
        SetAlphaAllMenuButtons(1);

        allMenuButtonHighlighteds[highlightedButtonIndex].button.Select();
    }

    public override void ExitMenu()
    {
        pauseMenuManager.EnterMenu(pauseMenuManager.menuMain);
        pauseMenuManager.menuMain.menuButtonHighlighteds[2].SetButtonNormalColor(Color.white);
        pauseMenuManager.menuMain.menuButtonHighlighteds[2].button.Select();
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitMenu();
        }
    }

    void MoveArraySelected(MenuButtonHighlighted arrayHighlighted)
    {
        pauseMenuManager.EnterMenu(menuMoveEquipSlotSelects[highlightedButtonIndex]);
        FieldEvents.SetTextColor(allMenuButtonHighlighteds[highlightedButtonIndex].tmp, Color.yellow, 1);
        SetAlphaAllMenuButtons(.7f);
    }

    public void SetAlphaAllMenuButtons(float alpha)
    { 
        foreach (MenuButtonHighlighted menuButtonHighlighted in allMenuButtonHighlighteds)
            FieldEvents.SetTextColor(menuButtonHighlighted.tmp, menuButtonHighlighted.tmp.color, alpha);

        foreach (TextMeshProUGUI tmp in headerButtonsTMP)
        {
            FieldEvents.SetTextColor(tmp, tmp.color, alpha);
        }
    }

    public void InitializeMainButtons()
    {
        List<Button> buttons = new List<Button>();

        foreach (MenuButtonHighlighted menuButtonHighlighted in allMenuButtonHighlighteds)
        {
            buttons.Add(menuButtonHighlighted.button);

            menuButtonHighlighted.onHighlighted = () => HandleMainHighlight(menuButtonHighlighted);

            menuButtonHighlighted.onUnHighlighted = () => FieldEvents.SetTextColor(menuButtonHighlighted.tmp, Color.white, 1);
            menuButtonHighlighted.button.onClick.AddListener(() => MoveArraySelected(menuButtonHighlighted));
        }

        FieldEvents.SetGridNavigationWrapAround(buttons, 3);
    }

    void SetHighlightedIndexInt(MenuButtonHighlighted menuButtonHighlighted)
    { 
        highlightedButtonIndex = allMenuButtonHighlighteds.IndexOf(menuButtonHighlighted);
    }

    void HeaderColor(TextMeshProUGUI textMeshProUGUI, Color color)
    {
        violentHeaderTMP.color = Color.white;
        cautiousHeaderTMP.color = Color.white;
        preciseHeaderTMP.color = Color.white;

        textMeshProUGUI.color = color;
    }

    void HighlightViolentListType()
    {
        HeaderColor(violentHeaderTMP, Color.yellow);
        HideAllButtonTMPS();
        DisplayButtonTMP(allMenuButtonHighlighteds[0]);
        DisplayButtonTMP(allMenuButtonHighlighteds[1]);
        DisplayButtonTMP(allMenuButtonHighlighteds[2]);
    }

    void HighlightCautiousListType()
    {
        HeaderColor(cautiousHeaderTMP, Color.yellow);
        HideAllButtonTMPS();
        DisplayButtonTMP(allMenuButtonHighlighteds[3]);
        DisplayButtonTMP(allMenuButtonHighlighteds[4]);
        DisplayButtonTMP(allMenuButtonHighlighteds[5]);
    }

    void HighlightPreciseListType()
    {
        HeaderColor(preciseHeaderTMP, Color.yellow);
        HideAllButtonTMPS();
        DisplayButtonTMP(allMenuButtonHighlighteds[6]);
        DisplayButtonTMP(allMenuButtonHighlighteds[7]);
        DisplayButtonTMP(allMenuButtonHighlighteds[8]);
    }

    void DisplaySlotsOfType()
    { 
        foreach (MenuMoveEquipSlotSelect menuSlotListOfType in menuMoveEquipSlotSelects)
            menuSlotListOfType.gameObject.SetActive(false);

        menuMoveEquipSlotSelects[highlightedButtonIndex].gameObject.SetActive(true);
    }

    void HandleMainHighlight(MenuButtonHighlighted button)
    {
        //update index
        highlightedButtonIndex = allMenuButtonHighlighteds.IndexOf(button);

        //hide everything
        foreach (var b in allMenuButtonHighlighteds)
            b.tmp.enabled = false;

        foreach (MenuMoveEquipSlotSelect menuSlotListOfType in menuMoveEquipSlotSelects)
            menuSlotListOfType.gameObject.SetActive(false);

        //display correct slots and types
        menuMoveEquipSlotSelects[highlightedButtonIndex].gameObject.SetActive(true);

        int i = highlightedButtonIndex;

        if (i < 3)
            HighlightViolentListType();
        else if (i < 6)
            HighlightCautiousListType();
        else
            HighlightPreciseListType();

        //update descriptionTMP
        MoveType moveType = menuMoveEquipSlotSelects[highlightedButtonIndex].moveType;
        UpdateMoveListDescription(moveType);

        //color button
        FieldEvents.SetTextColor(button.tmp, Color.yellow, 1);
    }

    void DisplayButtonTMP(MenuButtonHighlighted menuButtonHighlighted)
    {
        menuButtonHighlighted.tmp.enabled = true;
    }

    void HideAllButtonTMPS()
    {
        foreach (MenuButtonHighlighted menuButtonHighlighted in allMenuButtonHighlighteds)
            menuButtonHighlighted.tmp.enabled = false;
    }

    public void InitAllEquippedMovesToUISlots()
    {
        foreach (MenuMoveEquipSlotSelect menuMoveEquipSlotSelect in menuMoveEquipSlotSelects)
        {
           menuMoveEquipSlotSelect.InitMoveEquipSlotList();
        }
    }

    void UpdateMoveListDescription(MoveType moveType)
    {
        switch (moveType)
        {
            case MoveType.ViolentAttack:
                moveNameTMP.text = "Violent Attacks";
                probabilityTMP.text = "Execute savage attacks with a heavy price";
                break;
 
            case MoveType.ViolentFend:
                moveNameTMP.text = "Violent Fends";
                probabilityTMP.text = "Protect yourself while still being dangerous";
                break;

            case MoveType.ViolentFocus:
                moveNameTMP.text = "Violent Focuses";
                probabilityTMP.text = "Gather strength at the cost of everything else";
                break;

            case MoveType.CautiousAttack:
                moveNameTMP.text = "Cautious Attacks";
                probabilityTMP.text = "Deliver damage without becoming vulnerable";
                break;

            case MoveType.CautiousFend:
                moveNameTMP.text = "Cautious Fends";
                probabilityTMP.text = "Protect yourself as a priority";
                break;

            case MoveType.CautiousFocus:
                moveNameTMP.text = "Cautious Focuses";
                probabilityTMP.text = "Grow stronger while minimising risk";
                break;

            case MoveType.PreciseAttack:
                moveNameTMP.text = "Precises Attacks";
                probabilityTMP.text = "Break down your enemies";
                break;

            case MoveType.PreciseFend:
                moveNameTMP.text = "Precise Fends";
                probabilityTMP.text = "Gaurd exploitation with expertise";
                break;

            case MoveType.PreciseFocus:
                moveNameTMP.text = "Precise Focuses";
                probabilityTMP.text = "Sharpen your potential";
                break;

            default:
                Debug.Log("something went wrong");
                break;
        }
    }


}