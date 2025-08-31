using UnityEngine;

[CreateAssetMenu]

public class AllyPermanentStats : ScriptableObject
{
    [Header("Base Stats")]
    public int attackBase;
    public int fendBase;
    public int maxHP;
    public int currentHP;

    [Header("Exp")]
    public int level;
    public int XP;
    public int XPThreshold;
    public int attackBaseGrowth;
    public int fendBaseGrowth;

    public virtual void InitialisePlayerPermanentStats()
    {
        currentHP = maxHP;
        XPThreshold = NextLevelThreshold();
    }

    public virtual void LevelUp()
    {
        XPThreshold = NextLevelThreshold();
        attackBase += StatGrowth(attackBaseGrowth);
        fendBase += StatGrowth(fendBaseGrowth);
        level++;
    }

    int StatGrowth(int growthFactor)
    {
        int rawStatGrowth;
        int roundedStatGrowth;

        rawStatGrowth = (growthFactor * level) / 10;
        roundedStatGrowth = Mathf.CeilToInt(rawStatGrowth);
        return roundedStatGrowth;
    }

    public int NextLevelThreshold()
    {
        int rawXP = 100 + (level * level * level) * 2;
        int roundedXP = Mathf.RoundToInt(rawXP / 25f) * 25;
        return roundedXP;
    }

    public void UpdateThreshold()
    {
        XPThreshold = NextLevelThreshold();
    }
}
