using UnityEngine;
using System.Collections.Generic;

public class PlayerMoveInventorySO : ScriptableObject
{
    public List<string> violentAttacksListString = new List<string>();
    public List<string> violentFendsListString = new List<string>();
    public List<string> violentFocusesListString = new List<string>();

    public List<string> cautiousAttacksListString = new List<string>();
    public List<string> cautiousFendsListString = new List<string>();
    public List<string> cautiousFocusesListString = new List<string>();

    public List<string> preciseAttacksListString = new List<string>();
    public List<string> preciseFendsListString = new List<string>();
    public List<string> preciseFocusesListString = new List<string>();
}