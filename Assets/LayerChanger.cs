using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerChanger : MonoBehaviour
{
    public SpriteRenderer[] spritesLayersToChange;

    public void ChangeLayer(string layerNameToChangeTo)

    {//
        foreach (SpriteRenderer i in spritesLayersToChange)
        i.sortingLayerName = layerNameToChangeTo;
    }
}
