using UnityEngine;
using System.Collections.Generic;

public class PlayerMoveInventorySO : ScriptableObject
{
    public List<MoveSO> violentAttacksInventory = new List<MoveSO>();
    public List<MoveSO> violentFendsInventory = new List<MoveSO>();
    public List<MoveSO> violentFocusesInventory = new List<MoveSO>();

    public List<MoveSO> cautiousAttacksInventory = new List<MoveSO>();
    public List<MoveSO> cautiousFendsInventory = new List<MoveSO>();
    public List<MoveSO> cautiousFocusesInventory = new List<MoveSO>();

    public List<MoveSO> preciseAttacksInventory = new List<MoveSO>();
    public List<MoveSO> preciseFendsInventory = new List<MoveSO>();
    public List<MoveSO> preciseFocusesInventory = new List<MoveSO>();

    public MoveSO[] violentAttacksEquipped = new MoveSO[5];
    public MoveSO[] violentFendsEquipped = new MoveSO[5];
    public MoveSO[] violentFocusesEquipped = new MoveSO[5];

    public MoveSO[] cautiousAttacksEquipped = new MoveSO[5];
    public MoveSO[] cautiousFendsEquipped = new MoveSO[5];
    public MoveSO[] cautiousFocusesEquipped = new MoveSO[5];

    public MoveSO[] preciseAttacksEquipped = new MoveSO[5];
    public MoveSO[] preciseFendsEquipped = new MoveSO[5];
    public MoveSO[] preciseFocusesEquipped = new MoveSO[5];
}