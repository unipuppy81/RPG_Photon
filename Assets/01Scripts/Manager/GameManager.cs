using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public static bool isPlayGame = false;
    public static bool isChatting = false;
    public static bool isTradeChatting = false;
    public static bool isTradeChatInput = false;

    public static bool isTown = true;
    public static bool isBattle = false;

    public string playerName;

    private int gold;

    public GameObject player;

    public int Gold
    {
        set => gold = value;
        get => gold;
    }

    private Dictionary<string, int> playerGold = new Dictionary<string, int>();

    /// <summary>
    /// �÷��̾��� ��带 �����մϴ�.
    /// </summary>
    /// <param name="name">�÷��̾� �̸�</param>
    /// <param name="gold">������ ��� ��</param>
    public void SaveGold(string name, int gold)
    {
        PlayerPrefs.SetInt(name + "_Gold", gold);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// �÷��̾��� ��带 �����մϴ�.
    /// </summary>
    /// <param name="name">�÷��̾� �̸�</param>
    /// <param name="gold">������ ��� ��</param>
    public void ChangeGold(string name, int gold)
    {
        int currentGold = LoadGold(name);
        Gold = currentGold + gold;

        SaveGold(name, Gold); // ����� ��带 �����մϴ�.
        GoldManager.Instance.SetGoldText();

        Debug.Log("Gold for " + name + ": " + Gold);
    }

    /// <summary>
    /// �÷��̾��� ��带 �ε��մϴ�.
    /// </summary>
    /// <param name="playerName">�÷��̾� �̸�</param>
    /// <returns>�ε�� ��� ��</returns>
    public int LoadGold(string playerName)
    {
        int loadedGold = PlayerPrefs.GetInt(playerName + "_Gold", 0); // �÷��̾� �̸��� Ű�� ����Ͽ� ����� ��带 �ҷ��ɴϴ�.
        Debug.Log("Gold for " + playerName + ": " + loadedGold);
        Gold = loadedGold; // ���� ��� �� ������Ʈ

        GoldManager.Instance.SetGoldText();

        return Gold;
    }

    /// <summary>
    /// �÷��̾��� ��带 �����ɴϴ�.
    /// </summary>
    /// <param name="playerName">�÷��̾� �̸�</param>
    /// <returns>�ش� �÷��̾��� ���</returns>
    public int GetPlayerGold(string playerName)
    {
        if (playerGold.ContainsKey(playerName))
        {
            return playerGold[playerName];
        }
        else
        {
            return 0; // �ش� �÷��̾��� ��尡 ���� ��� 0�� ��ȯ
        }
    }


    public void ChangeGold(int gold)
    {
        int currentGold = LoadGold(playerName);
        Gold = currentGold + gold;

        SaveGold(playerName, Gold); // ����� ��带 �����մϴ�.

        Debug.Log("Gold for " + playerName + ": " + Gold);
    }

    public void GameSave()
    {
        // �÷��̾� ��ġ
        PlayerPrefs.SetFloat("playerX", player.transform.position.x);
        PlayerPrefs.SetFloat("playerY", player.transform.position.y);
        PlayerPrefs.SetFloat("playerZ", player.transform.position.z);

        // ����Ʈ
        PlayerPrefs.SetInt("QuestId", QuestManager.Instance.questId);
        PlayerPrefs.SetInt("QuestActionIndex", QuestManager.Instance.questActionIndex);

        PlayerPrefs.Save();
    }

    public void GameLoad()
    {
        if (!PlayerPrefs.HasKey("playerX"))
            return;

        float x = PlayerPrefs.GetFloat("playerX");
        float y = PlayerPrefs.GetFloat("playerY");
        float z = PlayerPrefs.GetFloat("plaeyrZ");
        int questId = PlayerPrefs.GetInt("QuestId");
        int questActionIndex = PlayerPrefs.GetInt("QuestActionIndex");

        player.transform.position = new Vector3(x, y, z);
        QuestManager.Instance.questId = questId;
        QuestManager.Instance.questActionIndex = questActionIndex;
    }

    public Vector3 GameLoad2()
    {
        if (!PlayerPrefs.HasKey("playerX"))
            return new Vector3(0,0,0);

        float x = PlayerPrefs.GetFloat("playerX");
        float y = PlayerPrefs.GetFloat("playerY");
        float z = PlayerPrefs.GetFloat("plaeyrZ");
        int questId = PlayerPrefs.GetInt("QuestId");
        int questActionIndex = PlayerPrefs.GetInt("QuestActionIndex");

        Vector3 pos = new Vector3(x, y, z);
        QuestManager.Instance.questId = questId;
        QuestManager.Instance.questActionIndex = questActionIndex;

        return pos;
    }

}
