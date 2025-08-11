using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encounters : MonoBehaviour
{
    [SerializeField] EnemyProfile[] EnemyRoster;
    [SerializeField] AllyProfile[] AllyRoster;
    public int maxTotalEnemies = 1;
    public Battle nextBattle;

    private void OnEnable()
    {
        FieldEvents.StartScene += StartScene;
    }

    private void OnDisable()
    {
        FieldEvents.StartScene -= StartScene;
    }

    void StartScene()
    {
        InitialiseBattle();
        EnemyLayout();
        SpawnBattle();
    }

    public void InitialiseBattle()
    {
        this.transform.position = Vector3.zero;
        nextBattle.playerFightingPosition = new GameObject("Player Fighting Position");
        nextBattle.playerFightingPosition.transform.SetParent(this.transform);
        nextBattle.playerFightingPosition.transform.position = Vector3.zero;

        nextBattle.enemies.Clear();

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
                Combatant combatant = enemyToAdd.GetComponent<Combatant>();

                combatant.gameObject.name = combatant.combatantName;

                enemy.amountSpawned++;
                if (enemy.amountSpawned > 1)
                {
                    combatant.combatantName += " " + enemy.amountSpawned;
                    combatant.gameObject.name = combatant.combatantName;
                }

                nextBattle.enemies.Add(combatant);
                enemy.remainingThisBattle--;
            }
        }
    }

    void EnemyLayout()
    {
        Vector2 fpf = nextBattle.playerFightingPosition.transform.position;
        int n = nextBattle.enemies.Count;
        float spacing = 0.2f;
        float totalHeight = (n - 1) * spacing;
        float startY = fpf.y - totalHeight / 1.5f;

        for (int i = 0; i < n; i++)
        {
            Combatant enemy = nextBattle.enemies[i];
            float yPos = startY + i * spacing;
            enemy.transform.position = new Vector2(fpf.x + 0.5f + Random.Range(0f, 0.7f), yPos);
            enemy.fightingPosition = new GameObject(enemy.combatantName + " FightingPosition");
            enemy.fightingPosition.transform.position = enemy.transform.position;
            enemy.fightingPosition.transform.SetParent(enemy.transform);
        }
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
        public int remainingThisBattle;
        public int amountSpawned = 0;
    }

    [System.Serializable]
    public class AllyProfile
    {
        public GameObject allyPreFab;
        public int maxNumber;
    }
}
