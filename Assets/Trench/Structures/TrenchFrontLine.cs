using System.Collections.Generic;
using UnityEngine;

public class TrenchFrontLine : MonoBehaviour
{
    public TrenchStructureIcon[] leftStructures = new TrenchStructureIcon[16];
    public TrenchStructureIcon[] rightStructures = new TrenchStructureIcon[16];
    public GameObject leftBaseGO, rightBaseGO;

    public void InstantiateConstructSlots(TrenchStructureIcon[] structures, GameObject emptyPrefab, GameObject baseParent)
    {
        int i = 0;

        foreach (TrenchStructureIcon structure in structures)
        {

            GameObject constructSlotGO = Instantiate(emptyPrefab, baseParent.transform);
            constructSlotGO.name = "Empty" + "" + i; 
            structures[i] = constructSlotGO.GetComponent<TrenchStructureIcon>();
            i++;
        }
    }
}
