using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
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

    [Header("BuildereQuest")]
    [SerializeField]
    private GameObject helmetBuilder;
    [SerializeField]
    private GameObject helmet;


    [SerializeField]
    private GameObject questClearPanel;
    [SerializeField] 
    private TextMeshProUGUI nextQuestText;
    private Image panelImage;
    private Color initialColor; // �ʱ� ���� ����� ����

    void Awake()
    {
        questList = new Dictionary<int, QuestData>();
        GenerateData();

        panelImage = questClearPanel.GetComponent<Image>();
        initialColor = panelImage.color;
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

                }
                else if (questActionIndex == 2)
                {
                    ShowQuestClearPanel("���ο� ����Ʈ : �κ��� ��Ź");
                }
                break;


            case 20:
                if (questActionIndex == 1)
                {

                }
                else if (questActionIndex == 2)
                {

                }
                else if(questActionIndex == 3)
                {
                    ShowQuestClearPanel("���ο� ����Ʈ : ���� ������ ����");

                    helmet.SetActive(false);
                    helmetBuilder.SetActive(true);
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

    public void ShowQuestClearPanel(string questText)
    {
        // ���ο� ����Ʈ �ؽ�Ʈ ����
        nextQuestText.text = questText;

        // �г��� Ȱ��ȭ
        questClearPanel.SetActive(true);
        
        // ���� �ʱ�ȭ
        ResetAlpha();

        // 2�� �Ŀ� ���̵�ƿ� �ڷ�ƾ ����
        StartCoroutine(FadeOutPanel(2.0f));
    }

    private IEnumerator FadeOutPanel(float fadeOutTime)
    {
        // ���
        yield return new WaitForSeconds(fadeOutTime);

        // ���� ����
        float startAlpha = panelImage.color.a;
        float rate = 1.0f / fadeOutTime;
        float progress = 0.0f;

        while (progress < 1.0f)
        {
            Color color = panelImage.color;
            color.a = Mathf.Lerp(startAlpha, 0, progress);
            panelImage.color = color;

            progress += rate * Time.deltaTime;
            yield return null;
        }

        // ������ �����ϰ� ����� ��Ȱ��ȭ
        Color finalColor = panelImage.color;
        finalColor.a = 0.0f;
        panelImage.color = finalColor;

        questClearPanel.SetActive(false);
    }

    private void ResetAlpha()
    {
        // �ʱ� �������� �����Ͽ� ���� �ʱ�ȭ
        panelImage.color = initialColor;
    }
}
