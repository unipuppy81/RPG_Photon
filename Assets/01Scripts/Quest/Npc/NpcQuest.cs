using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcQuest : MonoBehaviour
{
    [SerializeField]
    private Quest npcQuest;

    public void QuestSender()
    {
        GameObject obj = GameObject.Find("Quest Giver");
        if (obj != null)
        {
            QuestGiver questGiver = obj.GetComponent<QuestGiver>();
            if (questGiver != null)
            {
                questGiver.AddQuest(npcQuest);
                Debug.Log("����Ʈ�� ���������� �߰��Ǿ����ϴ�.");
            }
            else
            {
                Debug.LogWarning("QuestGiver ������Ʈ�� ã�� �� �����ϴ�.");
            }
        }
        else
        {
            Debug.LogWarning("Quest Giver ������Ʈ�� ã�� ���߽��ϴ�.");
        }
    }

    public void QuestSender(Quest sender)
    {
        GameObject obj = GameObject.Find("Quest Giver");
        if (obj != null)
        {
            QuestGiver questGiver = obj.GetComponent<QuestGiver>();
            if (questGiver != null)
            {
                questGiver.AddQuest(sender);
                Debug.Log("����Ʈ�� ���������� �߰��Ǿ����ϴ�.");
            }
            else
            {
                Debug.LogWarning("QuestGiver ������Ʈ�� ã�� �� �����ϴ�.");
            }
        }
        else
        {
            Debug.LogWarning("Quest Giver ������Ʈ�� ã�� ���߽��ϴ�.");
        }
    }
}
