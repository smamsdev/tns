using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Encounter : ToTrigger
{
    [SerializeField] EnemyProfile[] EnemyRoster;
    [SerializeField] AllyProfile[] AllyRoster;
    public int maxTotalEnemies = 1;
    public int maxBonusAllies = 0;
    public Battle battle;
    PlayerCombat playerCombat;
    public SceneSetup sceneSetup;
    [SerializeField] int surpriseAttackPer100;
    [SerializeField] int flankBattlePer100;

    private void OnEnable()
    {
        playerCombat = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>();
    }

    public override IEnumerator DoAction()
    {
        InitialiseBattle();
        SelectLayoutType();

        battle.playerFightingPosition = SetPlayerFightingPosition();

        SpawnBattle();
        yield return new WaitForSeconds(0.2f);
        sceneSetup.FadeUp();
        yield return null;
    }

    void SelectLayoutType()
    {
        int layoutTypeWeightingTotal = 100 + flankBattlePer100 + surpriseAttackPer100;
        int randomValue = Random.Range(1, layoutTypeWeightingTotal + 1);

        if (randomValue <= flankBattlePer100)
        {
            FlankBattleLayout();
            return;
        }

        if (randomValue <= flankBattlePer100 + surpriseAttackPer100)
        {
            SurpriseBattleLayout();
            return;
        }

        else
        {
            StandardBattleLayout();
            return;
        }
    }

    public void InitialiseBattle()
    {
        this.transform.position = Vector3.zero;
        battle.battleCenterPosition = this.transform;
        battle.enemies.Clear();
        battle.allies.Clear();

        foreach (var ally in AllyRoster)
        {
            ally.remainingThisBattle = ally.maxNumberOfType;
            ally.amountSpawned = 0;
        }

        for (int i = 0; i < playerCombat.party.partyMembers.Count; i++)
        { 
            GameObject allyToAdd = Instantiate(playerCombat.party.partyMembers[i].prefab,this.transform);
            Combatant allyCombatant = allyToAdd.GetComponent<Combatant>();
            battle.allies.Add(allyCombatant);

            allyToAdd.name = allyCombatant.combatantName;
            allyToAdd.transform.position = new Vector2(this.transform.position.x + 5, this.transform.position.y + 1000 + i);
        }

        int randomNumberOfAllies = Random.Range(0, maxBonusAllies + 1);
        int totalAllies = randomNumberOfAllies + playerCombat.party.partyMembers.Count;

        for (int i = 0; i < randomNumberOfAllies; i++)
        {
            int randomIndex = Random.Range(0, AllyRoster.Length);
            AllyProfile bonusAlly = AllyRoster[randomIndex];

            if (bonusAlly.remainingThisBattle > 0)
            {
                GameObject bonusAllyToAdd = Instantiate(bonusAlly.allyPreFab, this.transform);
                Combatant bonusAllyCombatant = bonusAllyToAdd.GetComponent<Combatant>();

                bonusAllyToAdd.name = bonusAllyCombatant.combatantName;
                bonusAllyToAdd.transform.position = new Vector2(this.transform.position.x + 6, this.transform.position.y + 1000 + i);

                bonusAlly.amountSpawned++;
                if (bonusAlly.amountSpawned > 1)
                {
                    bonusAllyCombatant.combatantName += " " + bonusAlly.amountSpawned;
                    bonusAllyToAdd.gameObject.name = bonusAllyCombatant.combatantName;
                }

                battle.allies.Add(bonusAllyCombatant);
                bonusAlly.remainingThisBattle--;
            }
        }

        battle.allies.Shuffle();
        NumberDuplicates(battle.allies);


        void NumberDuplicates(List<Combatant> allies)
        {
            Dictionary<string, int> nameCounts = new Dictionary<string, int>();

            foreach (var ally in allies)
            {
                string baseName = ally.combatantName;

                if (!nameCounts.ContainsKey(baseName))
                { 
                    nameCounts[baseName] = 0; 
                }

                nameCounts[baseName]++;

                // only append a number if there's more than one of this type
                if (nameCounts[baseName] > 1)
                {
                    ally.combatantName = baseName + " " + nameCounts[baseName];
                    ally.gameObject.name = ally.combatantName; // update GameObject name too
                }
            }
        }

        foreach (var enemy in EnemyRoster)
        {
            enemy.remainingThisBattle = enemy.maxNumberOfType;
            enemy.amountSpawned = 0;
        }

        int randomNumberOfEnemies = Random.Range(1, maxTotalEnemies + 1);

        for (int i = 0; i < randomNumberOfEnemies; i++)
        {
            int randomIndex = Random.Range(0, EnemyRoster.Length);
            EnemyProfile enemy = EnemyRoster[randomIndex];

            if (enemy.remainingThisBattle > 0)
            {
                GameObject enemyToAdd = Instantiate(enemy.enemyPreFab, this.transform);
                Combatant enemyCombatant = enemyToAdd.GetComponent<Combatant>();
                enemyCombatant.GetComponent<ActorMovementScript>().rigidBody2d.bodyType = RigidbodyType2D.Kinematic;

                enemyToAdd.name = enemyCombatant.combatantName;
                enemyToAdd.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + 1000 + i);

                enemy.amountSpawned++;
                if (enemy.amountSpawned > 1)
                {
                    enemyCombatant.combatantName += " " + enemy.amountSpawned;
                    enemyCombatant.gameObject.name = enemyCombatant.combatantName;
                }

                battle.enemies.Add(enemyCombatant);
                enemy.remainingThisBattle--;
            }
        }
    }

    void ReverseBattleLayout()
    {
        List<Combatant> allAllies = new List<Combatant>(battle.allies);
        allAllies.Insert(Random.Range(0, allAllies.Count + 1), playerCombat);

        // Allies on left, enemies on right
        SpaceCombatants(allAllies, false, 0, Vector2.left);
        SpaceCombatants(battle.enemies, true, -2.5f, Vector2.right);

        battle.playerDefaultLookDirection = Vector2.left;
    }

    void StandardBattleLayout()
    {
        List<Combatant> allAllies = new List<Combatant>(battle.allies);
        allAllies.Insert(Random.Range(0, allAllies.Count + 1), playerCombat);

        // Allies on left, enemies on right
        SpaceCombatants(allAllies, true, 0, Vector2.right);
        SpaceCombatants(battle.enemies, false, 0, Vector2.left);

        battle.playerDefaultLookDirection = Vector2.right;
    }

    void SpaceCombatants(List<Combatant> combatantList, bool isLeftSided, float startPosOffset, Vector2 lookDir)
    {
        Vector2 battleCenter = battle.battleCenterPosition.transform.position;
        int combatantCount = combatantList.Count;

        //these numbers are mostly eyeballed
        float minSpacingY = 0.1f;
        float maxSpacingY = 0.35f;
        float adjustedSpacingY = Mathf.Clamp(maxSpacingY - (combatantCount - 1) * 0.025f, minSpacingY, maxSpacingY);
        float totalHeightCombatants = (combatantCount - 1) * -adjustedSpacingY;
        float startYCombatants = battleCenter.y - totalHeightCombatants / 2f;

        float minSpacingX = 0.1f;
        float maxSpacingX = 0.35f;
        float adjustedSpacingX = Mathf.Clamp(maxSpacingX - (combatantCount - 1) * 0.02f, minSpacingX, maxSpacingX);

        int sideMultiplier = isLeftSided ? -1 : 1;
        float startXCombatants = battleCenter.x + (0.4f * sideMultiplier);

        int[] combatantIndices = new int[combatantCount];
        for (int i = 0; i < combatantCount; i++)
        {
            combatantIndices[i] = i * sideMultiplier;
        }
        combatantIndices.Shuffle();

        for (int i = 0; i < combatantCount; i++)
        {
            Combatant combatant = combatantList[i];
            float yPos = startYCombatants + i * -adjustedSpacingY;
            float xPos = startXCombatants + combatantIndices[i] * adjustedSpacingX;

            Vector2 fightingPos = new Vector2(xPos, yPos);

            combatant.fightingPosition = new GameObject(combatant.combatantName + " FightingPosition");
            combatant.fightingPosition.transform.SetParent(this.transform);
            combatant.fightingPosition.transform.position = fightingPos;

            if (startPosOffset != 0)
            {
                combatant.transform.position = new Vector2(startPosOffset, fightingPos.y);
            }
            else
            {
                combatant.transform.position = new Vector2(fightingPos.x + startPosOffset, fightingPos.y);
            }

            var movementScript = combatant.movementScript;
            movementScript.forceLookDirectionOnLoad = Vector2.zero;
            movementScript.lookDirection = lookDir;
            movementScript.movementSpeed = movementScript.defaultMovementspeed * 3;
        }
    }

    float CalculateSpacing(int combatantCount, float minSpacing, float maxSpacing)
    {
        return Mathf.Clamp(maxSpacing - (combatantCount - 1) * 0.02f, minSpacing, maxSpacing);
    }

    GameObject SetPlayerFightingPosition()
    {
        battle.playerFightingPosition = playerCombat.fightingPosition;
        return playerCombat.fightingPosition;
    }

    void FlankBattleLayout()
    {
        List<Combatant> allAllies = new List<Combatant>(battle.allies);
        allAllies.Insert(Random.Range(0, allAllies.Count + 1), playerCombat);

        // Allies on left, enemies on right
        SpaceCombatants(allAllies, true, -2.5f, Vector2.right);
        SpaceCombatants(battle.enemies, false, 0, Vector2.right);

        battle.playerDefaultLookDirection = Vector2.right;
        battle.isEnemyFlanked = true;
    }

    void SurpriseBattleLayout()
    {
        List<Combatant> allAllies = new List<Combatant>(battle.allies);
        allAllies.Insert(Random.Range(0, allAllies.Count + 1), playerCombat);

        // Allies on right, enemies on left
        SpaceCombatants(allAllies, false, 0, Vector2.right);
        SpaceCombatants(battle.enemies, true, -2.5f, Vector2.right);

        battle.playerDefaultLookDirection = Vector2.right;
        battle.isAllyFlanked = true;
    }

    public void SpawnBattle()
    {
        battle.combatManager.battleScheme = battle;
        battle.combatManager.StartBattle();
    }

    [System.Serializable]
    public class EnemyProfile
    {
        public GameObject enemyPreFab;
        public int maxNumberOfType;
        [HideInInspector]public int remainingThisBattle;
        [HideInInspector]public int amountSpawned = 0;
    }

    [System.Serializable]
    public class AllyProfile
    {
        public GameObject allyPreFab;
        public int maxNumberOfType;
        [HideInInspector]public int remainingThisBattle;
        [HideInInspector]public int amountSpawned = 0;
    }
}
