using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;
using static StatModifier;
using static UnityEngine.Rendering.DebugUI;

public class PlayerCombat : PartyMemberCombat
{
    public PlayerPermanentStats playerPermanentStats;
    public PartySO party;
    public PlayerInventorySO playerInventorySO;
    public PlayerMoveInventorySO playerMoveInventorySO;
    public GameObject gearMonoBehaviourParentFolder;
    public List<GearMonoBehaviour> gearBehaviours = new();
    public int styleType;
    public int actionType;
    
    public CombatManager combatManager;

    [SerializeField] private int maxPotential;
    public int MaxPotential
    {
        get => (maxPotential);
        set => maxPotential = Mathf.Clamp(value, 0, 9999);
    }

    [SerializeField] private int currentPotential;
    public int CurrentPotential
    {
        get => (currentPotential);
        set => currentPotential = Mathf.Clamp(value, 0, 9999);
    }

    [SerializeField] int focusBase;
    public int FocusBase
    {
        get => (focusBase);
        set => focusBase = Mathf.Clamp(value, 0, 9999);
    }

    public float fendPotMod;
    public float attackPowerPotMod;

    private void OnEnable()
    {
        CombatEvents.UpdatePlayerPot += UpdatePlayerPot;
        movementScript = GetComponent<MovementScript>();
    }

    private void OnDisable()
    {
        CombatEvents.UpdatePlayerPot -= UpdatePlayerPot;
    }

    private void Start()
    {
        InitStatsFromSO();
        combatManager = GameObject.FindGameObjectWithTag("CombatManager").GetComponent<CombatManager>();
        InstantiateAllEquippedGearBehaviours();
        ApplyAllGearStatMods();
    }

    public float CalculatePotentialMod()
    {
        float potentialMod = 0;

        if (currentPotential <= 0)
        {
            potentialMod = -0.1f;
        }

        if (currentPotential == playerPermanentStats.MaxPotential)
        {
            potentialMod = 2;
        }

        if (currentPotential > 0 && currentPotential < ( (float) (playerPermanentStats.MaxPotential /2) ))
        {   
            potentialMod = ((float)currentPotential / playerPermanentStats.MaxPotential) * 2.5f;
        }

        if (currentPotential< playerPermanentStats.MaxPotential && currentPotential >= ((float)(playerPermanentStats.MaxPotential / 2)))
        {
            potentialMod = 1;
        }

        return potentialMod;
    }

    public void UpdatePlayerPot(int value)
    {
        currentPotential += value;
        PlayerStatsDisplay playerStatsDisplay = combatantUI.statsDisplay as PlayerStatsDisplay;
        StartCoroutine(playerStatsDisplay.UpdatePlayerPotentialUI(Mathf.RoundToInt(currentPotential)));
    }

    public void CombineStanceAndMove()
    { 
        switch (styleType)
        {
            case 0: // Violent stance
                switch (actionType)
                {
                    case 0: 
                        SelectMove(playerMoveInventorySO.violentAttacksEquipped);break;
                    case 1: 
                        SelectMove(playerMoveInventorySO.violentFendsEquipped);
                        break;
                    case 2: 
                        SelectMove(playerMoveInventorySO.violentFocusesEquipped);
                        break;
                }
                break;

            case 1: // Cautious stance
                switch (styleType)
                {
                    case 0: SelectMove(playerMoveInventorySO.cautiousAttacksEquipped);break;
                    case 1: SelectMove(playerMoveInventorySO.cautiousFendsEquipped); break;
                    case 2: SelectMove(playerMoveInventorySO.cautiousFocusesEquipped); break;
                }
                break;

            case 2: // Precise stance
                switch (styleType)
                {
                    case 0: SelectMove(playerMoveInventorySO.preciseAttacksEquipped);break;
                    case 1: SelectMove(playerMoveInventorySO.preciseFendsEquipped); break;
                    case 2: SelectMove(playerMoveInventorySO.preciseFocusesEquipped); break;
                }
                break;

            default:
                {
                    Debug.Log("somethig went wrong");
                    Debug.DebugBreak();
                    break;
                }
        }
    }

    void SelectMove(MoveSO[] equippedMoveSOs)
    {
        int MoveWeightingTotal = 0;

        foreach (MoveSO moveSO in equippedMoveSOs)
        {
            if (moveSO != null && moveSO.MoveWeighting > 0)
                MoveWeightingTotal += moveSO.MoveWeighting;
        }

        if (MoveWeightingTotal == 0)
        {
            Debug.LogError("No valid moves available to select!!");
            return;
        }

        int randomValue = UnityEngine.Random.Range(1, MoveWeightingTotal + 1);

        foreach (MoveSO moveSO in equippedMoveSOs)
        {
            if (moveSO.MoveWeighting == 0) continue;

            if (randomValue > moveSO.MoveWeighting)
            {
                randomValue -= moveSO.MoveWeighting;
            }
            else
            {
                moveSOSelected = moveSO;
                InstantiateMoveBehaviour(moveSO);
                currentMoveBehaviour.LoadMoveReferences(this, combatManager);
                currentMoveBehaviour.CalculateMoveStats();
                return;
            }
        }

        Debug.LogError("Failed to select a move! This should never happen. Random value was " + randomValue);
    }

    public void InitStatsFromSO()
    {
        MaxHP = playerPermanentStats.MaxHP;
        CurrentHP = playerPermanentStats.CurrentHP;
        MaxPotential = playerPermanentStats.MaxPotential;
        CurrentPotential = playerPermanentStats.CurrentPotential;
        AttackBase = playerPermanentStats.AttackBase;
        FendBase = playerPermanentStats.FendBase;
        focusBase = playerPermanentStats.FocusBase;
    }

    public void GearConsumed(GearSO gearToUnequip)
    {
        //gearToUnequip.isCurrentlyEquipped = false;
        //int index = inventorySO.equippedGear.IndexOf(gearToUnequip);
        //inventorySO.equippedGear[index] = null;

        Debug.Log("fix");
    }

    public void InstantiateAllEquippedGearBehaviours()
    {
        ClearAllGearBehaviours();

        for (int i = 0; i < playerInventorySO.gearInstanceEquipped.Count; i++)
        {
            var gearInstance = playerInventorySO.gearInstanceEquipped[i];

            if (gearInstance.gearSO != null)
            {
                InstantiateGearBehaviour(combatManager, gearInstance, i);
            }
            else
            {
                GameObject gameObject = new GameObject("EquipSlot" + (i+1) + "Empty");

                gameObject.transform.SetParent(gearMonoBehaviourParentFolder.transform, false);
                gearBehaviours.Add(null);
            }
        }
    }

    void ClearAllGearBehaviours()
    {
        gearBehaviours.Clear();

        foreach (Transform child in gearMonoBehaviourParentFolder.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void InstantiateGearBehaviour(CombatManager combatManager, GearInstance gearInstance, int i)
    {
        GameObject gearMonoBehaviourGO = Instantiate(gearInstance.gearSO.MonobehaviourPrefab, gearMonoBehaviourParentFolder.transform);
        gearMonoBehaviourGO.name = "EquipSlot" + (i + 1) +gearInstance.gearSO.name + "Monobehaviour";

        GearMonoBehaviour gearMonoBehaviour = gearMonoBehaviourGO.GetComponent<GearMonoBehaviour>();

        gearMonoBehaviour.SetGearInstance(gearInstance);
        gearMonoBehaviour.combatManager = combatManager;
        gearMonoBehaviour.OnEquipGear();

        gearBehaviours.Add(gearMonoBehaviour);
    }

    public void ApplyAllGearStatMods()
    {
        foreach (GearInstance gearInstance in playerInventorySO.gearInstanceEquipped)
        {
            if (gearInstance.gearSO == null)
                continue;

            if (gearInstance is EquipmentInstance equipmentInstance && equipmentInstance.Charge <= 0)
                continue;

            foreach (StatModifier statModifier in gearInstance.gearSO.StatModifiers)
            {
                //Debug.Log(gearInstance.gearSO.GearName, gearInstance.gearSO);
                ChangeStat(statModifier);
            }
        }
    }

    public override void ChangeStat(StatModifier mod)
    {
        switch (mod.statToChange)
        {
            case StatToChange.AttackBase:
                AttackBase = Apply(AttackBase, mod);
                break;

            case StatToChange.FendBase:
                FendBase = Apply(FendBase, mod);
                break;

            case StatToChange.MaxHP:
                MaxHP = Apply(MaxHP, mod);
                break;

            case StatToChange.MaxPotential:
                MaxPotential = Apply(MaxPotential, mod);
                break;

            case StatToChange.FocusBase:
                FocusBase = Apply(FocusBase, mod);
                break;
        }

        static int Apply(int value, StatModifier mod)
        {
            if (mod.modifierType == ModifierType.Flat)
                return value + Mathf.RoundToInt(mod.amount);

            return Mathf.RoundToInt(value * mod.amount);
        }
    }

}
