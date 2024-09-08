using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class MenuMoveInventory : Menu
{
    [SerializeField] Button firstButtonToSelect;
    public List<string> moveInventoryToLoad;
    public GameObject previousDisplayContainerToHide;
    public Menu menuToRevertTo;
    public TextMeshProUGUI moveInventoryHeaderTMP;

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

    public void ChangeMoveInventoryHeaderText(string text)
    {
        moveInventoryHeaderTMP.text = text;
    }
}
