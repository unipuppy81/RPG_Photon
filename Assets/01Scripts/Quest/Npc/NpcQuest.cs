using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcQuest : MonoBehaviour
{
    [SerializeField]
    private List<Quest> npcQuest;
    public void QuestSender()
    {
        GameObject obj = GameObject.Find("Quest Giver");
        if (obj != null)
        {
            QuestGiver questGiver = obj.GetComponent<QuestGiver>();

            Quest quest = popFirstQuest();
            if (quest != null)
            {
                questGiver.AddQuest(quest);
                Debug.Log("����Ʈ�� ���������� �߰��Ǿ����ϴ�.");
            }
            else
            {
                Debug.LogWarning("�߰��� ����Ʈ�� �����ϴ�.");
            }
        }
        else
        {
            Debug.LogWarning("Quest Giver ������Ʈ�� ã�� ���߽��ϴ�.");
        }
    }

    public Quest popFirstQuest()
    {
        if (npcQuest != null && npcQuest.Count > 0)
        {
            Quest firstQuest = npcQuest[0];
            npcQuest.RemoveAt(0);
            return firstQuest;
        }
        return null; // ����Ʈ�� ����ְų� null�� ���
    }
}
