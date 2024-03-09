
using UnityEngine;


[CreateAssetMenu]

public class PlayerStatsSO : ScriptableObject
{

    private void OnEnable()
    {
        CombatEvents.UpdatePlayerFendMoveMod += UpdatePlayerFendMoveMod;
        CombatEvents.UpdatePlayerAttackMoveMod += UpdatePlayerAttackMoveMod;
        CombatEvents.UpdatePlayerHP += UpdatePlayerHP;
        CombatEvents.UpdatePlayerPotentialMoveCost += UpdatePlayerPotentialMoveCost;
        CombatEvents.UpdatePlayerPot += UpdatePlayerPot;
        FieldEvents.UpdateXP += UpdateXP;
    }

    private void OnDisable()
    {
        CombatEvents.UpdatePlayerFendMoveMod -= UpdatePlayerFendMoveMod;
        CombatEvents.UpdatePlayerAttackMoveMod += UpdatePlayerAttackMoveMod;
        CombatEvents.UpdatePlayerHP -= UpdatePlayerHP;
        CombatEvents.UpdatePlayerPotentialMoveCost -= UpdatePlayerPotentialMoveCost;
        CombatEvents.UpdatePlayerPot -= UpdatePlayerPot;
        FieldEvents.UpdateXP -= UpdateXP;
    }

    [Header("HP")]
    public int playerCurrentHP;
    public int playerMaxHP;
    int baseMaxCurrentHP = 100;
    int defaultPlayerMaxHP = 100;

    [Header("Fend")]
    public int playerFend;
    [SerializeField] float fendMoveMod;
    [SerializeField] int fendBase;

    [SerializeField] float fendPotMod;

    [Header("Power")]
    public int attackPower;
    [SerializeField] float attackPowerMoveMod;
    [SerializeField] int attackPowerBase;

    [SerializeField] float attackPowerPotMod;
    public float attackPowerGearMod;
    public float fendPowerGearMod;


    [Header("Potential")]
    [SerializeField] int playerCurrentPotential;
    [SerializeField] int playerMaxPotential;
    [SerializeField] int playerFocusbase;


    public float playerPotentialMoveCost;
    int defaultPlayerCurrentPotential = 50;
    int defaultPlayerMaxPotential = 100;

    [Header("Exp")]
    public int level;
    public int XP;
    public int XPThreshold;
    public int XPremainder;

    [Header("Other, careful with these")]
    [SerializeField] Vector2 position;
    public int defaultXPThreshold;
    public int defaultPlayerAttackPowerBase = 8;
    [SerializeField] int defaultPlayerFendBase = 8;
    [SerializeField] int defaultPlayerFocusbase = 10;
    [SerializeField] int playerFocusbaseChange;
    [SerializeField] int attackPowerBaseChange;
    [SerializeField] int fendBaseChange;


    public void InitalisePlayerStats()
    {
        playerCurrentHP = defaultPlayerMaxHP;
        playerMaxHP = defaultPlayerMaxHP;
        fendBase = defaultPlayerFendBase;

        attackPowerBase = defaultPlayerAttackPowerBase;

        playerCurrentPotential = defaultPlayerCurrentPotential;
        playerMaxPotential = defaultPlayerMaxPotential;

        playerFocusbase = defaultPlayerFocusbase;

        playerPotentialMoveCost = 0;

        attackPowerBaseChange = 0;
        fendBaseChange = 0;
        playerFocusbaseChange = 0;

        attackPowerGearMod = 0;
        fendPowerGearMod = 0;



        XPremainder = 0;
        XPThreshold = defaultXPThreshold + (level * 30);

        TotalPlayerMovePower();
        CombatEvents.InitializePlayerPotUI?.Invoke(playerCurrentPotential);


    }

    public void CheckForPotPunishment()
    {
        if (playerCurrentPotential == 0)
        {
            attackPowerPotMod = Mathf.CeilToInt(attackPowerBase * -0.75f);
            fendPotMod = Mathf.CeilToInt(fendBase * -0.75f);

            Debug.Log("punishing");
        }

        else
        {
            attackPowerPotMod = (100 - playerCurrentPotential) * -0.025f;
            fendPotMod = (100 - playerCurrentPotential) * -0.05f;
        }
    }

    public int TotalPlayerMovePower(string returnType)
    {
        CheckForPotPunishment();
        attackPower = Mathf.Clamp(Mathf.RoundToInt(attackPowerBase + attackPowerMoveMod + attackPowerPotMod + attackPowerGearMod), 0, 9999);
        playerFend = Mathf.Clamp(Mathf.RoundToInt(fendBase + fendMoveMod + fendPotMod), 0, 9999);

        if (returnType == "attack")

        { return attackPower; }

        if (returnType == "fend")

        { return playerFend; }

        else

        {
            Debug.Log("whoops");
            return attackPower;
        }
    }

    public void TotalPlayerMovePower()
    {
        CheckForPotPunishment();
        attackPower = Mathf.Clamp(Mathf.RoundToInt(attackPowerBase + attackPowerMoveMod + attackPowerPotMod + attackPowerGearMod), 0, 9999);
        playerFend = Mathf.Clamp(Mathf.RoundToInt(fendBase + fendMoveMod + fendPotMod), 0, 9999);
    }

    public void UpdatePlayerAttackMoveMod(float moveModMultiplier, bool isAttack)
    { if (isAttack)
        { attackPowerMoveMod = attackPowerBase * moveModMultiplier; }
    else { attackPowerMoveMod = -attackPowerBase; }

        TotalPlayerMovePower();
    }

    public void UpdatePlayerFendMoveMod(float moveModMultiplier, bool isFend)
    {
        if (isFend)
        {
            fendMoveMod = (fendBase * moveModMultiplier) + fendPowerGearMod;
         }
        else { fendMoveMod -= fendBase; }

        TotalPlayerMovePower();
    }

    public void UpdatePlayerPotentialMoveCost(float moveModMultiplier, int focusMoveCost, bool isFocus)
    {
        if (isFocus)
        { playerPotentialMoveCost = (playerFocusbase * moveModMultiplier) + focusMoveCost;

            playerFocusbaseChange++;
            attackPowerBaseChange++;
            fendBaseChange++;
        }
        else 
        { 
            playerPotentialMoveCost = focusMoveCost + (playerFocusbase * moveModMultiplier);
        }

    }

    public void UpdatePlayerPot(int value)
    {
        playerCurrentPotential = Mathf.Clamp(playerCurrentPotential+value, 0, 100);
        
        playerFocusbase += playerFocusbaseChange;
        attackPowerBase += attackPowerBaseChange;
        fendBase += fendBaseChange;

        CombatEvents.UpdatePlayerPotOnUI(playerCurrentPotential);
    }

     void UpdatePlayerHP(int value)
    {
        playerCurrentHP += value;
        CombatEvents.UpdatePlayerHPDisplay?.Invoke(playerCurrentHP);
    }

    public void ResetAllMoveMods()

    {
        attackPowerMoveMod = 0;
        fendMoveMod = 0;
        playerPotentialMoveCost = 0;
    }

    public void LevelUp()
    {
        defaultPlayerFendBase = defaultPlayerFendBase + Mathf.CeilToInt(defaultPlayerFendBase * 0.02f);
        defaultPlayerAttackPowerBase = defaultPlayerAttackPowerBase + Mathf.CeilToInt(defaultPlayerAttackPowerBase * 0.02f);
        defaultPlayerFocusbase = defaultPlayerFocusbase + Mathf.CeilToInt(defaultPlayerFocusbase * 0.02f);
        level++;

        XP = 0 + XPremainder;
        Debug.Log("LEVELUP");
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

        InitalisePlayerStats();
    }

}
      
