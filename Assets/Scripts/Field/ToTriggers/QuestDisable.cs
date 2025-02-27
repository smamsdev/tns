using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestDisable : ToTrigger
{
    public string questNameChange;
    public QuestStatusSO QuestStatus;

    public override IEnumerator DoAction()
    {
        foreach (Quest quest in QuestStatus.Quests)
        {
            if (quest.questName == questNameChange)
            {
                quest.completed = false;
                FieldEvents.HasCompleted.Invoke(this.gameObject);
                yield return null;
            }
        }
    }
}
