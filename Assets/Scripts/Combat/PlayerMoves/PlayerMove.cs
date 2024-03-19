using UnityEngine;

public abstract class PlayerMove : MonoBehaviour
{

    public string moveName;
    [TextArea(2, 5)]
    public string moveDescription;

    [Header("")]
    public float attackMoveModPercent;
    public float fendMoveModPercent;
    public int potentialCost;
    public int potentialRestorePercent;
    public int moveWeighting;
    public bool botched;

}