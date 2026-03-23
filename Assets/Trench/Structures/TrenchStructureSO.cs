using UnityEngine;

[CreateAssetMenu]

public class TrenchStructureSO : ScriptableObject
{
    [SerializeField] private string structureName;
    [SerializeField] private int structurePrice;
    [SerializeField] private int structureHP;
    [SerializeField] private int structureArmor;
    [SerializeField] private Sprite structureSprite;
    [SerializeField] private string structureDescription;
    [SerializeField] private string type;
    [SerializeField] private GameObject spawnPrefab;

    [HideInInspector] public string StructureName => structureName;
    [HideInInspector] public int StructurePrice => structurePrice;
    [HideInInspector] public int StructureHP => structureHP;
    [HideInInspector] public int StructureArmor => structureArmor;
    [HideInInspector] public Sprite StructureSprite => structureSprite;
    [HideInInspector] public string StructureDescription => structureDescription;
    [HideInInspector] public string Type => type;
    [HideInInspector] public GameObject SpawnPrefab => spawnPrefab;
}
