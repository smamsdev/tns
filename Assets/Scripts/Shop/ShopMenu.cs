using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public abstract class ShopMenu : MonoBehaviour
{
    public GameObject displayContainer;
    public Button mainButtonToRevert;
    //public IMenuButtonHighlighted menuButtonHighlighted;
    public ShopManagerUI menuManagerUI;

    public abstract void EnterMenu();
    public abstract void ExitMenu();
    public abstract void StateUpdate();
    public abstract void DisplayMenu(bool on);
}