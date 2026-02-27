using UnityEngine;
using System.Collections.Generic;

public class TrenchManager : MonoBehaviour
{
    public GameObject meleePrefab, playerBase, enemyBase, rangedPrefab, structureSlotPrefab;
    int playerArmyCount = 1;
    int enemyArmyCount = 1;
    public List<TrenchFrontLine> frontLines;

    private void Update()//
    {
        if (Input.GetKeyDown(KeyCode.I))
            SpawnPlayerArmy(meleePrefab);

        if (Input.GetKeyDown(KeyCode.O))
            SpawnEnemyArmy(meleePrefab);

        if (Input.GetKeyDown(KeyCode.R))
            SpawnPlayerArmy(rangedPrefab);

        if (Input.GetKeyDown(KeyCode.T))
            SpawnEnemyArmy(rangedPrefab);
    }

    private void Start()
    {
        foreach (TrenchFrontLine frontLine in frontLines)
        {
            frontLine.leftBase.trenchManager = this;
            frontLine.leftBase.InitializeBase();
            frontLine.rightBase.trenchManager = this;
            frontLine.rightBase.InitializeBase();

            FieldEvents.SetGridNavigationWrapAroundHorizontal(frontLine.leftBase.structureSlotButtons, 4);
            frontLine.leftBase.structureSlots[0].menuButtonHighlighted.button.Select();
        }
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
