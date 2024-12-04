using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestStatus")]
public class QuestStatusSO : ScriptableObject
{
    public Quest[] Quests;
}