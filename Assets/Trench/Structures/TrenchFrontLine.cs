using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrenchFrontLine : MonoBehaviour
{
    public TrenchStructureInstance[] leftStructures = new TrenchStructureInstance[16];
    public TrenchStructureInstance[] rightStructures = new TrenchStructureInstance[16];
    public GameObject leftBaseGO, rightBaseGO;

    public void InstantiateConstructSlots(TrenchManager.Team team, TrenchStructureInstance[] structures, GameObject emptyPrefab, GameObject baseParent)
    {
        GridLayoutGroup gridLayoutGroup = baseParent.GetComponent<GridLayoutGroup>();

        if (team == TrenchManager.Team.Left)
        {
            gridLayoutGroup.startCorner = GridLayoutGroup.Corner.UpperRight;
            gridLayoutGroup.childAlignment = TextAnchor.UpperRight;
        }

        else
        {
            gridLayoutGroup.startCorner = GridLayoutGroup.Corner.UpperLeft;
            gridLayoutGroup.childAlignment = TextAnchor.UpperLeft;
        }


        int i = 0;

        foreach (TrenchStructureInstance structure in structures)
        {

            GameObject constructSlotGO = Instantiate(emptyPrefab, baseParent.transform);
            constructSlotGO.name = "Empty" + "" + i; 
            structures[i] = constructSlotGO.GetComponent<TrenchStructureInstance>();
            i++;
        }
    }
}
