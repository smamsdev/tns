using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Template : GearMonoBehaviour
{
    public override IEnumerator ApplyGear()
    {
        yield return null;
    }

    public override void OnEquipGear()
    {
        return;
    }
}
