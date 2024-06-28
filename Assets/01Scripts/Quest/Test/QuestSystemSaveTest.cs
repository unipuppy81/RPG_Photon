using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestSystemSaveTest : MonoBehaviour
{
    [SerializeField]
    private Quest quest;
    [SerializeField]
    private Category category;
    [SerializeField]
    private TaskTarget target;


    void Start()
    {
        var questSystem = QuestSystem.Instance;

        if (questSystem.ActiveQuests.Count == 0)
        {
            Debug.Log("Register");

            var newQuest = questSystem.Register(quest);
        }
        else
        {
            questSystem.onQuestCompleted += (quest) =>
            {
                Debug.Log("Complete");
                PlayerPrefs.DeleteAll();
                PlayerPrefs.Save();
            };
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            QuestSystem.Instance.ReceiveReport(category, target, 1);
        }
    }
}
