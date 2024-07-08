using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData;
    Dictionary<int, System.Action> questActions; // ȣ���� �޼��带 �����ϱ� ���� ����
    Dictionary<int, QuestReporter> questReporters; // QuestReporter ������ ���� ���� �߰�
    QuestManager q;

    private void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        questActions = new Dictionary<int, System.Action>(); // ���� �ʱ�ȭ
        questReporters = new Dictionary<int, QuestReporter>(); // ���� �ʱ�ȭ

        GenerateData();
        GenerateQuestReporters();
        GenerateQuestActions();
    }

    private void Start()
    {
        q = GameObject.Find("QuestManager").GetComponent<QuestManager>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            q.NextQuest();
        }
    }
    void GenerateQuestReporters()
    {
        // �� ��ȭ ID�� �����ϴ� QuestReporter�� �����մϴ�.
        // ����: Npc_Nun �ش��ϴ� QuestReporter�� ã���ϴ�.
        GameObject npcNun = GameObject.Find("Npc_Nun");
        if (npcNun != null)
        {
            QuestReporter nunReporter = npcNun.GetComponent<QuestReporter>();
            if (nunReporter != null)
            {
                questReporters.Add(10 + 1000, nunReporter);
                questReporters.Add(11 + 1000, nunReporter);
            }
        }

        GameObject npcBuilder = GameObject.Find("Npc_Builder");
        if (npcBuilder != null)
        {
            QuestReporter builderReporter = npcBuilder.GetComponent<QuestReporter>();
            if (builderReporter != null)
            {
                questReporters.Add(20 + 2000, builderReporter);
                questReporters.Add(22 + 2000, builderReporter);
            }
        }
    }

    void GenerateQuestActions()
    {
        // Npc_Chief ������Ʈ�� �ִ� NpcQuest�� QuestSender() �޼��带 �����մϴ�.
        GameObject npcNun = GameObject.Find("Npc_Nun");
        if (npcNun != null)
        {
            NpcQuest npcQuest = npcNun.GetComponent<NpcQuest>();
            if (npcQuest != null)
            {
                // Ư�� ��ȭ ID�� NpcQuest�� QuestSender() �޼��带 �����մϴ�.
                questActions.Add(10 + 1000, npcQuest.QuestSender);
                questActions.Add(11 + 1000, npcQuest.QuestSender);
            }
        }

        // Npc_Builder ������Ʈ�� �ִ� NpcQuest�� QuestSender() �޼��带 �����մϴ�.
        GameObject npcBuilder = GameObject.Find("Npc_Builder");
        if (npcBuilder != null)
        {
            NpcQuest npcQuest = npcBuilder.GetComponent<NpcQuest>();
            if (npcQuest != null)
            {
                // Ư�� ��ȭ ID�� NpcQuest�� QuestSender() �޼��带 �����մϴ�.
                questActions.Add(20 + 2000, npcQuest.QuestSender);
                questActions.Add(22 + 2000, npcQuest.QuestSender);
            }
        }


    }
    void GenerateData()
    {
        // Talk Data
        // Nun : 1000, Builder : 2000
        // ��� : 100

        talkData.Add(100, new string[]
        {
            "����̴�"
        });

        talkData.Add(1000, new string[]
        { 
            "���̵��� �������� �ڶ����� ���ھ��."
        });

        talkData.Add(2000, new string[]
        {
            "�� ���縦 �������ؾ��ϴµ�.."
        });

        talkData.Add(3000, new string[]
        {
            "��Ż Npc �Դϴ�.",
            "�� ������.."
        });


        // Quest Talk
        talkData.Add(10 + 1000, new string[]
        {
            "���� ������ ���ɵ��� ���� ��Ÿ���� ���̵��� �����Ӱ� �پ�ٴҼ��� �����..",
            "���Բ��� �����ֽŴٰ��?",
            "�׷� ������ 5���� ������ ����ֽðھ��?"
        });

        talkData.Add(11 + 1000, new string[]{
            "�����մϴ�. ���п� ���̵��� �Ƚ��ϰ� �Ͱ��Ҽ� �ְھ��.",
            "���� �� �� �ƴ����� ��ʿ���."
        });

        talkData.Add(20 + 2000, new string[]
        {
            "���縦 �����ؾ��ϴµ� ������ ��� ���縦 �Ҽ��� ����.",
            "�ʰ� ã���ְڴٰ�?",
            "�ٸ� ������ �Ѿ�� �ٸ��ʿ��� �Ҿ������ ����."
        });

        talkData.Add(22 + 2000, new string[]
        {
            "����� ã�Ҵٰ�? ���� ã������ �� �ôµ�",
            "����Ƶ� ���� �� ���Ϸ� ���߰ھ�"
        });
    }

    public string GetTalk(int id, int talkIndex)
    {
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


        // ��簡 ����
        if (talkIndex == talkData[id].Length)
        {
            if (questReporters.ContainsKey(id))
            {
                questReporters[id].Report();
            }

            if (questActions.ContainsKey(id))
            {
                questActions[id].Invoke();
            }

            // QuestManager���� �ش� Quest�� �Ϸ�Ǿ����� üũ
            if (q.IsQuestComplete(id - 1000))
            {
                q.NextQuest();
            }

            return null;
        }
        else
        {
            return talkData[id][talkIndex];
        }
    }
}
