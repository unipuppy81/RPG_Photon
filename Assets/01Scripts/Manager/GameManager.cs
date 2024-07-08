using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    //public static GameManager Instance { get; private set; }

    public static bool isPlayGame = false;
    public static bool isChatting = false;
    public static bool isTradeChatting = false;
    public static bool isTradeChatInput = false;

    public string playerName;

    private int gold;

    public int Gold
    {
        set => gold = value;
        get => gold;
    }

    private Dictionary<string, int> playerGold = new Dictionary<string, int>();

    private void Awake()
    {
        /*
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ �ÿ��� �ν��Ͻ��� ����
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // �ߺ��� �ν��Ͻ��� ������ �ʵ��� �ı�
        }
        */
    }

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
        GoldManager.Instance.SetGoldText(Gold);

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

        GoldManager.Instance.SetGoldText(Gold);

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
}
