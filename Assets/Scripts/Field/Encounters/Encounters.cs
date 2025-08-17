using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Encounters : MonoBehaviour
{
    [SerializeField] EnemyProfile[] EnemyRoster;
    [SerializeField] AllyProfile[] AllyRoster;
    public int maxTotalEnemies = 1;
    public int maxBonusAllies = 0;
    public Battle nextBattle;
    PlayerCombat playerCombat;

    private void OnEnable()
    {
        playerCombat = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>();
        FieldEvents.StartScene += StartScene;
    }

    private void OnDisable()
    {
        FieldEvents.StartScene -= StartScene;
    }

    void StartScene()
    {
        StartCoroutine(StartSceneCoRo());
    }

    IEnumerator StartSceneCoRo()
    {

        InitialiseBattle();
        StandardBattleLayout();
        nextBattle.playerFightingPosition = SetPlayerFightingPosition();
        //yield return SurpriseAttack();

        yield return null;
        SpawnBattle();
    }

    public void InitialiseBattle()
    {
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

    void StandardBattleLayout()
    {
        this.transform.position = Vector3.zero;
        Vector2 battleCenter = this.transform.position;

        float verticalSpacingEnemies = 0.2f;
        int enemyCount = nextBattle.enemies.Count;
        float totalHeightEnemies = (enemyCount - 1) * verticalSpacingEnemies;
        float startYEnemies = battleCenter.y - totalHeightEnemies / 1.5f;

        int[] enemyIndices = new int[enemyCount];
        for (int i = 0; i < enemyCount; i++)
        {
            enemyIndices[i] = +i;
        }
        enemyIndices.Shuffle();

        float xSpacingEnemies = 0.2f;
        float startXEnemies = battleCenter.x + 0.7f;

        for (int i = 0; i < enemyCount; i++)
        {
            Combatant enemy = nextBattle.enemies[i];
            float yPos = startYEnemies + i * verticalSpacingEnemies;

            float xPos = startXEnemies + enemyIndices[i] * xSpacingEnemies;
            Vector2 fightingPos = new Vector2(xPos, yPos);

            enemy.fightingPosition = new GameObject(enemy.combatantName + " FightingPosition");
            enemy.fightingPosition.transform.SetParent(this.transform);
            enemy.fightingPosition.transform.position = fightingPos;

            enemy.transform.position = new Vector2(fightingPos.x + 2, fightingPos.y);

            var movementScript = enemy.movementScript;
            movementScript.forceLookDirectionOnLoad = Vector2.zero;
            movementScript.lookDirection = Vector2.left;
            movementScript.movementSpeed = movementScript.defaultMovementspeed * 3;
        }

        List<Combatant> allAllies = new List<Combatant>(nextBattle.allies);
        allAllies.Insert(Random.Range(0, allAllies.Count + 1), playerCombat);

        float verticalSpacingAlly = 0.2f;
        int allyCount = allAllies.Count;
        float totalHeightAlly = (allyCount - 1) * verticalSpacingAlly;
        float startYAlly = battleCenter.y - totalHeightAlly / 1.5f;

        int[] allyIndices = new int[allyCount];
        for (int i = 0; i < allyCount; i++)
        {
            allyIndices[i] = -i;
        }
        allyIndices.Shuffle();

        float xSpacingAlly = 0.2f;
        float startXAlly = battleCenter.x - (allyCount - 1) * xSpacingAlly / 2f;

        for (int i = 0; i < allyCount; i++)
        {
            Combatant allyOrPlayer = allAllies[i];
            float yPos = startYAlly + i * verticalSpacingAlly;

            float xPos = startXAlly + allyIndices[i] * xSpacingAlly;
            Vector2 fightingPos = new Vector2(xPos, yPos);

            allyOrPlayer.fightingPosition = new GameObject(allyOrPlayer.combatantName + " FightingPosition");
            allyOrPlayer.fightingPosition.transform.SetParent(this.transform);
            allyOrPlayer.fightingPosition.transform.position = fightingPos;

            allyOrPlayer.transform.position = fightingPos;

            var movementScript = allyOrPlayer.movementScript;
            movementScript.forceLookDirectionOnLoad = Vector2.zero;
            movementScript.lookDirection = Vector2.right;
        }
        nextBattle.playerDefaultLookDirection = Vector2.right;
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
