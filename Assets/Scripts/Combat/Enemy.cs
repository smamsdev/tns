using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Enemy : Ally
{
    [Header("")]
    public int XPReward;
    public GearSO itemReward;

    [HideInInspector] int enemyBodyMaxHP;
    [HideInInspector] int enemyArmsMaxHP;
    [HideInInspector] int enemyHeadMaxHP;

    [Header("Misc")]
    int moveWeightingTotal = 0;
    public int randomValue;
    public int rng;

    private void OnEnable()
    {
        movementScript = GetComponent<MovementScript>();
    }

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
