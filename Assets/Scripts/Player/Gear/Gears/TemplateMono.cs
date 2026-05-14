using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Template : GearMonoBehaviour
{
    public override IEnumerator ApplyGearEffect()
    {
        yield return null;
    }

    public override void OnEquipGear()
    {
        return;
    }
}
