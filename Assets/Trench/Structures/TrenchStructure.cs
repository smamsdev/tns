using UnityEngine;

[CreateAssetMenu]

public class TrenchStructure : ScriptableObject
{
    public string structureName;
    public int structurePrice;
    public int structureHP;
    public int structureArmor;
    public Sprite structureSprite;
    public string structureDescription;
    public string type;
    public GameObject structureGOSpawn;
    public TrenchManager.SpawnPosition playerSide = TrenchManager.SpawnPosition.Left;
}
