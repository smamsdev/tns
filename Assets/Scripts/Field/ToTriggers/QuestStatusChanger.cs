using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestStatusChanger : ToTrigger
{
    public string questNameChange;
    public QuestStatusSO QuestStatus;

    public override IEnumerator DoAction()
    {
        foreach (Quest quest in QuestStatus.Quests)
        {
            if (quest.questName == questNameChange)
            {
                quest.completed = true;
                FieldEvents.HasCompleted.Invoke(this.gameObject);
                yield return null;
            }
        }
    }
}
