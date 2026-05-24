using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PlayerPermanentStats : PartyMemberPermanentStats
{
    [SerializeField] private int maxPotential;
    public int MaxPotential
    {
        get => Mathf.Clamp(maxPotential, 1, 999);
        set => maxPotential = Mathf.Clamp(value, 1, 999);
    }

    [SerializeField] private int currentPotential;
    public int CurrentPotential
    {
        get => Mathf.Clamp(currentPotential, 0, maxPotential);
        set => currentPotential = Mathf.Clamp(value, 0, maxPotential);
    }

    [SerializeField] private int focusBase;
    public int FocusBase
    {
        get => Mathf.Clamp(focusBase, 1, 999);
        set => focusBase = Mathf.Clamp(value, 1, 999);
    }

    [SerializeField] private int virtuosity;
    public int Virtuosity
    {
        get => Mathf.Clamp(virtuosity, 1, 999);
        set => virtuosity = Mathf.Clamp(value, 1, 999);
    }

    [SerializeField] private int smams;
    public int Smams
    {
        get => Mathf.Clamp(smams, 0, 1000000);
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