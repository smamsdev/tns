using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrenchBase : MonoBehaviour
{
    public TrenchManager trenchManager;
    public List<TrenchStructure> structuresInInventory;
    public List<TrenchStructureSlot> structureSlots;
    public List<Button> structureSlotButtons;
    public GameObject structuresGO;

    public void InitializeBase()
    {
        int maxSlots = 16;
        structureSlots = new List<TrenchStructureSlot>(maxSlots);

        for (int i = 0; i < maxSlots; i++)
        {
            GameObject structureSlotGO = Instantiate(trenchManager.structureSlotPrefab, structuresGO.transform);
            structureSlotGO.name = "EmptyStructureSlot " + i;

            TrenchStructureSlot trenchStructureSlot = structureSlotGO.GetComponent<TrenchStructureSlot>();
            trenchStructureSlot.animator.enabled = false;
            trenchStructureSlot.go = structureSlotGO;

            trenchStructureSlot.menuButtonHighlighted.onHighlighted = () =>
            {
                SlotHighlighted(trenchStructureSlot);
            };


            trenchStructureSlot.menuButtonHighlighted.onUnHighlighted = () =>
            {
                SlotUnHighlighted(trenchStructureSlot);
            };

            structureSlots.Add(trenchStructureSlot);
            structureSlotButtons.Add(trenchStructureSlot.menuButtonHighlighted.button);
        }
    }

    void SlotHighlighted(TrenchStructureSlot trenchStructureSlotHighlighted)
    {
        trenchStructureSlotHighlighted.animator.enabled = true;
        //trenchStructureSlotHighlighted.animator.Play("StructureAnimation");
    }

    void SlotUnHighlighted(TrenchStructureSlot trenchStructureSlotHighlighted)
    {
        //trenchStructureSlotHighlighted.animator.Play("StructureUnHighlighted");
        //trenchStructureSlotHighlighted.animator.Update(0);
        trenchStructureSlotHighlighted.animator.enabled = false;
        trenchStructureSlotHighlighted.spriteRenderer.color = Color.white;
    }
}
