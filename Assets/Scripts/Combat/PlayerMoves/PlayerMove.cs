using UnityEngine;

public abstract class PlayerMove : MonoBehaviour
{

    public string moveName;
    [TextArea(2, 5)]
    public string moveDescription;

    [Header("")]
    public float attackMoveMultiplier;
    public float damageToBodyMultiplier;
    public float fendMoveMultiplier;
    public int potentialChange;
    public int moveWeighting;

    [Header("")]
    public bool isAttack;
    [Header("")]
    public bool isFend;

    public abstract void OnApplyMove();


    public abstract void OnEnemyAttack();

}