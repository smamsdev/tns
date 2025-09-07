using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CombatMenuManager : MonoBehaviour
{
    [SerializeField] CombatManager combatManager;
    [SerializeField] TextMeshProUGUI narratorTMP;

    [Header("Menu GameObjects")]
    public GameObject firstMoveMenu;
    public GameObject secondMoveMenu;
    public GameObject enemySelectMenu;
    public GameObject TacticalSelectMenu;
    public GameObject EquipSlotSelectMenu;
    public GameObject GearSelectMenu;
    public GameObject VictoryMenu;

    [Header("First Buttons to Highlight")]

    public Button firstMenuFirstButton; 
    public Button secondMenuFirstButton;
    public Button thirdMenuFirstButton;
    public Button tacticalMenuFirstButton;
    public Button gearSelectMenuFirstButton;

    public Color buttonSelectedYellow;

    private void OnEnable()
    {
        CombatEvents.UpdateNarrator += UpdateNarrator;
    }

    private void OnDisable()
    {
        CombatEvents.UpdateNarrator -= UpdateNarrator;
    }

    private void Start()
    {
        DisableMenuState();
    }

    public void DisplayMenuGO(GameObject menuToEnable, bool on)
    {
        menuToEnable.SetActive(on);
    }

    public void DisableMenuState()
    {
        firstMoveMenu.SetActive(false);
        secondMoveMenu.SetActive(false);
        enemySelectMenu.SetActive(false);
        GearSelectMenu.SetActive(false);
        VictoryMenu.SetActive(false);
        TacticalSelectMenu.SetActive(false);
        narratorTMP.gameObject.SetActive(false);
    }

    public void UpdateNarrator(string narratorText)
    {
        if (!narratorTMP.gameObject.activeSelf)
        {
            narratorTMP.gameObject.SetActive(true);
        }

        narratorTMP.text = narratorText;
    }

    public void SetButtonNormalColor(Button button, Color color)
    {
        ColorBlock colors = button.colors;
        colors.normalColor = color;
        button.colors = colors;
    }

    public void ColorButtonYellow(Button button) //via Button
    {
        SetButtonNormalColor(button, buttonSelectedYellow);
    }

    public void SetTextAlpha(TextMeshProUGUI textMeshProUGUI, float alpha, Color color = default)
    {
        if (color == default)
            color = Color.white;

        textMeshProUGUI.color = color;
        Color newColor = textMeshProUGUI.color;
        newColor.a = alpha;
        textMeshProUGUI.color = newColor;
    }
}