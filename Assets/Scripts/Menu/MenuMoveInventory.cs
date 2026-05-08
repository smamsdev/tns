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
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitMenu();
        }
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
            FieldEvents.SetTextColor(moveSlotUI.slotText, Color.white, 1f);

            if (moveSO.isEquipped) 
            {
                FieldEvents.SetTextColor(moveSlotUI.slotText, Color.white, .7f);
            }

            buttons.Add(moveSlotUI.button);
            instantiatedMoveSlots.Add(moveSlotUI);

            moveSlotUI.onHighlighted = () => SlotHighlighted(moveSlotUI);

            moveSlotUI.button.onClick.AddListener(() => EquipMoveFromInventoryToSlot(moveSlotUI));
        }

        FieldEvents.SetGridNavigationWrapAroundHorizontal(buttons, 3);
    }

    void SlotHighlighted(MoveSlotUI moveSlotUI)
    {
        headerTMP.text = "Assign " + moveSlotUI.moveSO.MoveName + "?";
    }

    public void UpdateMoveDescriptions(MoveSlotUI moveSlotUI)
    {
        menuMoves.moveNameTMP.text = moveSlotUI.moveSO.MoveName;
        menuMoves.probabilityTMP.text = moveSlotUI.moveSO.GetRarityDescription();
        menuMoves.moveDescriptionTMP.text = moveSlotUI.moveSO.MoveDescription;
        menuMoves.movePotentialChangeTMP.text = moveSlotUI.moveSO.PotentialChangeDescription;

        if (moveSlotUI.moveSO.isEquipped)
        {
            menuMoves.moveEquipStatusTMP.text = "Assigned to. Press CTRL to unassign";
            return;
        }

        menuMoves.moveEquipStatusTMP.text = "";
    }

    public void EquipMoveFromInventoryToSlot(MoveSlotUI selectedInventorySlot)
    {
        if (selectedInventorySlot.moveSO.isEquipped)
        {
            return;
        }

        if (!selectedInventorySlot.moveSO.isEquipped)
        {
            //if (moveSlotUIToEquipTo.moveSO != null)
           // {
           //     menuMoves.playerMoveManager.playerMoveInventorySO.UnequipMove(moveSlotUIToEquipTo.moveSO);
           // }

            Debug.Log("fix");
            //menuMoves.playerMoveManager.playerMoveInventorySO.EquipMoveToSlot(SelectedMoveArray(), moveSlotUIToEquipTo.equipSlotNumber, selectedInventorySlot.moveSO);

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
