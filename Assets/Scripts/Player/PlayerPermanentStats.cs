using UnityEngine;

[CreateAssetMenu]

public class PlayerPermanentStats : AllyPermanentStats
{
    [Header("Base Stats")]
    public int focusBase;
    public int maxPotential;
    public int currentPotential;
    public int smams;
    public int focusBaseGrowth;

    public override void InitialisePlayerPermanentStats()
    {
        currentHP = maxHP;
        currentPotential = maxPotential;
        XPThreshold = NextLevelThreshold();
    }

    public override void LevelUp()
    {
        XPThreshold = NextLevelThreshold();
        attackBase += StatGrowth(attackBaseGrowth);
        fendBase += StatGrowth(fendBaseGrowth);
        focusBase += StatGrowth(focusBaseGrowth);
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
}
