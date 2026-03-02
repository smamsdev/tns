using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class TrenchStructureInventoryState : TrenchMenuState
{
    public GameObject inventoryDisplayGO, inventoryUIPrefab, descriptionGO;
    public List<TrenchStructureSlotUI> slots;
    public TextMeshProUGUI nameTMP, descTMP, typeTMP;
    public int buttonSelectedIndex;

    private void Start()
    {
        descriptionGO.SetActive(false);
    }

    public override void EnterState()
    {
        inventoryDisplayGO.SetActive(true);
        descriptionGO.SetActive(true);
        initializeMenu();
        buttonSelectedIndex = 0;
        slots[buttonSelectedIndex].menuButtonHighlighted.button.Select();
    }

    void initializeMenu()
    {
        List<Button> buttons = new List<Button>();

        foreach (TrenchStructure trenchStructure in menuManager.playerInventorySO.TrenchStructureInventory)
        {
            GameObject structureSlotUIGO = Instantiate(inventoryUIPrefab, inventoryDisplayGO.transform);
            TrenchStructureSlotUI trenchStructureSlotUI = structureSlotUIGO.GetComponent<TrenchStructureSlotUI>();
            trenchStructureSlotUI.trenchStructure = trenchStructure;
            trenchStructureSlotUI.textMeshProUGUI.text = trenchStructure.structurePrice.ToString();
            trenchStructureSlotUI.image.sprite = trenchStructureSlotUI.trenchStructure.structureSprite;

            trenchStructureSlotUI.menuButtonHighlighted.onHighlighted = () => inventoryHighlighted(trenchStructureSlotUI);
            trenchStructureSlotUI.menuButtonHighlighted.onUnHighlighted = () => inventoryUnHighlighted(trenchStructureSlotUI);
            trenchStructureSlotUI.menuButtonHighlighted.button.onClick.AddListener(() => InventorySelected(trenchStructureSlotUI));

            slots.Add(trenchStructureSlotUI);
            buttons.Add(trenchStructureSlotUI.menuButtonHighlighted.button);
        }

        FieldEvents.SetGridNavigationWrapAround(buttons, 2);
    }

    void inventoryHighlighted(TrenchStructureSlotUI trenchStructureSlotUI)
    {
        buttonSelectedIndex = slots.IndexOf(trenchStructureSlotUI);
        trenchStructureSlotUI.image.color = Color.yellow;
        UpdateDescriptionArea(trenchStructureSlotUI);
    }

    void inventoryUnHighlighted(TrenchStructureSlotUI trenchStructureSlotUI)
    {
        trenchStructureSlotUI.image.color = Color.white;
    }

    void UpdateDescriptionArea(TrenchStructureSlotUI trenchStructureSlotUI)
    {
        TrenchStructure structure = trenchStructureSlotUI.trenchStructure;

        nameTMP.text = structure.structureName;
        descTMP.text = structure.structureDescription;
        typeTMP.text = structure.type;
    }

    void InventorySelected(TrenchStructureSlotUI trenchStructureSlotUI)
    {
        menuManager.constructState.structureToConstruct = trenchStructureSlotUI.trenchStructure;
        ExitState();
        menuManager.ChangeState(menuManager.constructState);
    }

    void DeleteAllUI()
    {
        slots.Clear();

        for (int i = inventoryDisplayGO.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(inventoryDisplayGO.transform.GetChild(i).gameObject);
        }
    }

    public override void ExitState()
    {
        descriptionGO.SetActive(false);
        inventoryDisplayGO.SetActive(false);
        DeleteAllUI();
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        { 
            ExitState();
            buttonSelectedIndex = 0;
            menuManager.mainState.WireButtons();
            menuManager.ChangeState(menuManager.mainState);
        }
    }
}
