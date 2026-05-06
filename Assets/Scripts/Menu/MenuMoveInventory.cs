using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class MenuMoveInventory : PauseMenu
{
    [SerializeField] Button firstButtonToSelect;
    public GameObject previousDisplayContainerToHide;
    public Menu menuToRevertTo;
    public TextMeshProUGUI moveInventoryHeaderTMP;
    public MenuMoves menuMoves;

    public List<MoveSO> moveInventory = new List<MoveSO>();
    public List<MoveSlotUI> instantiatedMoveSlots =  new List<MoveSlotUI>();
    public List<Button> instantiatedMoveSlotButtons = new List<Button>();

    [SerializeField] GameObject moveSlotPrefab, moveSlotsParent;
    public TextMeshProUGUI moveDescriptionTMP, movePropertiesTMP;

    MoveSlotUI moveSlotUIToEquipTo;

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
        previousDisplayContainerToHide.SetActive(false);
        LoadInventoryToButtonSlots();
        firstButtonToSelect.Select();
    }

    public void ChangeMenuToRevertTo(Menu _menuToRevertTo)
    { 
        menuToRevertTo = _menuToRevertTo;
    }

    public void MoveSlotToEquipTo(MoveSlotUI moveSlotUI)
    { 
        this.moveSlotUIToEquipTo = moveSlotUI;
    }

    public override void ExitMenu()
    {
        previousDisplayContainerToHide.SetActive(true);
        displayContainer.SetActive(false);
        pauseMenuManager.EnterMenu(menuToRevertTo);
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitMenu();
        }
    }

    public void LoadInventoryToButtonSlots()
    {
        instantiatedMoveSlots.Clear();

        foreach (MoveSO moveSO in moveInventory)
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
       
            instantiatedMoveSlotButtons.Add(moveSlotUI.button);
            instantiatedMoveSlots.Add(moveSlotUI);

            moveSlotUI.button.onClick.AddListener(() => EquipMoveFromInventoryToSlot(moveSlotUI));
        }

        FieldEvents.SetGridNavigationWrapAroundHorizontal(instantiatedMoveSlotButtons, 3);
        firstButtonToSelect = instantiatedMoveSlotButtons[0];
    }


    public void EquipMoveFromInventoryToSlot(MoveSlotUI selectedInventorySlot)
    {
        if (selectedInventorySlot.moveSO.isEquipped)
        {
            return;
        }

        if (!selectedInventorySlot.moveSO.isEquipped)
        {
            if (moveSlotUIToEquipTo.moveSO != null)
            {
                menuMoves.playerMoveManager.playerMoveInventorySO.UnequipMove(moveSlotUIToEquipTo.moveSO);
            }

            Debug.Log("fix");
            //menuMoves.playerMoveManager.playerMoveInventorySO.EquipMoveToSlot(SelectedMoveArray(), moveSlotUIToEquipTo.equipSlotNumber, selectedInventorySlot.moveSO);

            ExitMenu();
        }
    }

    public void ChangeMoveInventoryHeaderText(string text)
    {
        moveInventoryHeaderTMP.text = text;
    }

    public void SetInventoryMoveTypeViolentAttacks()
    {
        moveInventory = menuMoves.playerMoveManager.playerMoveInventorySO.violentAttacksInventory;
      //stringArrayToUpdateInSO = playerEquippedMovesSO.violentAttacksListString;
    }

    public void SetInventoryMoveTypeViolentFends()
    {
      //  moveTypeInventoryToDisplay = new List<PlayerMove>(moveInventory.violentFendsInventory);
     //   stringArrayToUpdateInSO = playerEquippedMovesSO.violentFendsListString;
    }

    public void SetInventoryMoveTypeViolentFocuses()
    {
       // moveTypeInventoryToDisplay = new List<PlayerMove>(moveInventory.violentFocusesInventory);
      //  stringArrayToUpdateInSO = playerEquippedMovesSO.violentFocusesListString;
    }

    public void SetInventoryMoveTypeCautiousAttacks()
    {
    //    moveTypeInventoryToDisplay = new List<PlayerMove>(moveInventory.cautiousAttacksInventory);
     //   stringArrayToUpdateInSO = playerEquippedMovesSO.cautiousAttackssListString;
    }

    public void SetInventoryMoveTypeCautiousFends()
    {
      //  moveTypeInventoryToDisplay = new List<PlayerMove>(moveInventory.cautiousFendsInventory);
     //   stringArrayToUpdateInSO = playerEquippedMovesSO.cautiousFendsListString;
    }

    public void SetInventoryMoveTypeCautiousFocuses()
    {
      //  moveTypeInventoryToDisplay = new List<PlayerMove>(moveInventory.cautiousFocusesInventory);
       // stringArrayToUpdateInSO = playerEquippedMovesSO.cautiousFocusesListString;
    }

    public void SetInventoryMoveTypePreciseAttacks()
    {
     //   moveTypeInventoryToDisplay = new List<PlayerMove>(moveInventory.preciseAttacksInventory);
       // stringArrayToUpdateInSO = playerEquippedMovesSO.preciseAttacksListString;
    }

    public void SetInventoryMoveTypePreciseFends()
    {
     //   moveTypeInventoryToDisplay = new List<PlayerMove>(moveInventory.preciseFendsInventory);
       // stringArrayToUpdateInSO = playerEquippedMovesSO.preciseFendsListString;
    }

    public void SetInventoryMoveTypePreciseFocuses()
    {
       // moveTypeInventoryToDisplay = new List<PlayerMove>(moveInventory.preciseFocusesInventory);
      //  stringArrayToUpdateInSO = playerEquippedMovesSO.preciseFocusesListString;
    }
}
