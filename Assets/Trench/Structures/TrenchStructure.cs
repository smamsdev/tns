using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]

public class TrenchStructure : ScriptableObject
{
    public string structureName;
    public int structurePrice;
    public int structureHP;
    public int structureArmor;
    public Sprite structureSprite;
    public string structureDescription;
    public GameObject structureGOSpawn;
}
