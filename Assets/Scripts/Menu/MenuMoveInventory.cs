using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class MenuMoveInventory : Menu
{
    [SerializeField] Button firstButtonToSelect;
    public GameObject previousDisplayContainerToHide;
    public Menu menuToRevertTo;
    public TextMeshProUGUI moveInventoryHeaderTMP;

    public List<MoveSO> moveInventory;
    public List<MoveSlot> instantiatedMoveSlots =  new List<MoveSlot>();
    public List<Button> instantiatedMoveSlotButtons = new List<Button>();

    MoveSlot moveSlotToEquipTo;

    public PlayerMoveManager playerMoveManager;

    [SerializeField] GameObject moveSlotPrefab, moveSlotsParent;
    public TextMeshProUGUI moveDescriptionTMP, movePropertiesTMP;

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
        menuManagerUI.DisplayMenuContainer(menuManagerUI.moveInventory);
        previousDisplayContainerToHide.SetActive(false);
        LoadInventoryToButtonSlots();
        firstButtonToSelect.Select();
    }

    public void ChangeMenuToRevertTo(Menu _menuToRevertTo)
    { 
        menuToRevertTo = _menuToRevertTo;
    }

    public void MoveSlotToEquipTo(MoveSlot moveSlotToEquipTo)
    { 
        this.moveSlotToEquipTo = moveSlotToEquipTo;
    }

    public override void ExitMenu()
    {
        previousDisplayContainerToHide.SetActive(true);
        displayContainer.SetActive(false);
        menuManagerUI.EnterMenu(menuToRevertTo);
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
            GameObject moveSlotGO = Instantiate(moveSlotPrefab);
            moveSlotGO.transform.SetParent(moveSlotsParent.transform);

            MoveInventorySlot moveInventorySlot = moveSlotGO.GetComponent<MoveInventorySlot>();
            moveInventorySlot.moveSO = moveSO;
            moveInventorySlot.menuMoveInventory = this;
            moveInventorySlot.slotText.text = moveInventorySlot.moveSO.MoveName;
            moveSlotGO.name = moveInventorySlot.moveSO.name;

            if (moveInventorySlot.moveSO.isEquipped) 
            {
                Color currentColor = moveInventorySlot.slotText.color;
                currentColor.a = 1f;
                moveInventorySlot.slotText.color = currentColor;

                currentColor = moveInventorySlot.slotText.color; 
                currentColor.a = 0.7f;
                moveInventorySlot.slotText.color = currentColor;
            }

            instantiatedMoveSlotButtons.Add(moveInventorySlot.button);

            moveInventorySlot.button.onClick.AddListener(() => EquipMoveFromInventoryToSlot(moveInventorySlot));
            instantiatedMoveSlots.Add(moveInventorySlot);
        }

        FieldEvents.SetGridNavigationWrapAroundHorizontal(instantiatedMoveSlotButtons, 3);
        firstButtonToSelect = instantiatedMoveSlotButtons[0];


        //for (int i = 0; i < menuMoveInventorySlots.Length; i++)
        //{
        //    if (i < moveTypeInventoryToDisplay.Count)
        //    {
        //        menuMoveInventorySlots[i].gameObject.SetActive(true);
        //        menuMoveInventorySlots[i].moveSO = moveTypeInventoryToDisplay[i];
        //        menuMoveInventorySlots[i].slotText.text = moveTypeInventoryToDisplay[i].MoveName;
        //
        //        Color currentColor = menuMoveInventorySlots[i].slotText.color; 
        //        currentColor.a = 1f;
        //        menuMoveInventorySlots[i].slotText.color = currentColor;
        //
        //        if (menuMoveInventorySlots[i].moveSO.isEquipped)
        //        {
        //            currentColor = menuMoveInventorySlots[i].slotText.color; 
        //            currentColor.a = 0.7f; 
        //            menuMoveInventorySlots[i].slotText.color = currentColor;
        //        }
        //
        //    }
        //    else
        //    {
        //        menuMoveInventorySlots[i].gameObject.SetActive(false);
        //    }
        //}
    }

    public void EquipMoveFromInventoryToSlot(MoveSlot moveFromInventorySlot)
    {
        if (!moveFromInventorySlot.moveSO.isEquipped)
        {
            if (moveSlotToEquipTo.moveSO != null)
            {
                moveSlotToEquipTo.moveSO.isEquipped = false;
            } 

            moveSlotToEquipTo.moveSO = moveFromInventorySlot.moveSO;
            moveFromInventorySlot.moveSO.isEquipped = true;
            moveSlotToEquipTo.slotText.text = "Slot " + (int.Parse(moveSlotToEquipTo.name) + 1) + ": " + moveFromInventorySlot.moveSO.MoveName;
            //stringArrayToUpdateInSO[int.Parse(moveSlotToEquipTo.name)] = moveInventorySlot.moveSO.MoveName;
            //playerMoveManager.LoadEquippedMoveListFromSO();

            ExitMenu();
        }

        if (moveFromInventorySlot.moveSO.isEquipped)
        {
            return;
        }
    }

    public void ChangeMoveInventoryHeaderText(string text)
    {
        moveInventoryHeaderTMP.text = text;
    }

    public void SetInventoryMoveTypeViolentAttacks()
    {
        moveInventory = playerMoveManager.playerMoveInventorySO.violentAttacksInventory;
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
