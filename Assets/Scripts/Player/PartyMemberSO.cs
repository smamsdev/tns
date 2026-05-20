using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PartyMemberPermanentStats : ScriptableObject
{
    public GameObject prefab;

    public List<MoveSO> moveList = new();

    [SerializeField] private int attackBase;
    public int AttackBase
    {
        get => attackBase;
        set => attackBase = Mathf.Clamp(value, 1, 999);
    }

    [SerializeField] private int fendBase;
    public int FendBase
    {
        get => fendBase;
        set => fendBase = Mathf.Clamp(value, 1, 999);
    }

    [SerializeField] private int maxHP;
    public int MaxHP
    {
        get => maxHP;
        set => maxHP = Mathf.Clamp(value, 1, 9999);
    }

    [SerializeField] private int currentHP;
    public int CurrentHP
    {
        get => currentHP;
        set => currentHP = Mathf.Clamp(value, 0, MaxHP);
    }

    [SerializeField] private int level;
    public int Level
    {
        get => level;
        set => level = Mathf.Clamp(value, 0, 100);
    }

    [SerializeField] private int xP;
    public int XP
    {
        get => xP;
        set => xP = Mathf.Clamp(value, 0, 10000000);
    }

    [SerializeField] private int xPThreshold;
    public int XPThreshold
    {
        get => xPThreshold;
        set => xPThreshold = Mathf.Clamp(value, 0, 10000000);
    }

    [SerializeField] private int attackBaseGrowth;
    public int AttackBaseGrowth => attackBaseGrowth;

    [SerializeField] private int fendBaseGrowth;
    public int FendBaseGrowth => fendBaseGrowth;

    [SerializeField] private int maxHPGrowth;
    public int MaxHPGrowth => maxHPGrowth;

    private void OnValidate()
    {
        if (moveList.Count == 0)
            Debug.Log("movelist empty for SO " + this, this);

        UpdateThreshold();
    }

    public virtual void LevelUp()
    {
        XPThreshold = NextLevelThreshold();
        AttackBase += StatGrowth(AttackBaseGrowth);
        FendBase += StatGrowth(FendBaseGrowth);
        MaxHP += StatGrowth(MaxHPGrowth);
        Level++;
    }

    public int StatGrowth(int growthFactor)
    {
        float rawStatGrowth = (growthFactor * level) / 10;
        int roundedStatGrowth = Mathf.CeilToInt(rawStatGrowth);
        return roundedStatGrowth;
    }

    public int NextLevelThreshold()
    {
        float rawXP = 100 + (level * level * level) * 2;
        int roundedXP = Mathf.RoundToInt(rawXP / 25f) * 25;
        return roundedXP;
    }

    public void UpdateThreshold()
    {
        XPThreshold = NextLevelThreshold();
    }

    public string GetHPString()
    {
        return CurrentHP + " / " + MaxHP;
    }
}