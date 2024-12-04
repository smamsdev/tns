using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestCheckOnSceneLoad : ToTrigger
{
    public string questNameToCheck;
    public QuestStatusSO QuestStatus;

    private void Start()
    {
        foreach (Quest quest in QuestStatus.Quests)
        {
            if (quest.questName == questNameToCheck)
            {
                if (quest.completed)
                {
                    StartCoroutine(DoAction());
                }
            }
        }
    }

    public override IEnumerator DoAction()
    {
        FieldEvents.HasCompleted.Invoke(this.gameObject);
        yield return null;
    }
}
