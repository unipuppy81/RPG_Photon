using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData;
    Dictionary<int, System.Action> questActions; // ȣ���� �޼��带 �����ϱ� ���� ����
    Dictionary<int, QuestReporter> questReporters; // QuestReporter ������ ���� ���� �߰�

    private void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        questActions = new Dictionary<int, System.Action>(); // ���� �ʱ�ȭ
        questReporters = new Dictionary<int, QuestReporter>(); // ���� �ʱ�ȭ

        GenerateData();
        GenerateQuestReporters();
        GenerateQuestActions();
    }
    void GenerateQuestReporters()
    {
        // �� ��ȭ ID�� �����ϴ� QuestReporter�� �����մϴ�.
        // ����: Npc_Chief�� �ش��ϴ� QuestReporter�� ã���ϴ�.
        GameObject npcChief = GameObject.Find("Npc_Chief");
        if (npcChief != null)
        {
            QuestReporter chiefReporter = npcChief.GetComponent<QuestReporter>();
            if (chiefReporter != null)
            {
                questReporters.Add(10 + 1000, chiefReporter);
                // �ʿ��� �ٸ� ��ȭ ID�͵� ����
                // ��: questReporters.Add(20 + 1000, chiefReporter);
            }
        }
        // �ٸ� NPC�� ������Ʈ�� ���� QuestReporter�� �����ϰ� �߰�
        // ����:
        // GameObject shopKeeper = GameObject.Find("ShopKeeper");
        // if (shopKeeper != null)
        // {
        //     QuestReporter shopReporter = shopKeeper.GetComponent<QuestReporter>();
        //     if (shopReporter != null)
        //     {
        //         questReporters.Add(11 + 2000, shopReporter);
        //     }
        // }
    }

    void GenerateQuestActions()
    {
        // Npc_Chief ������Ʈ�� �ִ� NpcQuest�� QuestSender() �޼��带 �����մϴ�.
        GameObject npcChief = GameObject.Find("Npc_Chief");
        if (npcChief != null)
        {
            NpcQuest npcQuest = npcChief.GetComponent<NpcQuest>();
            if (npcQuest != null)
            {
                // Ư�� ��ȭ ID�� NpcQuest�� QuestSender() �޼��带 �����մϴ�.
                questActions.Add(10 + 1000, npcQuest.QuestSender);
                // �ʿ信 ���� �ٸ� ��ȭ ID�� �޼��嵵 ������ �� �ֽ��ϴ�.
            }
            else
            {
                Debug.LogWarning("Npc_Chief ������Ʈ�� NpcQuest ������Ʈ�� �����ϴ�.");
            }
        }
        else
        {
            Debug.LogWarning("Npc_Chief ������Ʈ�� ã�� �� �����ϴ�.");
        }
    }
    void GenerateData()
    {
        // Talk Data
        // ���� : 1000, ���� : 2000, �ε� ������ : 3000
        // �л�(������) : 10000, ����(ȩ��) : 4000
        // ShopPotal : 100, OtherPotal : 200, 


        talkData.Add(1000, new string[]
        { 
            " ",
            "...",
            "�λ� ���ݾ�.."
        });

        talkData.Add(2000, new string[]
        {
            "�̰��� �����Դϴ�",
            "�����̶��.."
        });

        talkData.Add(3000, new string[]
        {
            "��Ż Npc �Դϴ�.",
            "�� ������.."
        });

        talkData.Add(100, new string[]
        {
            "�����Դϴ� ��"
        });

        talkData.Add(200, new string[] {
            "������ �����մϴ�"
        });

        // Quest Talk
        talkData.Add(10 + 1000, new string[]
        {
            "� ��",
            "���� �ڿ� �ִ� ������ ����",
            "���̾��ٰ�? ... ��.. �� �ٰ� .."
        });

        talkData.Add(11 + 2000, new string[]
        {
            "�� �׷� ���� �����̶��.",
            "ü�� �����̶� ���� ������ ���",
            "��� ��İ�? �� �˰Ե��־� �Ӹ�"
        });


        talkData.Add(20 + 2000, new string[]
        {
            "�� �׷� ������ �ٽ� ���ư���",
            "��� ���İ�?",
            "R : ü�� ����, T : ���� ����",
            "���� �� ��"
        });

        talkData.Add(20 + 1000, new string[]
        {
            "�Դ�?",
            "������ ü���̶� ������ 10 �÷��ִ� �� ��Ե���",
            "����ѹ� ������?",
            "���� �Ա��� ���� ���� �ɾ��",
            "�׸��� �������� ���̷����� �־�",
            "���̷��� �����̿� ���� �� �Ѿƿðž�",
            "�����ϵ��� ��"
        });

        talkData.Add(30 + 3000, new string[]
        {
            "��ø�, ��ų ������ ���ٰ�",
            "Q ��ų�� �ϴÿ��� ��ź�� �������� ��ų�̾�",
            "W ��ų�� �ٶ󺸴� �������� ���� �߻��ϴ� ��ų�̰�",
            "E ��ų�� ������ ���� ���� ������ ��ų�̾�",
            "���� ������ Ŭ���� �� �� �����Ŷ� �Ͼ�"
        });

        talkData.Add(40 + 3000, new string[]
        {
            "����߾�",
            "������ �� �����µ� ����Ѱ�",
            "���� �Խ��� �տ� �ִ� ģ������ ����"
        });

        // Object
        talkData.Add(30 + 300, new string[]
        {
            "�غ�ƾ�?",
            "Ŭ�����ϱ� ���� �� ���ƿ�",
            "�׷� ������"
        });

        talkData.Add(40 + 500, new string[]
        {

        });

        //Name Data
        //nameData.Add(1000, "���� ����");
        //nameData.Add(2000, "���� ����");
        //nameData.Add(3000, "���� ����");

    }

    public string GetTalk(int id, int talkIndex)
    {
        // Exception
        if (!talkData.ContainsKey(id))
        {
            // �ش� ����Ʈ ���� ���� ��簡 ���� ��.
            // ����Ʈ �� ó�� ��縦 ������ �´�.
            if (!talkData.ContainsKey(id - id % 10))
            {
                return GetTalk(id - id % 100, talkIndex);

            }
            else
            {
                // ����Ʈ �� ó�� ��縶�� ���� �� ( ���� )
                // �⺻ ��縦 ������ �´�.
                return GetTalk(id - id % 10, talkIndex);
            }
        }

        if (talkIndex == talkData[id].Length)
        {
            // ��簡 ���� ��� Report() �Լ� ȣ��
            if (questReporters.ContainsKey(id))
            {
                Debug.Log("Report");
                questReporters[id].Report();
            }

            // ��簡 ���� ��� ���ε� QuestSender() �޼��� ȣ��
            if (questActions.ContainsKey(id))
            {
                Debug.Log("QuestSender");
                questActions[id].Invoke();
            }

            return null;
        }
        else
        {
            return talkData[id][talkIndex];
        }
    }

    public string GetObjName(int id, string name)
    {
        return name;
    }
}
