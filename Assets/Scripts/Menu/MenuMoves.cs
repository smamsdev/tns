using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEditor;

public class MenuMoves : PauseMenu
{
    public GameObject moveDescriptionGO;
    public bool isSelectingMove;
    public PlayerMoveManager playerMoveManager;
    public MenuMoveInventory menuMoveInventory;

    public MenuButtonHighlighted[] violentButtonHighlighteds;
    public MenuButtonHighlighted[] cautiousButtonHighlighteds;
    public MenuButtonHighlighted[] preciseButtonHighlighteds;

    public List<MenuButtonHighlighted> allMenuButtonHighlighteds;

    public int highlightedButtonIndex;

    public TextMeshProUGUI moveDescriptions, movePropertyTMP;
    public TextMeshProUGUI violentHeaderTMP, cautiousHeaderTMP, preciseHeaderTMP;

    public GameObject[] slotsParents;

    public MoveSlotUI[] violentAttackSlots = new MoveSlotUI[5];
    public MoveSlotUI[] violentFendSlots = new MoveSlotUI[5];
    public MoveSlotUI[] violentFocusSlots = new MoveSlotUI[5];
    public MoveSlotUI[] cautiousAttackSlots = new MoveSlotUI[5];
    public MoveSlotUI[] cautiousFendSlots = new MoveSlotUI[5];
    public MoveSlotUI[] cautiousFocusSlots = new MoveSlotUI[5];
    public MoveSlotUI[] preciseAttackSlots = new MoveSlotUI[5];
    public MoveSlotUI[] preciseFendSlots = new MoveSlotUI[5];
    public MoveSlotUI[] preciseFocusSlots = new MoveSlotUI[5];

    private void OnEnable()
    {
        playerMoveManager = GameObject.Find("Player").GetComponentInChildren<PlayerMoveManager>();
    }

    public override void DisplayMenu(bool on)
    {
        moveDescriptionGO.SetActive(false);
        displayContainer.SetActive(on);
    }

    public override void EnterMenu()
    {
        pauseMenuManager.ClearThenDisplayMenu(this);
        moveDescriptionGO.SetActive(true);
        LoadAllEquippedMovesToUISlots();
        InitializeMainButtons();
        violentButtonHighlighteds[0].button.Select();
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

    public void InitializeMainButtons()
    {
        List<Button> buttons = new List<Button>();

        foreach (MenuButtonHighlighted menuButtonHighlighted in allMenuButtonHighlighteds)
        {
            buttons.Add(menuButtonHighlighted.button);

            // Single source of truth for highlight behavior
            menuButtonHighlighted.onHighlighted = () => HandleMainHighlight(menuButtonHighlighted);

            menuButtonHighlighted.onUnHighlighted = () => FieldEvents.SetTextColor(menuButtonHighlighted.tmp, Color.white, 1);
        }

        // Setup navigation
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
        DisplayArrayTMPs(violentButtonHighlighteds, true);
    }

    void HighlightCautiousListType()
    {
        HeaderColor(cautiousHeaderTMP, Color.yellow);
        DisplayArrayTMPs(cautiousButtonHighlighteds, true);
    }

    void HighlightPreciseListType()
    {
        HeaderColor(preciseHeaderTMP, Color.yellow);
        DisplayArrayTMPs(preciseButtonHighlighteds, true);
    }

    void DisplaySlotsOfType()
    { 
        foreach (GameObject gameObject in slotsParents)
            gameObject.SetActive(false);

        slotsParents[highlightedButtonIndex].SetActive(true);
    }

    void HandleMainHighlight(MenuButtonHighlighted button)
    {
        //update index
        highlightedButtonIndex = allMenuButtonHighlighteds.IndexOf(button);

        //hide everything
        foreach (var b in allMenuButtonHighlighteds)
            b.tmp.enabled = false;

        foreach (var go in slotsParents)
            go.SetActive(false);

        //display correct slots and types
        slotsParents[highlightedButtonIndex].SetActive(true);

        int i = highlightedButtonIndex;

        if (i < 3)
            HighlightViolentListType();
        else if (i < 6)
            HighlightCautiousListType();
        else
            HighlightPreciseListType();

        //color button
        FieldEvents.SetTextColor(button.tmp, Color.yellow, 1);
    }

    void DisplayArrayTMPs(MenuButtonHighlighted[] arrayTohide, bool on)
    {
        foreach (MenuButtonHighlighted typeButtonHighlighted in arrayTohide)
        {
            typeButtonHighlighted.tmp.enabled = on;
        }
    }

    public void LoadMoveList(MoveSO[] equippedMovesOfType, MoveSlotUI[] slots)
    {
       for (int i = 0; i < slots.Length; i++)
       {
           if (i < equippedMovesOfType.Length && equippedMovesOfType[i] != null)
           {
               slots[i].moveSO = equippedMovesOfType[i];
               slots[i].moveSO.isEquipped = true;
               slots[i].slotText.text = $"Slot {i + 1}: {equippedMovesOfType[i].MoveName}";
               slots[i].gameObject.name = $"Slot {i + 1}: {equippedMovesOfType[i].MoveName}";

            // Set alpha of the TextMeshProUGUI element based on whether the move is a flaw or is eqipped
            FieldEvents.SetTextColor(slots[i].slotText, Color.white, slots[i].moveSO.IsFlaw ? 0.75f : 1f);
           }
           else
           {
                slots[i].slotText.text = $"Slot {i + 1}: Empty";
                slots[i].gameObject.name = $"Slot {i + 1}: Empty";
                FieldEvents.SetTextColor(slots[i].slotText, Color.white, .75f);
           }
       }
    }

    public void LoadAllEquippedMovesToUISlots()
    {
        LoadMoveList(playerMoveManager.playerMoveInventorySO.violentAttacksEquipped, violentAttackSlots);
        LoadMoveList(playerMoveManager.playerMoveInventorySO.violentFendsEquipped, violentFendSlots);
        LoadMoveList(playerMoveManager.playerMoveInventorySO.violentFocusesEquipped, violentFocusSlots);
        
        LoadMoveList(playerMoveManager.playerMoveInventorySO.cautiousAttacksEquipped, cautiousAttackSlots);
        LoadMoveList(playerMoveManager.playerMoveInventorySO.cautiousFendsEquipped, cautiousFendSlots);
        LoadMoveList(playerMoveManager.playerMoveInventorySO.cautiousFocusesEquipped, cautiousFocusSlots);
        
        LoadMoveList(playerMoveManager.playerMoveInventorySO.preciseAttacksEquipped, preciseAttackSlots);
        LoadMoveList(playerMoveManager.playerMoveInventorySO.preciseFendsEquipped, preciseFendSlots);
        LoadMoveList(playerMoveManager.playerMoveInventorySO.preciseFocusesEquipped, preciseFocusSlots);
    }
}