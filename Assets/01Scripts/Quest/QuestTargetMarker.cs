using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestTargetMarker : MonoBehaviour
{
    [SerializeField]
    private TaskTarget target; // �ش� target�� ���� task ã�ƿͼ� ����
    [SerializeField]
    private MarkerMaterialData[] markerMaterialDatas;

    private Dictionary<Quest, Task> targetTasksByQuest = new Dictionary<Quest, Task>();
    private Transform cameraTransform; // marker�� �׻� �÷��̾ �ٶ󺸰� �ؾ���
    private Renderer renderer; // task�� ī�װ��� ���� �̹����� �ٸ��� �����ֱ� ����

    private int currentRunningTargetTaskCount; // �������� task�� count���� ����

    [System.Serializable]
    private struct MarkerMaterialData
    {
        public Category category;
        public Material markerMaterial;
    }

    private void Awake()
    {
        cameraTransform = Camera.main.transform;
        renderer = GetComponent<Renderer>();
    }

    private void Start()
    {
        gameObject.SetActive(false);

        QuestSystem.Instance.onQuestRegistered += TryAddTargetQuest;
        foreach (var quest in QuestSystem.Instance.ActiveQuests)
            TryAddTargetQuest(quest);
    }

    private void Update()
    {
        var rotation = Quaternion.LookRotation((cameraTransform.position - transform.position).normalized);
        transform.rotation = Quaternion.Euler(0f, rotation.eulerAngles.y + 180f, 0f);
    }

    private void OnDestroy()
    {
        QuestSystem.Instance.onQuestRegistered -= TryAddTargetQuest;
        foreach ((Quest quest, Task task) in targetTasksByQuest)
        {
            quest.onNewTaskGroup -= UpdateTargetTask;
            quest.onCompleted -= RemoveTargetQuest;
            task.onStateChanged -= UpdateRunningTargetTaskCount;
        }
    }

    /// <summary>
    /// ��ϵ� Quest�� Ȯ���Ͽ� Target�� ��� �����ϴ� ����
    /// </summary>
    /// <param name="quest"></param>
    private void TryAddTargetQuest(Quest quest)
    {
        if (target != null && quest.ContainsTarget(target))
        {
            quest.onNewTaskGroup += UpdateTargetTask;
            quest.onCompleted += RemoveTargetQuest;

            UpdateTargetTask(quest, quest.CurrentTaskGroup);
        }
    }

    /// <summary>
    /// �������� Task ��ü
    /// </summary>
    /// <param name="quest"></param>
    /// <param name="currentTaskGroup"></param>
    /// <param name="prevTaskGroup"></param>
    private void UpdateTargetTask(Quest quest, TaskGroup currentTaskGroup, TaskGroup prevTaskGroup = null)
    {
        targetTasksByQuest.Remove(quest);

        var task = currentTaskGroup.FindTaskByTarget(target);
        if (task != null)
        {
            targetTasksByQuest[quest] = task;
            task.onStateChanged += UpdateRunningTargetTaskCount;

            UpdateRunningTargetTaskCount(task, task.State);
        }
    }

    /// <summary>
    /// Target���� �����ִ� ����
    /// </summary>
    /// <param name="quest"></param>
    private void RemoveTargetQuest(Quest quest) => targetTasksByQuest.Remove(quest);


    /// <summary>
    /// Task�� ���¿� ���� Count�� �����ϰ� 
    /// count�� 0�̸� Marker�� ���� 0�̻��̸� Marker�� ���ش�
    /// </summary>
    /// <param name="task"></param>
    /// <param name="currentState"></param>
    /// <param name="prevState"></param>
    private void UpdateRunningTargetTaskCount(Task task, TaskState currentState, TaskState prevState = TaskState.Inactive)
    {
        if (currentState == TaskState.Running)
        {
            renderer.material = markerMaterialDatas.First(x => x.category == task.Category).markerMaterial;
            currentRunningTargetTaskCount++;
        }
        else
            currentRunningTargetTaskCount--;

        gameObject.SetActive(currentRunningTargetTaskCount != 0);
    }
}
