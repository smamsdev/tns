using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuMoves : Menu
{
    [SerializeField] Button firstButtonToSelect;
    public GameObject moveDescriptionGO;
    public bool isSelectingMove;
    IMenuMoveTypeHighlighted buttonTypeToReturnTo;
    public PlayerMoveManager playerMoveManager;

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

    public void LoadMoveList<T>(T[] moveArray, MoveSlot[] slots) where T : PlayerMove
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < moveArray.Length && moveArray[i] != null)
            {
                slots[i].move = moveArray[i];
                slots[i].move.isEquipped = true;
                slots[i].textMeshProUGUI.text = $"Slot {i + 1}: {moveArray[i].moveName}";

                // Set alpha of the TextMeshProUGUI element based on whether the move is a flaw
                menuManagerUI.SetTextAlpha(slots[i].textMeshProUGUI, slots[i].move.isFlaw ? 0.5f : 1f);
            }
            else
            {
                slots[i].textMeshProUGUI.text = $"Slot {i + 1}: Empty";
                menuManagerUI.SetTextAlpha(slots[i].textMeshProUGUI, 1f); // Default alpha for empty slots
            }
        }
    }

    public void LoadAllMoveLists()
    {
        playerMoveManager.LoadEquippedMoveListFromSO();

        LoadMoveList(playerMoveManager.violentAttackSlots, violentAttackSlots);
        LoadMoveList(playerMoveManager.violentFendSlots, violentFendSlots);
        LoadMoveList(playerMoveManager.violentFocusSlots, violentFocusSlots);

        LoadMoveList(playerMoveManager.cautiousAttackSlots, cautiousAttackSlots);
        LoadMoveList(playerMoveManager.cautiousFendSlots, cautiousFendSlots);
        LoadMoveList(playerMoveManager.cautiousFocusSlots, cautiousFocusSlots);

        LoadMoveList(playerMoveManager.preciseAttackSlots, preciseAttackSlots);
        LoadMoveList(playerMoveManager.preciseFendSlots, preciseFendSlots);
        LoadMoveList(playerMoveManager.preciseFocusSlots, preciseFocusSlots);
    }
}