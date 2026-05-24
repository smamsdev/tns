using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[CreateAssetMenu]
public class PartyMemberPermanentStats : ScriptableObject
{
    public GameObject prefab;

    public List<MoveSO> moveList = new();

    [SerializeField] private int attackBase;
    public int AttackBase
    {
        get => Mathf.Clamp(attackBase, 1, 999);
        set => attackBase = Mathf.Clamp(value, 1, 999);
    }

    [SerializeField] private int fendBase;
    public int FendBase
    {
        get => Mathf.Clamp(fendBase, 1, 999);
        set => fendBase = Mathf.Clamp(value, 1, 999);
    }

    [SerializeField] private int maxHP;
    public int MaxHP
    {
        get => Mathf.Clamp(maxHP, 1, 9999);
        set => maxHP = Mathf.Clamp(value, 1, 9999);
    }

    [SerializeField] private int currentHP;
    public int CurrentHP
    {
        get => Mathf.Clamp(currentHP, 0, MaxHP);
        set => currentHP = Mathf.Clamp(value, 0, MaxHP);
    }

    [SerializeField] private int level;
    public int Level
    {
        get => Mathf.Clamp(level, 1, 100);
        set => level = Mathf.Clamp(value, 1, 100);
    }

    [SerializeField] private int xP;
    public int XP
    {
        get => Mathf.Clamp(xP, 0, 10000000);
        set => xP = Mathf.Clamp(value, 0, 10000000);
    }

    [SerializeField] private int xPThreshold;
    public int XPThreshold
    {
        get => xPThreshold;
        set => xPThreshold = Mathf.Clamp(value, 0, 10000000);
    }

    [SerializeField] private int attackBaseGrowth;
    public int AttackBaseGrowth
    {
        get => Mathf.Clamp(attackBaseGrowth, 1, 10);
        set => attackBaseGrowth = Mathf.Clamp(value, 1, 10);
    }

    [SerializeField] private int fendBaseGrowth;
    public int FendBaseGrowth
    {
        get => Mathf.Clamp(fendBaseGrowth, 1, 10);
        set => fendBaseGrowth = Mathf.Clamp(value, 1, 10);
    }

    [SerializeField] private int maxHPGrowth;
    public int MaxHPGrowth
    {
        get => Mathf.Clamp(maxHPGrowth, 1, 10);
        set => maxHPGrowth = Mathf.Clamp(value, 1, 10);
    }

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