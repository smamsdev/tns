using System.Collections;
using UnityEngine;

public abstract class PlayerMove : MonoBehaviour
{

    public string moveName;
    [TextArea(2, 5)]
    public string moveDescription;
    public bool isEquipped;
    public bool isFlaw;

    [Header("")]
    public float attackMoveMultiplier;
    public float damageToPartsMultiplier;
    public float fendMoveMultiplier;
    public int potentialChange;
    public int moveWeighting;
    public float attackPushStrength;

    [Header("")]
    public bool isAttack;

    public CombatManager combatManager;
    public Enemy enemy;

    public abstract IEnumerator OnApplyMove(CombatManager _combatManager, Enemy _enemy);

    public abstract IEnumerator Return();

    public abstract IEnumerator OnEnemyAttack(CombatManager _combatManager, Enemy _enemy);

}