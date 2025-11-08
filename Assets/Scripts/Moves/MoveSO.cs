using UnityEngine;

[CreateAssetMenu(menuName = "Move")]
public class MoveSO : ScriptableObject
{
    [Header("Monobehaviour Prefab")]
    [SerializeField] private GameObject movePrefab;

    [Header("Move")]
    [SerializeField] private string moveName;
    [SerializeField, TextArea(2, 5)] private string moveDescription;
    [SerializeField] private int moveWeighting;
    [SerializeField] private float attackPushStrength;
    [SerializeField] private float attackMoveModPercent;
    [SerializeField] private float fendMoveModPercent;
    [SerializeField] private float moveAnimationFloat = 0;
    [SerializeField] private float targetPositionHorizontalOffset;
    [SerializeField] private bool offsetFromSelf;
    [SerializeField] private bool applyMoveToSelfOnly;

    [Header("Player Specific")]
    [SerializeField] private int potentialChange;
    [SerializeField] private bool isFlaw;
    public bool isEquipped;

    //Getters
    [HideInInspector] public GameObject MovePrefab => movePrefab;
    [HideInInspector] public string MoveName => moveName;
    [HideInInspector] public string MoveDescription => moveDescription;
    [HideInInspector] public int MoveWeighting => moveWeighting;
    [HideInInspector] public float AttackPushStrength => attackPushStrength;
    [HideInInspector] public float AttackMoveModPercent => attackMoveModPercent;
    [HideInInspector] public float FendMoveModPercent => fendMoveModPercent;
    [HideInInspector] public float MoveAnimationFloat => moveAnimationFloat;
    [HideInInspector] public float TargetPositionHorizontalOffset => targetPositionHorizontalOffset;
    [HideInInspector] public bool OffsetFromSelf => offsetFromSelf;
    [HideInInspector] public bool ApplyMoveToSelfOnly => applyMoveToSelfOnly;
    [HideInInspector] public int PotentialChange => potentialChange;
    [HideInInspector] public bool IsFlaw => isFlaw;
}
