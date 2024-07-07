using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalQuestServer : MonoBehaviour
{
    Dictionary<int, bool> questDic = new Dictionary<int, bool>();

    private void Awake()
    {
        for (int i = 10001; i < 10010; i++)
            questDic.Add(i, true);
    }

    public void StartQuest(UserData userData, int questId)
    {
        // 없거나 비활성화된 퀘스트
        if (!questDic.ContainsKey(questId) || !questDic[questId])
            return;

        // 해당 유저가 이미 진행중이거나 완료한 퀘스트
        if (userData.questState.ContainsKey(questId))
            return;

        userData.questState.Add(questId, QuestState.Progress);
    }

    public void CompleteQuest(UserData userData, int questId)
    {
        if (CheckQuestComplete(userData, questId))
        {
            // TODO
            userData.questState[questId] = QuestState.Done;
        }
    }

    private bool CheckQuestComplete(UserData userData, int questId)
    {
        if (userData.questState.ContainsKey(questId))
        {
            if (userData.questState[questId] == QuestState.Progress)
            {
                // TODO
                return true;
            }
        }

        return false;
    }
}
