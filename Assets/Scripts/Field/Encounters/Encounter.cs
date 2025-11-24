using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Encounter : ToTrigger
{
    [SerializeField] EnemyProfile[] EnemyRoster;
    [SerializeField] AllyProfile[] bonusAllyRoster;
    public int maxTotalEnemies = 1;
    public int maxBonusAllies = 0;
    public Battle battle;
    PlayerCombat playerCombat;
    public SceneSetup sceneSetup;
    [SerializeField] int allySurprisedBattlePer100;
    [SerializeField] int enemySurprisedBattlePer100;

    private void OnEnable()
    {
        playerCombat = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>();
    }

    public override IEnumerator TriggerFunction()
    {
        InitialiseBattle();
        SelectLayoutType();

        battle.playerFightingPosition = SetPlayerFightingPosition();

        SpawnBattle();
        yield return new WaitForSeconds(0.2f);
        yield return null;
    }

    void SelectLayoutType()
    {
        int defaultBattle = 100;
        int total = defaultBattle + enemySurprisedBattlePer100 + allySurprisedBattlePer100;
        int randomValue = Random.Range(1, total + 1);

        if (randomValue <= defaultBattle)
        {
            StandardBattleLayout();
            return;
        }

        if (randomValue <= defaultBattle + enemySurprisedBattlePer100)
        {
            EnemySurprisedBattleLayout();
            return;
        }

        if (randomValue <= total)
        {
            AllySurprisedBattleLayout();
            return;
        }
    }

    public void InitialiseBattle()
    {
        this.transform.position = Vector3.zero;
        battle.battleCenterPosition = this.transform;
        battle.isRandomEnounter = true;
        battle.enemies.Clear();
        battle.allies.Clear();

        foreach (var ally in bonusAllyRoster)
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

        if (bonusAllyRoster.Length > 0)
        {
            int randomNumberOfAllies = Random.Range(0, maxBonusAllies + 1);
            int totalAllies = randomNumberOfAllies + playerCombat.party.partyMembers.Count;

            for (int i = 0; i < randomNumberOfAllies; i++)
            {
                int randomIndex = Random.Range(0, bonusAllyRoster.Length);
                AllyProfile bonusAlly = bonusAllyRoster[randomIndex];

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

    void StandardBattleLayout()
    {
        List<Combatant> allAllies = new List<Combatant>(battle.allies);
        allAllies.Insert(Random.Range(0, allAllies.Count + 1), playerCombat);

        // Allies on left, enemies on right
        SpaceCombatants(allAllies, true, -2.5f, Vector2.right);
        SpaceCombatants(battle.enemies, false, 2.5f, Vector2.left);

        foreach (Combatant combatant in allAllies)
        {
            combatant.CombatLookDirX = 1;
        }

        foreach (Combatant combatant in battle.enemies)
        {
            combatant.CombatLookDirX = -1;
        }
    }

    void EnemySurprisedBattleLayout()
    {
        List<Combatant> allAllies = new List<Combatant>(battle.allies);
        allAllies.Insert(Random.Range(0, allAllies.Count + 1), playerCombat);

        // Allies on left, enemies on right
        SpaceCombatants(allAllies, true, -2.5f, Vector2.right);
        SpaceCombatants(battle.enemies, false, 0, Vector2.right);

        battle.isEnemyFlanked = true;

        foreach (Combatant combatant in allAllies)
        {
            combatant.CombatLookDirX = 1;
        }

        foreach (Combatant combatant in battle.enemies)
        {
            combatant.CombatLookDirX = 1;
        }
    }

    void AllySurprisedBattleLayout()
    {
        List<Combatant> allAllies = new List<Combatant>(battle.allies);
        allAllies.Insert(Random.Range(0, allAllies.Count + 1), playerCombat);

        // Allies on right, enemies on left
        SpaceCombatants(allAllies, false, 0, Vector2.right);
        SpaceCombatants(battle.enemies, true, -2.5f, Vector2.right);

        battle.isAllyFlanked = true;

        foreach (Combatant combatant in allAllies)
        {
            combatant.CombatLookDirX = 1;
        }

        foreach (Combatant combatant in battle.enemies)
        {
            combatant.CombatLookDirX = 1;
        }
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
