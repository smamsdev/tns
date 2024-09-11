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

    public List<PlayerMove> moveTypeInventoryToDisplay;
    MoveInventory moveInventory;
    public MoveSlot[] menuMoveInventorySlots;
    public MoveSlot moveSlotToEquipTo;

    public PlayerMoveManager playerMoveManager;
    public PlayerEquippedMovesSO playerEquippedMovesSO;
    public string[] stringArrayToUpdateInSO;

    private void OnEnable()
    {
        var player = GameObject.Find("Player");
        moveInventory = player.GetComponentInChildren<MoveInventory>();
        playerMoveManager = player.GetComponentInChildren<PlayerMoveManager>();
        playerEquippedMovesSO = playerMoveManager.playerEquippedMovesSO;
    }

    private void Start()
    {
        displayContainer.SetActive(false);
    }

    public override void DisplayMenu(bool on)
    {
        throw new System.NotImplementedException();
    }

    public override void EnterMenu()
    {
        previousDisplayContainerToHide.SetActive(false);
        LoadInventoryToButtonSlots();
        displayContainer.SetActive(true);
        firstButtonToSelect.Select();
    }

    public void ChangeMenuToRevertTo(Menu _menuToRevertTo)

    { 
        menuToRevertTo = _menuToRevertTo;
    }

    public override void ExitMenu()
    {
        previousDisplayContainerToHide.SetActive(true);
        displayContainer.SetActive(false);
        menuManagerUI.EnterSubMenu(menuToRevertTo);
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
        for (int i = 0; i < menuMoveInventorySlots.Length; i++)
        {
            if (i < moveTypeInventoryToDisplay.Count)
            {
                menuMoveInventorySlots[i].gameObject.SetActive(true);
                menuMoveInventorySlots[i].move = moveTypeInventoryToDisplay[i];
                menuMoveInventorySlots[i].textMeshProUGUI.text = moveTypeInventoryToDisplay[i].moveName;

                Color currentColor = menuMoveInventorySlots[i].textMeshProUGUI.color; 
                currentColor.a = 1f;
                menuMoveInventorySlots[i].textMeshProUGUI.color = currentColor;

                if (menuMoveInventorySlots[i].move.isEquipped)
                {
                    currentColor = menuMoveInventorySlots[i].textMeshProUGUI.color; 
                    currentColor.a = 0.7f; 
                    menuMoveInventorySlots[i].textMeshProUGUI.color = currentColor;
                }

            }
            else
            {
                menuMoveInventorySlots[i].gameObject.SetActive(false);
            }
        }
    }

    public void MoveSlotToEquipTo(MoveSlot moveSlot)
    {
        moveSlotToEquipTo = moveSlot;
    }

    public void EquipMoveFromInventoryToSlot(MoveSlot moveInventorySlot)
    {
        if (!moveInventorySlot.move.isEquipped)
        {
            if (moveSlotToEquipTo.move != null)
            {
                moveSlotToEquipTo.move.isEquipped = false;
            } 

            moveSlotToEquipTo.move = moveInventorySlot.move;
            moveInventorySlot.move.isEquipped = true;
            moveSlotToEquipTo.textMeshProUGUI.text = "Slot " + (int.Parse(moveSlotToEquipTo.name) + 1) + ": " + moveInventorySlot.move.moveName;
            stringArrayToUpdateInSO[int.Parse(moveSlotToEquipTo.name)] = moveInventorySlot.move.moveName;
            playerMoveManager.LoadEquippedMoveListFromSO();

            ExitMenu();
        }

        if (moveInventorySlot.move.isEquipped)

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
        moveTypeInventoryToDisplay = new List<PlayerMove>(moveInventory.violentAttacksInventory);
        stringArrayToUpdateInSO = playerEquippedMovesSO.violentAttacksListString;
    }

    public void SetInventoryMoveTypeViolentFends()
    {
        moveTypeInventoryToDisplay = new List<PlayerMove>(moveInventory.violentFendsInventory);
        stringArrayToUpdateInSO = playerEquippedMovesSO.violentFendsListString;
    }

    public void SetInventoryMoveTypeViolentFocuses()
    {
        moveTypeInventoryToDisplay = new List<PlayerMove>(moveInventory.violentFocusesInventory);
        stringArrayToUpdateInSO = playerEquippedMovesSO.violentFocusesListString;
    }

    public void SetInventoryMoveTypeCautiousAttacks()
    {
        moveTypeInventoryToDisplay = new List<PlayerMove>(moveInventory.cautiousAttacksInventory);
        stringArrayToUpdateInSO = playerEquippedMovesSO.cautiousAttackssListString;
    }

    public void SetInventoryMoveTypeCautiousFends()
    {
        moveTypeInventoryToDisplay = new List<PlayerMove>(moveInventory.cautiousFendsInventory);
        stringArrayToUpdateInSO = playerEquippedMovesSO.cautiousFendsListString;
    }

    public void SetInventoryMoveTypeCautiousFocuses()
    {
        moveTypeInventoryToDisplay = new List<PlayerMove>(moveInventory.cautiousFocusesInventory);
        stringArrayToUpdateInSO = playerEquippedMovesSO.cautiousFocusesListString;
    }

    public void SetInventoryMoveTypePreciseAttacks()
    {
        moveTypeInventoryToDisplay = new List<PlayerMove>(moveInventory.preciseAttacksInventory);
        stringArrayToUpdateInSO = playerEquippedMovesSO.preciseAttacksListString;
    }

    public void SetInventoryMoveTypePreciseFends()
    {
        moveTypeInventoryToDisplay = new List<PlayerMove>(moveInventory.preciseFendsInventory);
        stringArrayToUpdateInSO = playerEquippedMovesSO.preciseFendsListString;
    }

    public void SetInventoryMoveTypePreciseFocuses()
    {
        moveTypeInventoryToDisplay = new List<PlayerMove>(moveInventory.preciseFocusesInventory);
        stringArrayToUpdateInSO = playerEquippedMovesSO.preciseFocusesListString;
    }
}
