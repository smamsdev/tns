using UnityEngine;
using System.Collections.Generic;
using NUnit.Framework.Interfaces;

public class Enemy : Ally
{
    public int XPReward;
    public List<GearDrop> gearDrops = new();

    public GearSO ItemDrop()
    {
        if (gearDrops == null)
            return null;

        int weightingTotal = 0;

        foreach (GearDrop gearDrop in gearDrops)
            if (gearDrop.RewardWeighting > 0)
                weightingTotal += gearDrop.RewardWeighting;

        if (weightingTotal == 0)
            Debug.Log("no gear available to drop or weightings not set correctly");

        int randomValue = Random.Range(1, weightingTotal + 1);

        foreach (GearDrop gearDrop in gearDrops)
        {
            if (gearDrop.RewardWeighting == 0)
                continue;

            if (randomValue > gearDrop.RewardWeighting)
                randomValue -= gearDrop.RewardWeighting;

            else
            {
                return gearDrop.GearReward;
            }
        }

        Debug.LogError("Failed to select a drop! This should never happen. Random value was " + randomValue);
        return null;
    }
}

[System.Serializable]
public class GearDrop
{
    public GearSO GearReward => gearReward;
    public int RewardWeighting => rewardWeighting;

    [SerializeField] GearSO gearReward;
    [SerializeField] int rewardWeighting;
}
