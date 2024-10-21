using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSwap : ToTrigger
{
    public SpriteRenderer spriteRenderer;
    public Sprite newSprite;
    public float delay;

    public override IEnumerator DoAction()
    {
        yield return new WaitForSeconds(delay);
        spriteRenderer.sprite = newSprite;
        FieldEvents.HasCompleted.Invoke(this.gameObject);
        yield return null;
    }
}
