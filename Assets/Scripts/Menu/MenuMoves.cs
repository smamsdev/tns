using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class MenuMoves : Menu
{
    [SerializeField] Button firstButtonToSelect;
    public GameObject moveDescriptionGO;
    public bool isSelectingMove;
    IMenuMoveTypeHighlighted buttonTypeToReturnTo;
    public PlayerMoveManager playerMoveManager;
    public MenuMoveInventory menuMoveInventory;

    public TextMeshProUGUI moveDescriptions;
    public TextMeshProUGUI movePropertyTMP;


    public MoveSlot[] violentAttackSlots = new MoveSlot[5];
    public MoveSlot[] violentFendSlots = new MoveSlot[5];
    public MoveSlot[] violentFocusSlots = new MoveSlot[5];
    public MoveSlot[] cautiousAttackSlots = new MoveSlot[5];
    public MoveSlot[] cautiousFendSlots = new MoveSlot[5];
    public MoveSlot[] cautiousFocusSlots = new MoveSlot[5];
    public MoveSlot[] preciseAttackSlots = new MoveSlot[5];
    public MoveSlot[] preciseFendSlots = new MoveSlot[5];
    public MoveSlot[] preciseFocusSlots = new MoveSlot[5];

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
        menuManagerUI.DisplayMenuContainer(this);
        menuButtonHighlighted.SetButtonColor(menuButtonHighlighted.highlightedColor);
        menuButtonHighlighted.enabled = false; //this removes the blue underline
        firstButtonToSelect.Select();
        moveDescriptionGO.SetActive(true);

        LoadAllEquippedMovesToUISlots();
    }

    public override void ExitMenu()
    {
        menuButtonHighlighted.SetButtonColor(Color.white);
        menuButtonHighlighted.enabled = true; //this keeps the blue underline
        menuManagerUI.EnterMenu(menuManagerUI.main);
        mainButtonToRevert.Select();
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitMenu();
        }
    }

    public void LoadMoveList(MoveSO[] equippedMovesOfType, MoveSlot[] slots)
    {
       for (int i = 0; i < slots.Length; i++)
       {
           if (i < equippedMovesOfType.Length && equippedMovesOfType[i] != null)
           {
               slots[i].moveSO = equippedMovesOfType[i];
               slots[i].moveSO.isEquipped = true;
               slots[i].slotText.text = $"Slot {i + 1}: {equippedMovesOfType[i].MoveName}";
       
               // Set alpha of the TextMeshProUGUI element based on whether the move is a flaw or is eqipped
               menuManagerUI.SetTextAlpha(slots[i].slotText, slots[i].moveSO.IsFlaw ? 0.75f : 1f);
           }
           else
           {
               slots[i].slotText.text = $"Slot {i + 1}: Empty";
               menuManagerUI.SetTextAlpha(slots[i].slotText, .75f);
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