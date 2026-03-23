using UnityEngine;
using System.Collections.Generic;
using System;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using TMPro;

public class TrenchManager : MonoBehaviour
{
    public enum Team {Left, Right }

    public GameObject meleePrefab, rangedPrefab, emptyStructurePrefab;
    public List<TrenchFrontLine> frontLines;
    public List<TrenchStructureInstance> activeStructures;

    [Header ("Timer")]
    public TextMeshProUGUI durationTMP;
    public float spawnInterval = 5;
    public float totalGameTime;
    public float intervalTimer;
    int lastDisplayedSecond = -1;

    public TrenchStructureSO testPrefab;
    int i = 0;

    void Update()
    {
        totalGameTime += Time.deltaTime;
        intervalTimer += Time.deltaTime;

        if (intervalTimer >= spawnInterval)
        {
            intervalTimer = 0;
            SpawnAll();
        }

        // Update clock display once per second
        int currentSecond = (int)totalGameTime;
        if (currentSecond != lastDisplayedSecond)
        {
            lastDisplayedSecond = currentSecond;

            TimeSpan t = TimeSpan.FromSeconds(currentSecond);
            durationTMP.text = t.ToString(@"mm\:ss");
        }

        if (Input.GetKeyDown(KeyCode.Tab)) 
            SpawnEnemyStructure();
    }

    void SpawnAll()
    {
        foreach (TrenchStructureInstance trenchStructureInstance in activeStructures)
        {
            trenchStructureInstance.SpawnUnit();
        }
    }

    public void ConstructStructure(Team team, int frontLineIndex, TrenchStructureInstance newStructureInstance, TrenchStructureSO structureSOToConstruct)
    {
        newStructureInstance.structureSO = structureSOToConstruct;
        newStructureInstance.team = team;
        newStructureInstance.targetTag = GetTag();
        newStructureInstance.spriteRenderer.sprite = newStructureInstance.structureSO.StructureSprite;
        newStructureInstance.spriteRenderer.enabled = true;

        activeStructures.Add(newStructureInstance);
    }

    public TrenchStructureInstance[] GetBaseStructureList(Team team, int frontLineIndex)
    {
        if (team == Team.Left)
            return frontLines[frontLineIndex].leftStructures;

        return frontLines[frontLineIndex].rightStructures;
    }

    void SpawnEnemyStructure()
    {
        ConstructStructure(Team.Right, 0, frontLines[0].rightStructures[i], testPrefab);
        i++;
    }

    public String GetTag()
    {
        Debug.Log("todo");
        string tag = "";
        return tag;
    }

    private void Start()
    {
        FieldEvents.movementLocked = true;

        foreach (TrenchFrontLine trenchFrontLine in frontLines)
        {
            trenchFrontLine.InstantiateConstructSlots(Team.Left, trenchFrontLine.leftStructures, emptyStructurePrefab, trenchFrontLine.leftBaseGO);
            trenchFrontLine.InstantiateConstructSlots(Team.Right, trenchFrontLine.rightStructures, emptyStructurePrefab, trenchFrontLine.rightBaseGO);
        }
    }
}
