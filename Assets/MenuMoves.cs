using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class MenuMoves : Menu
{
    [SerializeField] Button firstButtonToSelect;
    public GameObject moveDescriptionGO;
    public bool isSelectingMove;
    IMenuMoveTypeHighlighted buttonTypeToReturnTo;
    public PlayerMoveManager playerMoveManager;

    public MoveSlot[] violentAttackSlots = new MoveSlot[5];
    public MoveSlot[] violentFendSlots =  new MoveSlot[5];
    public MoveSlot[] violentFocusSlots = new MoveSlot[5];
    public MoveSlot[] cautiousAttackSlots = new MoveSlot[5];
    public MoveSlot[] cautiousFendSlots = new MoveSlot[5];
    public MoveSlot[] cautiousFocuseSlots = new MoveSlot[5];
    public MoveSlot[] preciseAttackSlots = new MoveSlot[5];
    public MoveSlot[] preciseFendSlots = new MoveSlot[5];
    public MoveSlot[] preciseFocuseSlots = new MoveSlot[5];

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
        menuButtonHighlighted.SetButtonColor(menuButtonHighlighted.highlightedColor);
        menuButtonHighlighted.enabled = false; //this removes the blue underline
        firstButtonToSelect.Select();
        moveDescriptionGO.SetActive(true);

        LoadAllMoveLists();
    }

    public override void ExitMenu()
    {
        menuButtonHighlighted.SetButtonColor(Color.white);
        menuButtonHighlighted.enabled = true; //this keeps the blue underline
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

    private void LoadMoveList<T>(T[] moveArray, MoveSlot[] slots) where T : PlayerMove
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < moveArray.Length && moveArray[i] != null)
            {
                slots[i].move = moveArray[i]; 
                slots[i].textMeshProUGUI.text = "Slot " + (i + 1) + ": " + moveArray[i].moveName;
            }
            else
            {
                slots[i].textMeshProUGUI.text = "Slot " + (i + 1) + ": Empty";
            }
        }
    }

    void LoadAllMoveLists()
    {
        LoadMoveList(playerMoveManager.violentAttackSlots, violentAttackSlots);
        LoadMoveList(playerMoveManager.violentFendSlots, violentFendSlots);
        LoadMoveList(playerMoveManager.violentFocusSlots, violentFocusSlots);

        LoadMoveList(playerMoveManager.cautiousAttackSlots, violentAttackSlots);
        LoadMoveList(playerMoveManager.cautiousFendSlots, violentFendSlots);
        LoadMoveList(playerMoveManager.cautiousFocusSlots, violentFocusSlots);

        LoadMoveList(playerMoveManager.preciseAttackSlots, violentAttackSlots);
        LoadMoveList(playerMoveManager.preciseFendSlots, violentFendSlots);
        LoadMoveList(playerMoveManager.preciseFocusSlots, violentFocusSlots);
    }
}
