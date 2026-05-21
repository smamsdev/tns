using UnityEngine;

public class PlayerPermanentStats : PartyMemberPermanentStats
{
    [SerializeField] private int maxPotential;
    public int MaxPotential
    {
        get => maxPotential;
        set => maxPotential = Mathf.Clamp(value, 1, 999);
    }

    [SerializeField] private int currentPotential;
    public int CurrentPotential
    {
        get => currentPotential;
        set => currentPotential = Mathf.Clamp(value, 0, maxPotential);
    }

    [SerializeField] private int focusBase;
    public int FocusBase
    {
        get => focusBase;
        set => focusBase = Mathf.Clamp(value, 1, 999);
    }

    [SerializeField] private int smams;
    public int Smams
    {
        get => smams;
        set => smams = Mathf.Clamp(value, 0, 1000000);
    }

    [SerializeField] private int maxPotentialBaseGrowth;
    public int MaxPotentialBaseGrowth => maxPotentialBaseGrowth;

    [SerializeField] private int focusBaseGrowth;
    public int FocusBaseGrowth => focusBaseGrowth;

    [SerializeField] private bool isStatsMenuAvailable;
    public bool IsStatsMenuAvailable
    {
        get => isStatsMenuAvailable;
        set => isStatsMenuAvailable = value;
    }

    public override void LevelUp()
    {
        XPThreshold = NextLevelThreshold();
        AttackBase += StatGrowth(AttackBaseGrowth);
        FendBase += StatGrowth(FendBaseGrowth);
        MaxPotential += StatGrowth(MaxPotentialBaseGrowth);
        FocusBase += StatGrowth(FocusBaseGrowth);
        Level++;
    }

    public string SmamsFormattedString()
    {
        return "Account: " + Smams.ToString("N0") + " $MAMS";
    }

    public string GetPotentialString()
    {
        return CurrentPotential + " / " + MaxPotential;
    }
}