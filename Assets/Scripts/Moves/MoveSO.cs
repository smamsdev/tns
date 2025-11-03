using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class MoveSO : ScriptableObject
{
    public string moveName;
    [TextArea(2, 5)]
    public string moveDescription;

    [Header("")]
    public int moveWeighting;
    public float attackPushStrength;
    public float attackMoveModPercent;
    public float fendMoveModPercent;
    public float moveAnimationFloat = 0;
    public float targetPositionHorizontalOffset;
    public bool offsetFromSelf;
    public bool applyMoveToSelfOnly;

    [Header("Player Specific")]
    public int potentialChange;
    public bool isFlaw;
    public bool isEquipped;

    public GameObject movePrefab;
    [System.NonSerialized] public Move moveInstance;

    [HideInInspector] public CombatManager combatManager;
    [HideInInspector] public Animator combatantToActAnimator;
    [HideInInspector] public MovementScript combatantToActMovementScript;
    [HideInInspector] public Combatant combatantToAct, targetCombatant;
}
