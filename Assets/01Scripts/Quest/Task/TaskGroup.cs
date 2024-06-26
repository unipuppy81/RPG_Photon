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
    public Quest Owner {  get; private set; }
    public bool IsAllTaskComplete => tasks.All(x => x.IsComplete);
    public bool IsComplete => State == TaskGroupState.Complete;
    public TaskGroupState State { get; private set; }

    /// <summary>
    /// 다른 taskgroup을 copy하는 생성자
    /// </summary>
    /// <param name="copyTarget"></param>
    public TaskGroup(TaskGroup copyTarget)
    {
        tasks = copyTarget.Tasks.Select(x => Object.Instantiate(x)).ToArray();

        foreach (var task in tasks)
        {
            task.onStateChanged += OnTaskStateChanged;  // Subscribe to task state change event
        }
    }

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
            //Quest가 가진 여러개의 TaskGroup중에 
            // 현재 작동해야하는 TaskGroup이 시작될때 실행됨
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

    private void OnTaskStateChanged(Task task, TaskState currentState, TaskState prevState)  // Event handler for task state change
    {
        if (currentState == TaskState.Complete && IsAllTaskComplete)
        {
            Complete();
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
