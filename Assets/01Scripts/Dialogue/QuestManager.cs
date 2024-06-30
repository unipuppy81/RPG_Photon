using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public int questId;
    public int questActionIndex;

    public GameObject[] questObject; // portal

    [SerializeField] GameObject questListManagerObject;
    QuestListManager qListManager;


    public Dictionary<int, QuestData> questList;



    [Header("GameQuest")]
    public int killCount;
    public int hpGlobeCount;
    public int mpGlobeCount;


    [SerializeField] GameObject questClearPanel;

    void Awake()
    {
        questList = new Dictionary<int, QuestData>();
        GenerateData();
    }

    void Start()
    {
        //qListManager = questListManagerObject.GetComponent<QuestListManager>();
    }


    // ����Ʈ ���� �г�

    void ClearQuestPanelOn()
    {
        //questClearPanel.SetActive(true);
    }

    // ��ȭ ����Ʈ 
    void GenerateData()
    {
        questList.Add(10, new QuestData("���� �ѹ��� ����",      // ����Ʈ �̸�
                                new int[] { 1000, 3000 },  // 1000 : 1000�� npcid object
                                "0"));

        questList.Add(20, new QuestData("���ǻ��",
                                 new int[] { 2000, 1000 },
                                 "0"));

        questList.Add(30, new QuestData("���� ����",
                                new int[] { 3000 },
                                "0"));


        questList.Add(40, new QuestData("���� Ŭ�����ϱ�",
                               new int[] { 3000, 3000 },
                               "0"));
    }





    public int GetQuestTalkIndex(int id)
    {
        return questId + questActionIndex;
    }

    public string CheckQuest(int id)
    {
        // ���� NPC Ȯ��
        if (id == questList[questId].npcId[questActionIndex])
        {
            questActionIndex++;
        }

        // Control Quest Object
        ControlObject();


        //Debug.Log(questList[questId].npcId.Length);
        // Talk Complete & Next Quest
        if (questActionIndex == questList[questId].npcId.Length)
        {
            NextQuest();
        }

        // ���� ����Ʈ�� ��ȯ
        return questList[questId].questName;
    }

    void NextQuest()
    {
        questId += 10;
        questActionIndex = 0;
    }

    void ControlObject()
    {
        switch (questId)
        {
            case 10:
                if (questActionIndex == 1)
                {
                    //ClearQuestPanelOn();
                    //Debug.Log("���� ���̿��� �� �ɱ� Ŭ����");
                }
                else if (questActionIndex == 2)
                {
                    //Debug.Log("�� �ڲ� �ߴ°���?");
                    //questObject[0].SetActive(true);
                }
                break;


            case 20:
                if (questActionIndex == 1)
                {
                   // Debug.Log("���� �����ϱ� Ŭ����");
                }
                else if (questActionIndex == 2)
                {

                }
                break;

            case 30:
                if (questActionIndex == 1)
                {
                    //questObject[1].SetActive(true);
                }
                break;

            case 40:
                if (questActionIndex == 2)
                {
                   // Debug.Log("���� Ŭ���ӤӾ�");
                }
                break;
        }
    }

    // ���� ����Ʈ





    // ����


}
