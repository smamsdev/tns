using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu]

public class PlayerEquippedMovesSO : ScriptableObject
{
    public string[] violentAttacksListString = new string[5];
    public string[] violentFendsListString = new string[5];
    public string[] violentFocusesListString = new string[5];

    public string[] cautiousAttackssListString = new string[5];
    public string[] cautiousFendsListString = new string[5];
    public string[] cautiousFocusesListString = new string[5];

    public string[] preciseAttacksListString = new string[5];
    public string[] preciseFendsListString = new string[5];
    public string[] preciseFocusesListString = new string[5];

}
