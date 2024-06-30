using System.Collections;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Linq;
using UnityEngine;

public enum TaskState
{
    Inactive,
    Running,
    Complete
}

[CreateAssetMenu(menuName = "Quest/Task/Task", fileName ="Task_")]
public class Task : ScriptableObject
{
    /*
     * ��ǥ������ UI ���� ������ UI Update Code �� Event�� �����س�����
     * Task�� ���¸� Update���� ��� ������ �ʿ���� 
     * ���°� �ٲ�� �˾Ƽ� UI�� Update �ȴ�.
     */

    #region Events
    public delegate void StateChangedHandler(Task task, TaskState currentState, TaskState prevState);
    public delegate void SuccessChangedHandler(Task task, int currentSuccess, int prevSuccess);
    #endregion


    [SerializeField]
    private Category category;

    [Header("Text")]
    [SerializeField]
    private string codeName;
    [SerializeField]
    private string description;

    [Header("Action")]
    [SerializeField]
    private TaskAction action;

    [Header("Target")]
    [SerializeField]
    private TaskTarget[] targets;


    [Header("Setting")]
    [SerializeField]
    private InitialSuccessValue initialSuccessValue;
    [SerializeField]
    private int needSuccessToComplete;
    [SerializeField]
    private bool canReceiveReportDuringCompletion; // Item 100�� ��Ƽ� �Ϸ��ϴ� Quest�ε� ������ ������100���� ������� ����Ʈ�� �Ϸ��ϱ� ���� 50�� �������� ����
    // �� �� ������ 100���� ��Ҵٰ� ���̻� 100���� ��Ҵٰ� ���̻� ���� �ȹ޾ƹ����� ������ ������ Quest �Ϸ� ��������
    // �� ���(Option)�� Action�� Set�� �����ؼ� ���

    private TaskState state;
    private int currentSuccess;

    public event StateChangedHandler onStateChanged;
    public event SuccessChangedHandler onSuccessChanged;

    public int CurrentSuccess 
    {
        get => currentSuccess;
        set
        {
            int prevSuccess = currentSuccess;
            currentSuccess = Mathf.Clamp(value, 0, needSuccessToComplete);
            if(currentSuccess != prevSuccess)
            {
                State = currentSuccess == needSuccessToComplete ? TaskState.Complete : TaskState.Running;
                onSuccessChanged?.Invoke(this, currentSuccess, prevSuccess);
            }
        }
    }
    public Category Category => category;
    public string CodeName => codeName;
    public string Description => description;
    public int NeedSuccessToComplete => needSuccessToComplete;
    public TaskState State
    {
        get => state;
        set
        {
            var prevState = state;
            state = value;
            onStateChanged?.Invoke(this, state, prevState);
        }
    }

    public bool IsComplete => State == TaskState.Complete;
    public Quest Owner { get; private set; }

    public void Setup(Quest owner)
    {
        Owner = owner;
    }

    /// <summary>
    /// Task�� ���۵ɶ� ������ �Լ�
    /// </summary>
    public void Start()
    {
        State = TaskState.Running;

        if (initialSuccessValue)
            currentSuccess = initialSuccessValue.GetValue(this);
    }


    /// <summary>
    /// Task�� ������ ����� �Լ�
    /// </summary>
    public void End()
    {
        onStateChanged = null;
        onSuccessChanged = null;
    }

    /// <summary>
    /// Task�� ��� �Ϸ��ϴ� �Լ�
    /// </summary>
    public void Complete()
    {
        CurrentSuccess = needSuccessToComplete;
    }


    public void ReceiveReport(int successCount)
    {
        CurrentSuccess = action.Run(this, CurrentSuccess, successCount);
    }

    /// <summary>
    /// TaskTarget�� ���� �� Task�� ���� Ƚ���� ���� ���� ������� Ȯ���ϴ� �Լ�
    /// Setting �س��� Target�� �߿� �ش��ϴ� Target�� ������ true, �ƴϸ� false ��ȯ
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public bool IsTarget(string category, object target)
        => Category == category &&
        targets.Any(x => x.IsEqual(target)) &&
        (!IsComplete || (IsComplete && canReceiveReportDuringCompletion));

    public bool ContainsTarget(object target) => targets.Any(x => x.IsEqual(target));


}
