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
        if (menuManager.playerInventorySO.trenchStructuresInventory.Count <= 0)
            Debug.LogError("inventoryblank");

        List<Button> buttons = new List<Button>();

        foreach (TrenchStructureSO trenchStructureSO in menuManager.playerInventorySO.trenchStructuresInventory)
        {
            GameObject structureSlotUIGO = Instantiate(inventoryUIPrefab, inventoryDisplayGO.transform);
            TrenchStructureSlotUI trenchStructureSlotUI = structureSlotUIGO.GetComponent<TrenchStructureSlotUI>();

            trenchStructureSlotUI.structureSO = trenchStructureSO;
            trenchStructureSlotUI.textMeshProUGUI.text = trenchStructureSlotUI.structureSO.StructurePrice.ToString();
            trenchStructureSlotUI.image.sprite = trenchStructureSlotUI.structureSO.StructureSprite;

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
        UpdateDescriptionArea(trenchStructureSlotUI.structureSO);
    }

    void inventoryUnHighlighted(TrenchStructureSlotUI trenchStructureSlotUI)
    {
        trenchStructureSlotUI.image.color = Color.white;
    }

    void UpdateDescriptionArea(TrenchStructureSO structureSO)
    {
      nameTMP.text = structureSO.StructureName;
      descTMP.text = structureSO.StructureDescription;
      typeTMP.text = structureSO.Type;
    }

    void InventorySelected(TrenchStructureSlotUI trenchStructureSlotUI)
    {
        menuManager.constructState.structureSOToConstruct = trenchStructureSlotUI.structureSO;
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
