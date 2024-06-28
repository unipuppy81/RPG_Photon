using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;

public class QuestCompletionNotifier : MonoBehaviour
{
    [SerializeField]
    private string titleDescription;    // Ÿ��Ʋ ����

    [SerializeField]
    private TextMeshProUGUI titleText;  // Ÿ��Ʋ �ؽ�Ʈ
    [SerializeField]
    private TextMeshProUGUI rewardText; // ���� �ؽ�Ʈ
    [SerializeField]
    private float showTime = 3f;    // notifier ������ �ð� ����

    // �ѹ��� ������ ����Ʈ ���� ��� ����� �뺸 ���� List
    private Queue<Quest> reservedQuests = new Queue<Quest>();
    // ���� ��� �ſ� ����
    private StringBuilder stringBuilder = new StringBuilder();

    private void Start()
    {
        var questSystem = QuestSystem.Instance;
        questSystem.onQuestCompleted += Notify;
        questSystem.onAchievementCompleted += Notify;

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        var questSysem = QuestSystem.Instance;
        if (questSysem != null)
        {
            questSysem.onQuestCompleted -= Notify;
            questSysem.onAchievementCompleted -= Notify;
        }
    }

    /// <summary>
    /// �Ϸ�� ����Ʈ ���
    /// </summary>
    /// <param name="quest"></param>
    private void Notify(Quest quest)
    {
        reservedQuests.Enqueue(quest);

        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
            StartCoroutine("ShowNotice");
        }
    }

    /// <summary>
    /// Ŭ������ ����Ʈ�� ���� �����ִ� �Լ�
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShowNotice()
    {
        var waitSeconds = new WaitForSeconds(showTime);

        Quest quest;
        while (reservedQuests.TryDequeue(out quest))
        {
            titleText.text = titleDescription.Replace("%{dn}", quest.DisplayName);
            foreach (var reward in quest.Rewards)
            {
                stringBuilder.Append(reward.Description);
                stringBuilder.Append(" ");
                stringBuilder.Append(reward.Quantity);
                stringBuilder.Append(" ");
            }
            rewardText.text = stringBuilder.ToString();
            stringBuilder.Clear();

            yield return waitSeconds;
        }

        gameObject.SetActive(false);
    }
}
