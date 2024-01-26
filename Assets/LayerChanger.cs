using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerChanger : MonoBehaviour
{

    public SpriteRenderer[] spritesLayersToChange;
    public string layerNameToChangeTo;
    public string tagNameToTrigger;



    void OnTriggerEnter2D(Collider2D colliderToTrigger)

    {
        if (colliderToTrigger.tag == tagNameToTrigger)

        {

            foreach (SpriteRenderer i in spritesLayersToChange)

                i.sortingLayerName = layerNameToChangeTo;
        }
    }


}
