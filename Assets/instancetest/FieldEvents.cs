using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public static class FieldEvents
{

    public static Action<string, int, bool> DialogueEvent;
    public static Action<string, bool> ActorActionHasStarted;
    public static Action<string, bool> ActorActionHasCompleted;
    public static Action<GameObject> HasBeenDefeated;



}
