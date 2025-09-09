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

    [Header("Default Buttons for Highlight")]
    public Button actionMenuDefaultButton;
    public Button styleMenuDefaultButton;
    public Button selectEnemyMenuDefaultButton;
    public Button tacticalSelectMenuDefaultButton;
    public Button equipSlotSelectMenuDefaultButton;
    public Button gearSelectMenuDefaultButton;

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
        EquipSlotSelectMenu.SetActive(false);
        GearSelectMenu.SetActive(false);

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
        combatManager.currentState.lastButtonSelected = button;
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