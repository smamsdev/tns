
using UnityEngine;

public class PlayerPermanentStats : ScriptableObject
{
    private void OnEnable()
    {
        FieldEvents.UpdateXP += UpdateXP;
    }

    private void OnDisable()
    {
        FieldEvents.UpdateXP -= UpdateXP;
    }

    public int fendBase;
    public int attackPowerBase;
    public int playerFocusbase;
    public int maxPotential;
    public int maxHP;


    [Header("Exp")]
    public int level;
    public int XP;
    public int XPThreshold;
    public int XPremainder;
    public int defaultXPThreshold;

    public void InitaliseplayerPermanentStats()
    {
       // level up stuff
       // XPremainder = 0;
       // XPThreshold = defaultXPThreshold + (level * 30);
       //
       // TotalPlayerMovePower();
       // CombatEvents.InitializePlayerPotUI?.Invoke(playerCurrentPotential);


    }

    public void LevelUp()
    {
        // defaultPlayerFendBase = defaultPlayerFendBase + Mathf.CeilToInt(defaultPlayerFendBase * 0.02f);
        // defaultPlayerAttackPowerBase = defaultPlayerAttackPowerBase + Mathf.CeilToInt(defaultPlayerAttackPowerBase * 0.02f);
        // defaultPlayerFocusbase = defaultPlayerFocusbase + Mathf.CeilToInt(defaultPlayerFocusbase * 0.02f);
        // level++;
        // 
        // XP = 0 + XPremainder;
        // Debug.Log("LEVELUP");
    }

    public void UpdateXP(GameObject gameObject)
    {
        var enemy = gameObject.transform.GetChild(0).GetComponent<Enemy>();
        XP += enemy.enemyXP;
        XPremainder = XP - XPThreshold;

        if (XPThreshold <= XP)

        {
            LevelUp();
        }

        InitaliseplayerPermanentStats();
    }

}
      
