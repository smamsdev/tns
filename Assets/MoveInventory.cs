using System.Collections.Generic;
using UnityEngine;

public class MoveInventory : MonoBehaviour
{
    public PlayerMoveInventorySO moveInventorySO;

    public List<ViolentMove> violentAttacksInventory = new List<ViolentMove>();
    public List<ViolentMove> violentFendsInventory = new List<ViolentMove>();
    public List<ViolentMove> violentFocusesInventory = new List<ViolentMove>();

    public List<CautiousMove> cautiousAttacksInventory = new List<CautiousMove>();
    public List<CautiousMove> cautiousFendsInventory = new List<CautiousMove>();
    public List<CautiousMove> cautiousFocusesInventory = new List<CautiousMove>();

    public List<PreciseMove> preciseAttacksInventory = new List<PreciseMove>();
    public List<PreciseMove> preciseFendsInventory = new List<PreciseMove>();
    public List<PreciseMove> preciseFocusesInventory = new List<PreciseMove>();

    private void Start()
    {
        LoadAllInventoryMovesFromSO();
    }

    public void LoadAllInventoryMovesFromSO()
    {
        LoadMovesOfType(moveInventorySO.violentAttacksListString, violentAttacksInventory);
        LoadMovesOfType(moveInventorySO.violentAttacksListString, violentAttacksInventory);
        LoadMovesOfType(moveInventorySO.violentFendsListString, violentFendsInventory);
        LoadMovesOfType(moveInventorySO.violentFocusesListString, violentFocusesInventory);
        LoadMovesOfType(moveInventorySO.cautiousAttacksListString, cautiousAttacksInventory);
        LoadMovesOfType(moveInventorySO.cautiousFendsListString, cautiousFendsInventory);
        LoadMovesOfType(moveInventorySO.cautiousFocusesListString, cautiousFocusesInventory);
        LoadMovesOfType(moveInventorySO.preciseAttacksListString, preciseAttacksInventory);
        LoadMovesOfType(moveInventorySO.preciseFendsListString, preciseFendsInventory);
        LoadMovesOfType(moveInventorySO.preciseFocusesListString, preciseFocusesInventory);
    }

    public void LoadMovesOfType<T>(List<string> moveTypeInventory, List<T> moveList) where T : Component
    {
        moveList.Clear();

        foreach (var moveName in moveTypeInventory)
        {
            if (!string.IsNullOrEmpty(moveName))
            {
                var moveGameObject = GameObject.Find(moveName);
                var moveComponent = moveGameObject?.GetComponent<T>();

                if (moveComponent != null)
                {
                    moveList.Add(moveComponent);
                }
                else
                {
                    Debug.Log("No move of this name found: " + moveName);
                }
            }
            else
            {
                Debug.Log("Empty string found");
            }
        }
    }
}