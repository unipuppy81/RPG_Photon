using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class QuestSystem : MonoBehaviour
{
    #region Save Path
    private const string kSaveRootPath = "questSystem";
    private const string kActiveQuestsSavePath = "activeQuests";
    private const string kCompletedQuestsSavePath = "completedQuests";
    private const string kActiveAchievementsSavePath = "activeAchievement";
    private const string kCompletedAchievementsSavePath = "completedAchievement";
    #endregion

    #region Events
    public delegate void QuestRegisteredHandler(Quest newQuest);
    public delegate void QuestCompletedHandler(Quest quest);
    public delegate void QuestCanceledHandler(Quest quest);
    #endregion

    private static QuestSystem instance;
    private static bool isApplicationQuitting;

    public static QuestSystem Instance
    {
        get
        {
            if(!isApplicationQuitting && instance == null)
            {
                instance = FindObjectOfType<QuestSystem>();
                if(instance == null)
                {
                    instance = new GameObject("QuestSystem").AddComponent<QuestSystem>();
                    DontDestroyOnLoad(instance.gameObject);
                }
            }
            return instance;
        }
    }

    [SerializeField]
    private List<Quest> activeQuests = new List<Quest>();
    [SerializeField]
    private List<Quest> completedQuests = new List<Quest>();

    private List<Quest> activeAchievements = new List<Quest>();
    private List<Quest> completedAchievements = new List<Quest>();

    [SerializeField]
    private QuestDatabase questDatabase;
    [SerializeField]
    private QuestDatabase achievementDatabase;

    public event QuestRegisteredHandler onQuestRegistered;
    public event QuestCompletedHandler onQuestCompleted;
    public event QuestCanceledHandler onQuestCanceled;

    public event QuestRegisteredHandler onAchievementRegistered;
    public event QuestCompletedHandler onAchievementCompleted;
    public event QuestCanceledHandler onArchievementCanceled;
    public IReadOnlyList<Quest> ActiveQuests => activeQuests;
    public IReadOnlyList<Quest> CompletedQuests => completedQuests;

    public IReadOnlyList<Quest> ActiveAchievements => activeAchievements;
    public IReadOnlyList<Quest> CompletedAchievements => completedAchievements;


    private void Awake()
    {
        StartCoroutine(DelayedSetup());

        questDatabase = Resources.Load<QuestDatabase>("QuestDatabase");
        achievementDatabase = Resources.Load<QuestDatabase>("AchievementDatabase");


        if (achievementDatabase == null)
            return;
    }

    private IEnumerator DelayedSetup()
    {
        yield return new WaitUntil(() => GameManager.isPlayGame);


        if (!Load())
        {
            foreach (var achievement in achievementDatabase.Quests)
            {
                Register(achievement);
            }
        }
    }

    // �ӽ÷� ���� ����� ����
    private void OnApplicationQuit()
    {
        isApplicationQuitting = true;
        Save();
    }

    /// <summary>
    /// ����Ʈ ����ϴ� �Լ�
    /// </summary>
    /// <param name="quest"></param>
    /// <returns></returns>
    public Quest Register(Quest quest)
    {
        var newQuest = quest.Clone();

        if(newQuest is Achievement)
        {
            newQuest.onCanceled += OnAchievementcompleted;

            activeAchievements.Add(newQuest);

            newQuest.OnRegister();
            onAchievementRegistered?.Invoke(newQuest);
        }
        else
        {
            newQuest.onCompleted += OnQuestCompleted;
            newQuest.onCanceled += OnQuestCanceled;

            activeQuests.Add(newQuest);

            newQuest.OnRegister();
            onQuestRegistered?.Invoke(newQuest);
        }

        return newQuest;
    }

    // ���� �޴� �Լ� ����, �ܺ�
    public void ReceiveReport(string category, object target, int successCount)
    {
        ReceiveReport(activeQuests, category, target, successCount);
        ReceiveReport(activeAchievements, category, target, successCount);
    }

    public void ReceiveReport(Category category, TaskTarget target, int successCount)
        => ReceiveReport(category.CodeName, target.Value, successCount);

    private void ReceiveReport(List<Quest> quests, string category, object target, int successCount)
    {
        // ToArray�� List�� �纻�� ���� for�� ������ ������
        // for���� ���ư��� ���߿� quest�� �Ϸ�Ǿ� ��Ͽ��� �������� �ֱ� ����
        foreach(var quest in quests.ToArray())
            quest.ReceiveReport(category, target, successCount);
    }

    public void CompleteWaitingQuests()
    {
        foreach (var quest in activeQuests.ToList())
        {
            if (quest.IsCompletable)
            {
                quest.Complete();
            }
        }
        /*
         * �ٸ������� ����Ʈ ���� �ϴ¹�
         * QuestSystem.Instance.CompleteWaitingQuests();
         * QuestSystem.Instance.Save();
         */
    }

    // Quest�� ��Ͽ� �ִ��� Ȯ���ϴ� �Լ�
    public bool ContainsInActiveQuests(Quest quest) => activeQuests.Any(x => x.CodeName == quest.CodeName);
    public bool ContainsInCompleteQuests(Quest quest) => completedQuests.Any(x => x.CodeName == quest.CodeName);
    public bool ContainsInActiveAchievement(Quest quest) => activeAchievements.Any(x => x.CodeName == quest.CodeName);
    public bool ContainsInCompleteAchievement(Quest quest) => completedAchievements.Any(x => x.CodeName == quest.CodeName);

    /*
     * save�� ���� ��Ȳ���� �� �� �ִµ� 
     * Quest�� Event�� ����ؼ� Task�� ���� Ƚ���� ���ϸ� �����ϰ� ���� �ǰ�
     * �Ȱ��� event�� ����ؼ� quest�� �Ϸ�Ǹ� �����ϰ� ���� �ǰ� 
     * �ƴϸ� ���� ��� save �Լ� ���� ����ص� ��
     */
    public void Save()
    {
        var root = new JObject();
        root.Add(kActiveQuestsSavePath, CreateSaveDatas(activeQuests));
        root.Add(kCompletedQuestsSavePath, CreateSaveDatas(completedQuests));
        root.Add(kActiveAchievementsSavePath, CreateSaveDatas(activeAchievements));
        root.Add(kCompletedAchievementsSavePath, CreateSaveDatas(completedAchievements));

        PlayerPrefs.SetString(kSaveRootPath, root.ToString());
        PlayerPrefs.Save();
    }

    public bool Load()
    {
        if (PlayerPrefs.HasKey(kSaveRootPath))
        {
            var root = JObject.Parse(PlayerPrefs.GetString(kSaveRootPath));

            LoadSaveDatas(root[kActiveQuestsSavePath], questDatabase, LoadActiveQuest);
            LoadSaveDatas(root[kCompletedQuestsSavePath], questDatabase, LoadCompletedQuest);

            LoadSaveDatas(root[kActiveAchievementsSavePath], questDatabase, LoadActiveQuest);
            LoadSaveDatas(root[kCompletedAchievementsSavePath], questDatabase, LoadCompletedQuest);

            return true;
        }
        else
            return false;
    }

    //save Data ����� �Լ�
    private JArray CreateSaveDatas(IReadOnlyList<Quest> quests)
    {
        var saveDatas = new JArray();
        foreach(var quest in quests)
        {
            if (quest.IsSavable)
            {
                // Save Data�� Json ���·� ��ȯ��Ų �Ŀ� JSON Array�� �־��ִ� �ڵ�
                saveDatas.Add(JObject.FromObject(quest.ToSaveData()));
            }
        }
        return saveDatas;
    }

    private void LoadSaveDatas(JToken datasToken, QuestDatabase database, System.Action<QuestSaveData, Quest> onSuccess)
    {
        var datas = datasToken as JArray;
        foreach(var data in datas)
        {
            var saveDatas = data.ToObject<QuestSaveData>();
            var quest = database.FindQuestBy(saveDatas.codeName);
            onSuccess.Invoke(saveDatas, quest);
        }
    }

    /// <summary>
    /// �ҷ��� ����Ʈ���� ������ְ� ����� ����Ʈ�� ����� �����͸� �־��ִ� �Լ�
    /// </summary>
    /// <param name="saveData"></param>
    /// <param name="quest"></param>
    private void LoadActiveQuest(QuestSaveData saveData, Quest quest)
    {
        var newQuest = Register(quest);
        newQuest.LoadFrom(saveData);
    }

    private void LoadCompletedQuest(QuestSaveData saveData, Quest quest)
    {
        var newQuest = quest.Clone();
        newQuest.LoadFrom(saveData);

        if(newQuest is Achievement)
        {
            completedAchievements.Add(newQuest);
        }
        else
        {
            completedQuests.Add(newQuest);
        }
    }

    // �ռ� ���� event ���� quest event�� ����Ҽ��ְ� �ݹ��Լ� ����
    #region Callback

    // �Ʒ� �Լ����� ����Ʈ�� ������ �ڵ����� ����Ʈ�� ���� �߰�����
    private void OnQuestCompleted(Quest quest)
    {
        activeQuests.Remove(quest);
        completedQuests.Add(quest);

        onQuestCompleted?.Invoke(quest);
    }

    private void OnQuestCanceled(Quest quest)
    {
        activeQuests.Remove(quest);
        onQuestCompleted?.Invoke(quest);

        Destroy(quest, Time.deltaTime);
    }

    private void OnAchievementcompleted(Quest achievement)
    {
        activeAchievements.Remove(achievement);
        completedAchievements.Add(achievement);

        onAchievementCompleted?.Invoke(achievement);
    }
    #endregion
}
