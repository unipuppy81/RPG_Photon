using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

using Debug = UnityEngine.Debug;

public enum QuestState
{
    Inactive,
    Running,
    Complete,   // ����Ʈ �ڵ� �Ϸ�
    Cancel,
    WaitingForcompletion // ����Ʈ �Ϸ�� User�� ����Ʈ �ϷḦ �������ϴ� ����Ʈ
}

[CreateAssetMenu(menuName = "Quest/Quest", fileName = "Quest_")]
public class Quest : ScriptableObject
{
    #region Event
    public delegate void TaskSuccessChangedHandler(Quest quest, Task task, int currentSuccess, int prevSuccess);
    public delegate void CompletedHandler(Quest quest);
    public delegate void CancelHandler(Quest quest);
    public delegate void NewTaskGroupHandler(Quest quest, TaskGroup currentTaskGroup, TaskGroup prevTaskGroup); 
    #endregion

    [SerializeField]
    private Category category;
    [SerializeField]
    private Sprite icon;


    [Header("Text")]
    [SerializeField]
    private string codeName;
    [SerializeField]
    private string displayName;
    [SerializeField, TextArea]
    private string description;

    [Header("Task")]
    [SerializeField]
    private TaskGroup[] taskGroups;

    [Header("Reward")]
    [SerializeField]
    private Reward[] rewards;

    [Header("Option")]
    [SerializeField]
    private bool useAutoComplete;
    [SerializeField]
    private bool isCancelable;
    [SerializeField]
    private bool isSavable;

    private int currentTaskGroupIndex;
    
    [Header("Condition")]
    [SerializeField]
    private Condition[] acceptionConditions;
    [SerializeField]
    private Condition[] cancelConditons;

    public Category Category => category;
    public Sprite Icon => icon;
    public string CodeName => codeName;
    public string DisplayName => displayName;
    public string Description => description;
    public QuestState State {  get; private set; }


    public TaskGroup CurrentTaskGroup
    {
        get
        {
            if (currentTaskGroupIndex < 0 || currentTaskGroupIndex >= taskGroups.Length)
            {
                throw new IndexOutOfRangeException("currentTaskGroupIndex is out of bounds");
            }
            return taskGroups[currentTaskGroupIndex];
        }
    }

    //public TaskGroup CurrentTaskGroup => taskGroups[currentTaskGroupIndex];
    public IReadOnlyList<TaskGroup> TaskGroups => taskGroups;
    public IReadOnlyList<Reward> Rewards => rewards;
    public bool IsRegistered => State != QuestState.Inactive;
    public bool IsCompletable => State == QuestState.WaitingForcompletion;
    public bool IsComplete => State == QuestState.Complete;
    public bool IsCancel => State == QuestState.Cancel;
    public virtual bool IsCancelable => isCancelable && cancelConditons.All(x => x.IsPass(this));
    public bool IsAcceptable => acceptionConditions.All(x => x.IsPass(this));
    public virtual bool IsSavable => isSavable;

    /*
     * ���� �̺�Ʈ ����ǵ� �ʿ��� �̺�Ʈ�� 
     * ����޾����� ������ �̺�Ʈ
     * Quest �Ϸ������� ������ �̺�Ʈ
     * Quest ��ҽ� ������ �̺�Ʈ
     * ���ο� TaskGroup�� ���۵ɶ� ������ �̺�Ʈ
     */
    public event TaskSuccessChangedHandler onTaskSuccessChanged;
    public event CompletedHandler onCompleted;
    public event CancelHandler onCanceled;
    public event NewTaskGroupHandler onNewTaskGroup;

    /// <summary>
    /// Awake ������ �Լ� Quest�� System�� ��ϵǾ����� ����
    /// </summary>
    public void OnRegister()
    {
        // �Լ��� ù �κп� Assert�� Quest�� ��ϵǾ��ִµ� �� ����Ϸ��ϸ� ��������
        // ���ڷ� ���� ���� false�� �� ������ error�� ���
        Debug.Assert(!IsRegistered, "This quest has already been registerd.");


        /*
         * Quest���� ���� event�� �����ϴ� callbacks �Լ��� task�� ����ϹǷμ�
         * �ܺο��� task�� ������ event�� ����� �ʿ� ���� 
         * quest�� event�� ������ָ� task�� ���� Ƚ���� ���ߴٴ� ���� �� �� ����
         */
        foreach(var taskGroup in taskGroups)
        {
            taskGroup.Setup(this);
            foreach(var task in taskGroup.Tasks)
            {
                task.onSuccessChanged += OnSuccessChanged;
            }

            State = QuestState.Running;
            CurrentTaskGroup.Start();
        }
    }

    /// <summary>
    /// ���� �޴� �Լ�
    /// </summary>
    /// <param name="category"></param>
    /// <param name="target"></param>
    /// <param name="successCount"></param>
    public void ReceiveReport(string category, object target, int successCount)
    {
        Debug.Assert(IsRegistered, "This quest has not been registered.");
        Debug.Assert(!IsCancel, "This quest has been canceled.");

        if (IsComplete)
            return;

        CurrentTaskGroup.ReceiveReport(category, target, successCount);

        if (CurrentTaskGroup.IsAllTaskComplete)
        {
            var prevTaskGroup = taskGroups[currentTaskGroupIndex];

            if (currentTaskGroupIndex + 1 < taskGroups.Length)
            {
                currentTaskGroupIndex++;
                CurrentTaskGroup.Start();
                onNewTaskGroup?.Invoke(this, CurrentTaskGroup, prevTaskGroup);
            }
            else
            {
                State = QuestState.WaitingForcompletion;

                if (useAutoComplete)
                {
                    Complete();
                }
            }
        }
        else // Task option �߿� �Ϸ�Ǿ ����ؼ� ����޴� �ɼ�
        {
            State = QuestState.Running;
        }
    }

    /// <summary>
    /// Quest�� �Ϸ��ϴ� �Լ�
    /// </summary>
    public void Complete()
    {
        CheckIsRunning();

        foreach (var taskGroup in taskGroups)
        {
            taskGroup.Complete();
        }

        State = QuestState.Complete;

        foreach(var reward in rewards)
        {
            reward.Give(this);
        }

        onCompleted?.Invoke(this);

        onTaskSuccessChanged = null;
        onCompleted = null;
        onCanceled = null;
        onNewTaskGroup = null;
    }

    /// <summary>
    /// ����Ʈ�� ĵ���ϴ� �Լ�
    /// </summary>
    public virtual void Cancel()
    {
        CheckIsRunning();
        Debug.Assert(IsCancelable, "This quest can't be canceled");
            
        State = QuestState.Cancel;
        onCanceled?.Invoke(this);
    }

    public bool ContainsTarget(object target) => taskGroups.Any(x => x.ContainsTarget(target));
    public bool ContainsTarget(TaskTarget target) => ContainsTarget(target.Value);

    /// <summary>
    /// ������ ���纻�� ���鋚 task�� �����ؼ� �־�������
    /// ���� quest�� �ٸ� module �߿��� task ó��
    /// �ǽð����� �������� �ٲ�� ��찡 �ִٸ� 
    /// �� module �鵵 taskó�� ���纻�� ���� �־������ 
    /// </summary>
    /// <returns></returns>
    public Quest Clone()
    {
        var clone = Instantiate(this);
        clone.taskGroups = taskGroups.Select(x => new TaskGroup(x)).ToArray();
        
        return clone;
    }
    
    public QuestSaveData ToSaveData()
    {
        return new QuestSaveData
        {
            codeName = codeName,
            state = State,
            taskGroupIndex = currentTaskGroupIndex,
            taskSuccessCounts = CurrentTaskGroup.Tasks.Select(x => x.CurrentSuccess).ToArray()
        };
    }
    public void LoadFrom(QuestSaveData saveData)
    {
        State = saveData.state;
        currentTaskGroupIndex = saveData.taskGroupIndex;

        for (int i = 0; i < currentTaskGroupIndex; i++)
        {
            var taskGroup = taskGroups[i];
            taskGroup.Start();
            taskGroup.Complete();
        }

        if (currentTaskGroupIndex < taskGroups.Length)
        {
            CurrentTaskGroup.Start();
            for (int i = 0; i < saveData.taskSuccessCounts.Length; i++)
            {
                CurrentTaskGroup.Tasks[i].CurrentSuccess = saveData.taskSuccessCounts[i];
            }
        }
        else
        {
            throw new IndexOutOfRangeException("currentTaskGroupIndex is out of bounds during LoadFrom");
        }
    }

    private void OnSuccessChanged(Task task, int currentSuccess, int prevSuccess)
        => onTaskSuccessChanged?.Invoke(this, task, currentSuccess, prevSuccess);


    [Conditional("UNITY_EDITOR")]
    private void CheckIsRunning()
    {
        Debug.Assert(IsRegistered, "This quest has already been registerd.");
        Debug.Assert(!IsCancel, "This quest has been canceled.");
        Debug.Assert(!IsComplete, "This quest has already been completed.");
    }
}
