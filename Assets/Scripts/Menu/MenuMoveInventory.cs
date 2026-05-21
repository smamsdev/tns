using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static PlayerMoveInventorySO;

public class MenuMoveInventory : PauseMenu
{
    public TextMeshProUGUI moveInventoryHeaderTMP;
    public MenuMoves menuMoves;
    public MenuMoveEquipSlotSelect menuMoveEquipSlotSelectInPlay;
    [SerializeField] GameObject moveSlotPrefab, moveSlotsParent;
    public TextMeshProUGUI headerTMP, moveNameTMP, probabilityTMP, moveDescriptionTMP, movePotentialChangeTMP, moveEquipStatusTMP;
    public int highlightedButtonIndex = 0;
    public List<MoveSO> moveList;
    public List<MoveSlotUI> instantiatedMoveSlots = new List<MoveSlotUI>();

    private void Start()
    {
        displayContainer.SetActive(false);
    }

    public override void DisplayMenu(bool on)
    {
        displayContainer.SetActive(on);
    }

    public override void EnterMenu()
    {
        pauseMenuManager.ClearThenDisplayMenu(pauseMenuManager.moveInventory);
        ClearAllDescriptionTMPs();
        InitMoveInventoryUI();
        instantiatedMoveSlots[highlightedButtonIndex].button.Select();
    }

    public override void ExitMenu()
    {
        displayContainer.SetActive(false);
        pauseMenuManager.EnterMenu(menuMoveEquipSlotSelectInPlay);
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitMenu();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            UnassignSlot(instantiatedMoveSlots[highlightedButtonIndex].moveSO);
        }
    }

    public void UnassignSlot(MoveSO moveSO)
    {
        if (moveSO.IsFlaw && !menuMoves.playerMoveInventorySO.IsFlawReassignmentEnabled)
            return;

        MoveType moveType = menuMoveEquipSlotSelectInPlay.moveType;

        menuMoves.playerMoveInventorySO.UnequipMoveFromSlot(moveType, moveSO);
        menuMoveEquipSlotSelectInPlay.InitMoveEquipSlotList();
        InitMoveInventoryUI();
        menuMoves.InitAllEquippedMovesToUISlots();
        instantiatedMoveSlots[highlightedButtonIndex].button.Select();
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

   public void DeleteAllInventoryUI()
   {
        instantiatedMoveSlots.Clear();
   
       for (int i = moveSlotsParent.transform.childCount - 1; i >= 0; i--)
       {
           Destroy(moveSlotsParent.transform.GetChild(i).gameObject);
       }
   }

    public void InitMoveInventoryUI()
    {
        List<Button> buttons = new();

        DeleteAllInventoryUI();
  
        //moveList is set by EquipSlotSelect Class before EnterMenu func
        foreach (MoveSO moveSO in moveList)
        {
            GameObject moveSlotUIGO = Instantiate(moveSlotPrefab);
            moveSlotUIGO.name = moveSO.name;
            moveSlotUIGO.transform.SetParent(moveSlotsParent.transform);
       
            MoveSlotUI moveSlotUI = moveSlotUIGO.GetComponent<MoveSlotUI>();
            moveSlotUI.moveSO = moveSO;
            moveSlotUI.slotText.text = moveSO.MoveName;
            moveSlotUI.icon.sprite = moveSO.IsFlaw? moveSlotUI.flawIcon : moveSlotUI.moveIcon;
            FieldEvents.SetTextColor(moveSlotUI.slotText, Color.white, 1f);

            if (moveSO.isEquipped) 
                FieldEvents.SetTextColor(moveSlotUI.slotText, Color.white, .7f);

            if (moveSO.IsFlaw && !menuMoves.playerMoveInventorySO.IsFlawReassignmentEnabled)
                FieldEvents.SetTextColor(moveSlotUI.slotText, Color.white, .7f);

            buttons.Add(moveSlotUI.button);
            instantiatedMoveSlots.Add(moveSlotUI);
            moveSlotUI.onHighlighted = () => SlotHighlighted(moveSlotUI);
            moveSlotUI.button.onClick.AddListener(() => MoveSelected(moveSlotUI));
        }

        FieldEvents.SetGridNavigationWrapAroundHorizontal(buttons, 3);
    }

    void SlotHighlighted(MoveSlotUI moveSlotUI)
    {
        highlightedButtonIndex = instantiatedMoveSlots.IndexOf(moveSlotUI);
        UpdateMoveDescriptions(moveSlotUI);
    }

    public void UpdateMoveDescriptions(MoveSlotUI moveSlotUI)
    {
        int equipIndex = menuMoveEquipSlotSelectInPlay.highlightedButtonIndex;

        moveNameTMP.text = moveSlotUI.moveSO.MoveName;
        probabilityTMP.text = moveSlotUI.moveSO.GetRarityDescription();
        moveDescriptionTMP.text = moveSlotUI.moveSO.MoveDescription;
        movePotentialChangeTMP.text = moveSlotUI.moveSO.PotentialChangeDescription;

        if (moveSlotUI.moveSO.isEquipped)
        {
            moveEquipStatusTMP.text = "Press CTRL to unassign";
            var equipList = menuMoves.playerMoveInventorySO.GetEquippedArrayOfType(menuMoveEquipSlotSelectInPlay.moveType);
            int equippedSlotIndex = System.Array.IndexOf(equipList, moveSlotUI.moveSO) + 1;
            headerTMP.text = "Already assigned to " + equippedSlotIndex;
        }

        else
        {
            headerTMP.text = $"Assign {moveSlotUI.moveSO.MoveName} to slot {equipIndex + 1}?";
            moveEquipStatusTMP.text = "";
        }

        if (moveSlotUI.moveSO.IsFlaw && !menuMoves.playerMoveInventorySO.IsFlawReassignmentEnabled)
        {
            headerTMP.text = "Unable to assign a FLAW";
            moveEquipStatusTMP.text = "";
        }
    }

    public void MoveSelected(MoveSlotUI selectedInventorySlot)
    {
        if (selectedInventorySlot.moveSO.isEquipped)
            return;

        if (selectedInventorySlot.moveSO.IsFlaw && !menuMoves.playerMoveInventorySO.IsFlawReassignmentEnabled)
            return;

        if (!selectedInventorySlot.moveSO.isEquipped)
        {
            int slotIndex = menuMoveEquipSlotSelectInPlay.highlightedButtonIndex;
            MoveSlotUI slotToEquipTo = menuMoveEquipSlotSelectInPlay.moveEquipArrayUI[slotIndex];
            MoveType moveType = menuMoveEquipSlotSelectInPlay.moveType;

            //manage if equip slot is occupied
            if (slotToEquipTo.moveSO != null)
                menuMoves.playerMoveInventorySO.UnequipMoveFromSlot(moveType, slotToEquipTo.moveSO);


            menuMoves.playerMoveInventorySO.EquipMoveToSlot(moveType, slotIndex, selectedInventorySlot.moveSO);
            menuMoveEquipSlotSelectInPlay.InitMoveEquipSlotList();
            ExitMenu();
        }
    }

    public string GetInventoryListStringOfType(MoveType moveType)
    {
        switch (moveType)
        {
            case MoveType.ViolentAttack:
                return "Violent Attack";

            case MoveType.ViolentFend:
                return "Violent Fend";

            case MoveType.ViolentFocus:
                return "Violent Focus";

            case MoveType.CautiousAttack:
                return "Cautious Attack";

            case MoveType.CautiousFend:
                return "Cautious Fend";

            case MoveType.CautiousFocus:
                return "Cautious Focus";

            case MoveType.PreciseAttack:
                return "Precise Attack";

            case MoveType.PreciseFend:
                return "Precise Fend";

            case MoveType.PreciseFocus:
                return "Precise Focus";

            default:
                Debug.Log("something went wrong");
                return null;
        }
    }
}
