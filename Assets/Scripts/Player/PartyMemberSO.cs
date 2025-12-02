using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class PartyMemberSO : ScriptableObject
{
    public GameObject prefab;

    public int AttackBase
        {
        get { return attackBase; }
        set
        {
            attackBase = Mathf.Clamp(value, 1, 999);
        }
    }

    public int FendBase
    {
        get { return fendBase; }
        set
        {
            fendBase = Mathf.Clamp(value, 1, 999);
        }
    }

    public int MaxHP
    {
        get { return maxHP; }
        set
        {
            maxHP = Mathf.Clamp(value, 1, 9999);
        }
    }

    public int CurrentHP
        {
        get { return currentHP; }
        set
        {
            currentHP = Mathf.Clamp(value, 0, 9999);
        }
    }

    public int Level
    {
        get { return level; }
        set
        {
            level = Mathf.Clamp(value, 0, 100);
        }
    }

    public int XP
    {
        get { return xP; }
        set
        {
            xP = Mathf.Clamp(value, 0, 100);
        }
    }

    public int XPThreshold
    {
        get { return xPThreshold; }
        set
        {
            XPThreshold = Mathf.Clamp(value, 0, 100);
        }
    }

    [SerializeField] private int attackBase;
    [SerializeField] private int fendBase;
    [SerializeField] private int maxHP;
    [SerializeField] private int currentHP;
    [SerializeField] int level;
    [SerializeField] int xP;
    [SerializeField] int xPThreshold;


    [SerializeField] int attackBaseGrowth;
    [SerializeField] int fendBaseGrowth;

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