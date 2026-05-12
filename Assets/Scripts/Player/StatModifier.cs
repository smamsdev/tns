using UnityEngine;

[System.Serializable]
public class StatModifier
{
    public enum StatToChange
    {
        AttackBase,
        FendBase,
        FocusBase,
        MaxHP,
        MaxPotential,
        Smams,
        XP
    }

    public enum ModifierType
    {
        Flat,
        Multiplier
    }

    public StatToChange statToChange;
    public ModifierType modifierType;
    public float amount;
}
