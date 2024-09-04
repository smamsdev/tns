using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class MenuInventory : Menu
{
    [SerializeField] Button firstButtonToSelect;

    public void EnterInventoryMenu()

    {
        firstButtonToSelect.Select();
    }


    public override void EnterMenu()
    {
       //
    }


}
