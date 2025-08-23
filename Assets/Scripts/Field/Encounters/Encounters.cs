using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Encounters : ToTrigger
{
    [SerializeField] EnemyProfile[] EnemyRoster;
    [SerializeField] AllyProfile[] AllyRoster;
    public int maxTotalEnemies = 1;
    public int maxBonusAllies = 0;
    public Battle nextBattle;
    PlayerCombat playerCombat;
    public SceneSetup sceneSetup;

    private void OnEnable()
    {
        playerCombat = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>();
    }

    public override IEnumerator DoAction()
    {
        InitialiseBattle();
        StandardBattleLayout();
        //ReverseBattleLayout();
        nextBattle.playerFightingPosition = SetPlayerFightingPosition();
        //yield return SurpriseAttack();


        SpawnBattle();
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(sceneSetup.FadeUpAndUnlock());
        yield return null;
    }

    public void InitialiseBattle()
    {
        this.transform.position = Vector3.zero;
        nextBattle.battleCenterPosition = this.transform;
        nextBattle.enemies.Clear();
        nextBattle.allies.Clear();

        foreach (var ally in AllyRoster)
        {
            ally.remainingThisBattle = ally.maxNumberOfType;
            ally.amountSpawned = 0;
        }

        for (int i = 0; i < playerCombat.party.partyMembers.Count; i++)
        { 
            GameObject allyToAdd = Instantiate(playerCombat.party.partyMembers[i].prefab,this.transform);
            Combatant allyCombatant = allyToAdd.GetComponent<Combatant>();
            nextBattle.allies.Add(allyCombatant);

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

                nextBattle.allies.Add(bonusAllyCombatant);
                bonusAlly.remainingThisBattle--;
            }
        }

        nextBattle.allies.Shuffle();
        NumberDuplicates(nextBattle.allies);


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

                nextBattle.enemies.Add(enemyCombatant);
                enemy.remainingThisBattle--;
            }
        }
    }

    void ReverseBattleLayout()
    {
        List<Combatant> allAllies = new List<Combatant>(nextBattle.allies);
        allAllies.Insert(Random.Range(0, allAllies.Count + 1), playerCombat);

        // Allies on left, enemies on right
        SpaceCombatants(allAllies, false, 0, Vector2.left);
        SpaceCombatants(nextBattle.enemies, true, -2.5f, Vector2.right);

        nextBattle.playerDefaultLookDirection = Vector2.left;
    }

    void StandardBattleLayout()
    {
        List<Combatant> allAllies = new List<Combatant>(nextBattle.allies);
        allAllies.Insert(Random.Range(0, allAllies.Count + 1), playerCombat);

        // Allies on left, enemies on right
        SpaceCombatants(allAllies, true, 0, Vector2.right);
        SpaceCombatants(nextBattle.enemies, false, 2.5f, Vector2.left);

        nextBattle.playerDefaultLookDirection = Vector2.right;
    }

    void SpaceCombatants(List<Combatant> combatantList, bool isLeftSided, float startPosOffset, Vector2 lookDir)
    {
        Vector2 battleCenter = nextBattle.battleCenterPosition.transform.position;
        int combatantCount = combatantList.Count;

        //these numbers are mostly eyeballed
        float minSpacingY = 0.1f;
        float maxSpacingY = 0.35f;
        float adjustedSpacingY = Mathf.Clamp(maxSpacingY - (combatantCount - 1) * 0.025f, minSpacingY, maxSpacingY);
        float totalHeightCombatants = (combatantCount - 1) * adjustedSpacingY;
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
            float yPos = startYCombatants + i * adjustedSpacingY;
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
            movementScript.movementSpeed = movementScript.defaultMovementspeed * 4;
        }
    }

    float CalculateSpacing(int combatantCount, float minSpacing, float maxSpacing)
    {
        return Mathf.Clamp(maxSpacing - (combatantCount - 1) * 0.02f, minSpacing, maxSpacing);
    }

    GameObject SetPlayerFightingPosition()
    {
        nextBattle.playerFightingPosition = playerCombat.fightingPosition;
        return playerCombat.fightingPosition;
    }

    IEnumerator SurpriseAttack()
    {
        nextBattle.playerDefaultLookDirection = Vector2.right;
        List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();

        Vector2 fpf = nextBattle.playerFightingPosition.transform.position;
        int nEnemies = nextBattle.enemies.Count;
        float verticalSpacing = 0.2f;
        float totalHeight = (nEnemies - 1) * verticalSpacing;
        float startY = fpf.y - totalHeight / 1.5f;

        for (int i = 0; i < nEnemies; i++)
        {
            Combatant enemy = nextBattle.enemies[i];

            float yPos = startY + i * verticalSpacing;
            var movementScript = enemy.movementScript as ActorMovementScript;
            movementScript.forceLookDirectionOnLoad = Vector2.zero;
            movementScript.lookDirection = Vector2.right;
            movementScript.movementSpeed = 3;

            Vector2 fightingPos = new Vector2(fpf.x - 0.9f + Random.Range(-0.5f, 0.5f), yPos);
            enemy.fightingPosition = new GameObject(enemy.combatantName + " FightingPosition");

            enemy.transform.position = new Vector2(fightingPos.x, fightingPos.y);
            enemy.fightingPosition.transform.SetParent(this.transform);
            enemy.fightingPosition.transform.position = fightingPos;

            var spriteRenderer = enemy.GetComponent<SpriteRenderer>();

            Color c = spriteRenderer.color;
            c.a = 0f;
            spriteRenderer.color = c;

            spriteRenderers.Add(spriteRenderer);
        }

        float duration = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            foreach (SpriteRenderer sprite in spriteRenderers)
            {
                Color c = sprite.color;
                c.a = Mathf.Lerp(0f, 1f, t);
                sprite.color = c;
            }

            yield return null;
        }

        yield return null;
    }

    public void SpawnBattle()
    {
        nextBattle.combatManager.battleScheme = nextBattle;
        nextBattle.combatManager.StartBattle();
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
