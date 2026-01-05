using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public abstract class Menu : MonoBehaviour
{
    public GameObject displayContainer;
    public MenuManagerUI menuManagerUI;
    public MenuButtonHighlighted lastParentButtonSelected;

    public abstract void EnterMenu();
    public abstract void ExitMenu();
    public abstract void StateUpdate();
    public abstract void DisplayMenu(bool on);
}
