using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CrossSceneReferences
{
   public  static Dictionary<int, ToTrigger> triggerReferences = new Dictionary<int, ToTrigger>();

    public static void Clear()
    {
        triggerReferences.Clear();
    }
}


