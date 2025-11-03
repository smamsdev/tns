using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Enemy : Ally
{
    public int XPReward;
    public GearSO itemReward;

    public GearSO ItemDrop()
    {
        if (itemReward != null)
        {
            if (Random.Range(0, 3) == 0)
            {
                return itemReward;
            }

            else { return null; }
        }
        else { return null; }
    }
}
