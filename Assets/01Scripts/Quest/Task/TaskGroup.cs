using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum TaskGroupState
{
    Inactive,
    Running,
    Complete
}

[System.Serializable]
public class TaskGroup 
{
    [SerializeField]
    private Task[] tasks;

    public IReadOnlyList<Task> Tasks => tasks;

    // TaskGroup�� ������
    public Quest Owner {  get; private set; }
    
    // Task�� ��� �Ϸ�Ǿ��°�?
    public bool IsAllTaskComplete => tasks.All(x => x.IsComplete);
    public bool IsComplete => State == TaskGroupState.Complete;
    public TaskGroupState State { get; private set; }

    /// <summary>
    /// �ٸ� taskgroup�� copy�ϴ� ������
    /// </summary>
    /// <param name="copyTarget"></param>
    public TaskGroup(TaskGroup copyTarget)
    {
        tasks = copyTarget.Tasks.Select(x => Object.Instantiate(x)).ToArray();
    }


    /// <summary>
    /// task���� Setup�� ���� �����Ҷ� �����Ѱ�ó��
    /// ���⼭�� ��� task���� Setup�� ���ش�
    /// </summary>
    /// <param name="owner"></param>
    public void Setup(Quest owner)
    {
        Owner = owner;
        foreach(var task in tasks)
        {
            task.Setup(owner);
        }
    }

    public void Start()
    {
        State = TaskGroupState.Running;
        foreach(var task in tasks)
        {
            // Quest�� ���� �������� TaskGroup�߿� 
            // ���� �۵��ؾ��ϴ� TaskGroup�� ���۵ɶ� �����
            task.Start();
        }
    }

    public void End()
    {
        State = TaskGroupState.Complete;
        foreach(var task in tasks)
        {
            task.End();
        }
    }

    /// <summary>
    /// task�� ���� Ƚ�� �����ϴ� �Լ�
    /// </summary>
    /// <param name="category"></param>
    /// <param name="target"></param>
    /// <param name="successCount"></param>
    public void ReceiveReport(string category, object target, int successCount)
    {
        foreach(var task in tasks)
        {
            if(task.IsTarget(category, target))
            {
                task.ReceiveReport(successCount);
            }
        }
    }

    public void Complete()
    {
        if (IsComplete)
            return;

        State = TaskGroupState.Complete;

        foreach(var task in tasks)
        {
            if (!task.IsComplete)
            {
                task.Complete();
            }
        }
    }

    public Task FindTaskByTarget(object target) => tasks.FirstOrDefault(x => x.ContainsTarget(target));
    public Task FindTaskByTarget(TaskTarget target) => FindTaskByTarget(target.Value);
    public bool ContainsTarget(object target) => tasks.Any(x => x.ContainsTarget(target));
    public bool ContainsTarget(TaskTarget target) => ContainsTarget(target.Value);
}
