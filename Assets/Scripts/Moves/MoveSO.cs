using TMPro;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

[CreateAssetMenu(menuName = "Move")]
public class MoveSO : ScriptableObject
{
    public enum Rarity {unassigned, common, uncommon, veryRare}

    [Header("Monobehaviour Prefab")]
    [SerializeField] private GameObject movePrefab;

    [Header("Move")]
    [SerializeField] private string moveName;
    [SerializeField] private int moveWeighting;
    [SerializeField] private float attackPushStrength;
    [SerializeField] private float attackMoveModPercent;
    [SerializeField] private float fendMoveModPercent;
    [SerializeField] private float moveAnimationFloat = 0;
    [SerializeField] private float targetPositionHorizontalOffset;
    [SerializeField] private bool targetPosSelfOffset;
    [SerializeField] private bool applyMoveToSelfOnly;

    [Header("Player Specific")]
    [SerializeField] private int potentialChangeInt;
    [SerializeField, TextArea(2, 5)] private string moveDescription;
    [SerializeField] private Rarity rarity;
    [SerializeField, TextArea(2, 5)] private string potentialChangeDescription;
    [SerializeField] private bool isFlaw;
    public bool isEquipped;

    public GameObject MovePrefab => movePrefab;
    public string MoveName => moveName;
    public string MoveDescription => moveDescription;
    public int MoveWeighting => moveWeighting;
    public float AttackPushStrength => attackPushStrength;
    public float AttackMoveModPercent
    {
        get => attackMoveModPercent;
        set => attackMoveModPercent = value;
    }
   
    public float FendMoveModPercent => fendMoveModPercent;
    public float MoveAnimationFloat => moveAnimationFloat;
    public float TargetPositionHorizontalOffset => targetPositionHorizontalOffset;
    public bool TargetPosSelfOffset => targetPosSelfOffset;
    public bool ApplyMoveToSelfOnly => applyMoveToSelfOnly;
    public int PotentialChange => potentialChangeInt;
    public string PotentialChangeDescription => "Potential change: " + potentialChangeDescription;
    public bool IsFlaw => isFlaw;

    public string GetRarityDescription()
    {
        switch (rarity)
        {
            case Rarity.unassigned:
                Debug.LogError($"rarity not assigned on {MoveName}", this);
                return "";

            case Rarity.uncommon:
                return "(Uncommon)";

            case Rarity.common:
                return "(Common)";
        }

        Debug.LogError($"Unknown rarity encountered on {MoveName}", this);
        return "";
    }
}