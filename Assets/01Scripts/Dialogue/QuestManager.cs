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
        questList.Add(10, new QuestData("����Ʈ 1",  
                                new int[] { 1000, 1000 }, 
                                "0"));

        questList.Add(20, new QuestData("����Ʈ 2",
                                 new int[] { 2000, 100, 2000 },
                                 "0"));

        questList.Add(30, new QuestData("����Ʈ 3",
                                new int[] { 3000 },
                                "0"));

        questList.Add(40, new QuestData("����Ʈ 4",
                               new int[] { 3000, 3000 },
                               "0"));
    }


    // Quest �Ϸ� ���� Ȯ��
    public bool IsQuestComplete(int questIdToCheck)
    {
        if (questList.ContainsKey(questIdToCheck))
        {
            return questActionIndex >= questList[questIdToCheck].npcId.Length;
        }
        return false;
    }


    public int GetQuestTalkIndex()
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

        // Talk Complete & Next Quest
        if (questActionIndex == questList[questId].npcId.Length)
        {
            NextQuest();
        }

        // ���� ����Ʈ�� ��ȯ
        return questList[questId].questName;
    }

    public void NextQuest()
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
                    Debug.Log("quest ID : " + questId + "  questActionIndex : " + questActionIndex);
                }
                else if (questActionIndex == 2)
                {
                    Debug.Log("quest ID : " + questId + "  questActionIndex : " + questActionIndex);
                }
                break;


            case 20:
                if (questActionIndex == 1)
                {
                    Debug.Log("quest ID : " + questId + "  questActionIndex : " + questActionIndex);
                }
                else if (questActionIndex == 2)
                {
                    Debug.Log("quest ID : " + questId + "  questActionIndex : " + questActionIndex);
                }
                break;

            case 30:
                if (questActionIndex == 1)
                {
                    Debug.Log("quest ID : " + questId + "  questActionIndex : " + questActionIndex);
                }
                break;

            case 40:
                if (questActionIndex == 2)
                {
                    Debug.Log("quest ID : " + questId + "  questActionIndex : " + questActionIndex);
                }
                break;
        }
    }

    // ���� ����Ʈ





    // ����


}
