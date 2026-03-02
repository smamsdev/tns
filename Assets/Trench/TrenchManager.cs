using UnityEngine;
using System.Collections.Generic;
using System;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using TMPro;

public class TrenchManager : MonoBehaviour
{
    public enum SpawnPosition {Left, Right};

    public GameObject meleePrefab, playerBase, enemyBase, rangedPrefab, emptyStructurePrefab;
    int playerArmyCount = 1;
    int enemyArmyCount = 1;
    public List<TrenchFrontLine> frontLines;
    public SpawnPosition playerSide = SpawnPosition.Left;
    public int frontLineInPlay;
    public List<TrenchStructure> activeStructures;

    [Header ("Timer")]
    public TextMeshProUGUI durationTMP;
    public float spawnInterval = 5;
    public float totalGameTime;
    public float intervalTimer;
    int lastDisplayedSecond = -1;

    void Update()
    {
        totalGameTime += Time.deltaTime;
        intervalTimer += Time.deltaTime;

        if (intervalTimer >= spawnInterval)
        {
            intervalTimer = 0;
            Debug.Log("trigger");
        }

        // Update clock display once per second
        int currentSecond = (int)totalGameTime;
        if (currentSecond != lastDisplayedSecond)
        {
            lastDisplayedSecond = currentSecond;

            TimeSpan t = TimeSpan.FromSeconds(currentSecond);
            durationTMP.text = t.ToString(@"mm\:ss");
        }
    }

    private void Start()
    {
        FieldEvents.movementLocked = true;

        playerSide = SpawnPosition.Left;
        frontLineInPlay = 0;

        foreach (TrenchFrontLine trenchFrontLine in frontLines)
        {
            trenchFrontLine.InstantiateConstructSlots(trenchFrontLine.leftStructures, emptyStructurePrefab, trenchFrontLine.leftBaseGO);
            trenchFrontLine.InstantiateConstructSlots(trenchFrontLine.rightStructures, emptyStructurePrefab, trenchFrontLine.rightBaseGO);
        }
    }

    public TrenchStructureIcon[] currentBase()
    {
        if (playerSide == SpawnPosition.Left)
            return frontLines[frontLineInPlay].leftStructures;

        return frontLines[frontLineInPlay].rightStructures;
    }

    void SpawnPlayerArmy(GameObject prefab)
    {
        GameObject newPawnGO = Instantiate(prefab, playerBase.transform);
        newPawnGO.name = "Player" + prefab.name + "" + playerArmyCount;
        playerArmyCount++;
        Pawn pawn = newPawnGO.GetComponent<Pawn>();
        pawn.team = Pawn.Team.Player;
        pawn.targetFortress = enemyBase;
        pawn.EnterState(pawn.advancing);
    }

    void SpawnEnemyArmy(GameObject prefab)
    {
        GameObject newPawnGO = Instantiate(prefab, enemyBase.transform);
        newPawnGO.name = "Enemy" + prefab.name + "" + enemyArmyCount;
        enemyArmyCount++;
        Pawn pawn = newPawnGO.GetComponent<Pawn>();
        pawn.team = Pawn.Team.Enemy;
        pawn.targetFortress = playerBase;
        pawn.EnterState(pawn.advancing);
    }
}
