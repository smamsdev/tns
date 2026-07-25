using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;
using static StatModifier;
using static UnityEngine.Rendering.DebugUI;

public class PlayerCombat : PartyMemberCombat
{
    [Header("Player Specific")]

    [SerializeField] private int _maxPotential;
    public int MaxPotential
    {
        get => (_maxPotential);
        set => _maxPotential = Mathf.Clamp(value, 0, 9999);
    }

    [SerializeField] private int _currentPotential;
    public int CurrentPotential
    {
        get => (_currentPotential);
        set => _currentPotential = Mathf.Clamp(value, 0, MaxPotential);
    }

    [SerializeField] int _focusBase;
    public int FocusBase
    {
        get => (_focusBase);
        set => _focusBase = Mathf.Clamp(value, 0, 9999);
    }

    public List<GearMonoBehaviour> gearBehaviours = new();

    public float fendPotMod;
    public float attackPowerPotMod;

    [Header("Player refs")]
    public PlayerPermanentStats playerPermanentStats;
    public PartySO partySO;
    public PlayerInventorySO playerInventorySO;
    public PlayerMoveInventorySO playerMoveInventorySO;
    public GameObject gearMonoBehaviourParentFolder;

    [HideInInspector] public int styleType;
    [HideInInspector] public int actionType;

    [HideInInspector] public CombatManager combatManager;

    private void OnEnable()
    {
        movementScript = GetComponent<MovementScript>();
    }

    private void Start()
    {
        InitStatsFromSO();
        combatManager = GameObject.FindGameObjectWithTag("CombatManager").GetComponent<CombatManager>();
        InstantiateAllEquippedGearBehaviours();
    }

    public void SyncHierarchyToGearList()
    {
        for (int i = 0; i < gearBehaviours.Count; i++)
        {
            if (gearBehaviours[i] != null)
            {
                gearBehaviours[i].transform.SetSiblingIndex(i);
            }
        }
    }

    public float CalculatePotentialMod()
    {
        float potentialMod = 0;

        if (CurrentPotential <= 0)
        {
            potentialMod = -0.1f;
        }

        if (CurrentPotential == playerPermanentStats.MaxPotential)
        {
            potentialMod = 2;
        }

        if (CurrentPotential > 0 && CurrentPotential < ( (float) (playerPermanentStats.MaxPotential /2) ))
        {   
            potentialMod = ((float)CurrentPotential / playerPermanentStats.MaxPotential) * 2.5f;
        }

        if (CurrentPotential < playerPermanentStats.MaxPotential && CurrentPotential >= ((float)(playerPermanentStats.MaxPotential / 2)))
        {
            potentialMod = 1;
        }

        return potentialMod;
    }

    public void UpdatePlayerPot(int change)
    {
        CurrentPotential += change;
        PlayerStatsDisplay playerStatsDisplay = combatantUI.statsDisplay as PlayerStatsDisplay;
        StartCoroutine(playerStatsDisplay.UpdatePlayerPotentialUI(change, MaxPotential));
    }

    public override IEnumerator CombatUpdateHPCoRo(int change)
    {
        if (change >= 0)
            combatantUI.statsDisplay.HPTMPAnimator.Play("CombatUIStatPlus");

        else
            combatantUI.statsDisplay.HPTMPAnimator.Play("CombatUIStatMinus");

        int initialHP = CurrentHP;
        int finalHP = Mathf.Clamp(CurrentHP + change, 0, 9999);
        float lerpDuration = .5f;

        yield return FieldEvents.LerpValuesCoRo(initialHP, finalHP, lerpDuration, (output) =>
        {
            int outputInt = Mathf.RoundToInt(output);

            CurrentHP = outputInt;
            combatantUI.statsDisplay.UpdateHPDisplay(CurrentHP);

        });

        CurrentHP = finalHP;
        combatantUI.statsDisplay.UpdateHPDisplay(finalHP);

        if (CurrentHP == 0)
        {
            Debug.Log("teh");
            movementScript.animator.Play("Fall");
            yield return new WaitForSeconds(1f);
        }

        yield return new WaitForSeconds(.5f);
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
                switch (actionType)
                {
                    case 0: SelectMove(playerMoveInventorySO.cautiousAttacksEquipped);break;
                    case 1: SelectMove(playerMoveInventorySO.cautiousFendsEquipped); break;
                    case 2: SelectMove(playerMoveInventorySO.cautiousFocusesEquipped); break;
                }
                break;

            case 2: // Precise stance
                switch (actionType)
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
            if (moveSO == null || moveSO.MoveWeighting <= 0)
                continue;

            if (randomValue > moveSO.MoveWeighting)
            {
                randomValue -= moveSO.MoveWeighting;
            }
            else
            {
                InstantiateMoveBehaviour(moveSO);
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
        FocusBase = playerPermanentStats.FocusBase;
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

        int count = playerInventorySO.gearInstanceEquipped.Count;

        gearBehaviours = new List<GearMonoBehaviour>(count);

        for (int i = 0; i < count; i++)
        {
            gearBehaviours.Add(null);

            var gearInstance = playerInventorySO.gearInstanceEquipped[i].GetGearType();

            if (gearInstance.gearSO != null)
            {
                InstantiateGearBehaviour(gearInstance, i);
            }
            else
            {
               // new GameObject("EquipSlot" + (i + 1) + "Empty").transform.SetParent(gearMonoBehaviourParentFolder.transform, false);
            }
        }
    }

    public void InstantiateGearBehaviour(GearInstance gearInstance, int i)
    {
        GameObject gearMonoBehaviourGO = Instantiate(gearInstance.gearSO.MonobehaviourPrefab, gearMonoBehaviourParentFolder.transform);
        gearMonoBehaviourGO.name = "EquipSlot" + (i + 1) + gearInstance.gearSO.name + "Monobehaviour";

        GearMonoBehaviour gearMonoBehaviour = gearMonoBehaviourGO.GetComponent<GearMonoBehaviour>();

        gearMonoBehaviour.gearInstance = gearInstance;
        gearMonoBehaviour.combatManager = GameObject.FindGameObjectWithTag("CombatManager").GetComponent<CombatManager>();

        foreach (StatModifier statModifier in gearInstance.gearSO.StatModifiers)
        {
            ChangeStat(statModifier);
        }

        gearBehaviours[i] = gearMonoBehaviour;
    }

    void ClearAllGearBehaviours()
    {
        gearBehaviours.Clear();

        foreach (Transform child in gearMonoBehaviourParentFolder.transform)
        {
            Destroy(child.gameObject);
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
                Debug.Log(FocusBase);
                break;
        }

        static int Apply(int value, StatModifier mod)
        {
            if (mod.modifierType == ModifierType.Flat)
                return value + Mathf.RoundToInt(mod.amount);

            return Mathf.RoundToInt(value + (value * mod.amount));
        }
    }

}
