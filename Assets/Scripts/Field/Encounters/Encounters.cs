using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encounters : MonoBehaviour
{
    [SerializeField] EnemyProfile[] EnemyRoster;
    [SerializeField] AllyProfile[] AllyRoster;
    public int numberOfEnemies = 1;
    public Battle nextBattle;

    void Start()
    {
        InitialiseBattle();
        SpawnBattle();
    }

    public void InitialiseBattle()
    {
        nextBattle.enemies.Clear();

        int randomNumberOfEnemies = Random.Range(1, numberOfEnemies);

        for (int i = 0; i < randomNumberOfEnemies; i++)
        {
            int randomIndex = Random.Range(0, EnemyRoster.Length);
            if (EnemyRoster[randomIndex].maxNumber > 0)
            {
                GameObject enemyToAdd = Instantiate(EnemyRoster[randomIndex].enemyPreFab);
                Combatant combatantToAdd = enemyToAdd.GetComponent<Combatant>();
                nextBattle.enemies.Add(combatantToAdd);
                EnemyRoster[randomIndex].maxNumber--;
            }
        }

        nextBattle.playerFightingPosition = new GameObject();
        nextBattle.playerFightingPosition.transform.position = Vector2.zero;
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
        public int maxNumber;
    }

    [System.Serializable]
    public class AllyProfile
    {
        public GameObject allyPreFab;
        public int maxNumber;
    }
}
