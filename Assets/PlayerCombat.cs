using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;
using static StatModifier;
using static UnityEngine.Rendering.DebugUI;

public class PlayerCombat : PartyMemberCombat
{
    public PlayerMoveManager playerMoveManager;
    public PlayerPermanentStats playerPermanentStats;
    public PartySO party;
    public PlayerInventorySO playerInventorySO;
    public GameObject gearMonoBehaviourParentFolder;
    public List<GearMonoBehaviour> gearBehaviours = new();

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
        InstantiateAllEquippedGearBehaviours(GameObject.FindGameObjectWithTag("CombatManager").GetComponent<CombatManager>());
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

    public override void SelectMove()
    {
        throw new System.NotImplementedException();
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

    public void InstantiateAllEquippedGearBehaviours(CombatManager combatManager)
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
                ChangeStat(statModifier);
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
